
#include <xc.h>
#include "Comms.h"
#include "Balance.h"
#include "6MSM.h"
#include "Timer.h"

extern SysConfig cfg;
extern volatile BalData bal;
extern volatile PackData pack;
extern volatile Timer balReplyTmr;

extern volatile BOOL balBusRxFlag;
extern volatile unsigned char balBusRxData[BUS_RX_BUF_SIZE];
extern volatile unsigned int balBusRxWPos;
extern volatile unsigned int balBusRxRPos;

void sendBalPkt(void)
{
	balBusTx(CMD_GET_BAL_INFO);
	balBusTx(0);	//Hop count
	balBusTx(cfg.slaveCount);

	int i;
	for(i = 0; i < cfg.slaveCount; i++)
	{
		balBusTx(pack.loadEnable[i + 1]);
	}

}
void balBusTx(unsigned char data)
{
	while(U1STAbits.UTXBF);	//Wait for the buffer to become not full

	U1TXREG = data;
}

//Get stats based on read votlages, and generate load enables
void processBalData(void)
{
	unsigned int maxV = 0;
	unsigned int minV = 0xFFFF;
	unsigned long totalV = 0;
	unsigned int i;
	unsigned char balancer, cell;
	unsigned short balanceThreshold;
	static unsigned int maxVFiltered = 3800, minVFiltered = 3800;
	unsigned int chargeAllowance, dischargeAllowance;


	for(i = 0, balancer = 0, cell = 0; i < cfg.cellCount;)
	{
		if(cfg.cellEnable[balancer] & (1 << cell))	//If this cell is enabled
		{
			//Determine minimum, maximum, sum total and average voltages
			if(pack.voltages[balancer*6+cell] > maxV)
				maxV = pack.voltages[balancer*6+cell];

			if(pack.voltages[balancer*6+cell] < minV)
				minV = pack.voltages[balancer*6+cell];

			totalV += pack.voltages[balancer*6+cell];

			i++;
		}

		//Iterate through each balancer module and cell
		cell++;
		if(CELLS_PER_SLAVE == cell)
		{
			cell = 0;
			balancer++;
		}
	}

	//Store stats
	bal.minV = minV;
	bal.maxV = maxV;
	bal.totalV = totalV;
	bal.averageV = totalV / cfg.cellCount;

	//The balance threshold is either the cell balance voltage setting, or
	//slightly above the average voltage, whichever is greater
	balanceThreshold = MAX(cfg.cellVoltageBalance, bal.averageV + cfg.balanceOffset);

	//Go through balancers again and set the load enables now that we have the stats
	for(i = 0, balancer = 0, cell = 0; i < cfg.cellCount;)
	{
		if(cfg.cellEnable[balancer] & (1 << cell))	//If this cell is enabled
		{
			//If the cell voltage is above the balance threshold, enable the load
			if(pack.voltages[balancer*6+cell] >= balanceThreshold)
				pack.loadEnable[balancer] |= (1 << cell);
			else
				pack.loadEnable[balancer] &= ~(1 << cell);

			i++;
		}
		else
			//Disable any loads that are not on enabled cells
			pack.loadEnable[balancer] &= ~(1 << cell);

		//Iterate through each balancer module and cell
		cell++;
		if(CELLS_PER_SLAVE == cell)
		{
			cell = 0;
			balancer++;
		}
	}

	//Coloumb counting for SOC
	bal.couloumbCount += bal.current;
	//Track lifetime discharge couloumb count
	if(bal.current > 0)
		bal.lifetimeColoumbs += bal.current;

	maxVFiltered = maxVFiltered + (((long)bal.maxV - (long)maxVFiltered) * (long)cfg.maxVFilterMult) / 65536;
	minVFiltered = minVFiltered + (((long)bal.minV - (long)minVFiltered) * (long)cfg.minVFilterMult) / 65536;

	//higher value = less limiting
	dischargeAllowance = CLIP(((long)minVFiltered - cfg.cellVoltageMin) * cfg.dischargeLimitGain ,0,0xFFFF);
	chargeAllowance = CLIP(((long)cfg.cellVoltageMax - maxVFiltered) * cfg.chargeLimitGain ,0,0xFFFF);

	//If there's a fault condition disable all charging and discharging
	if(bal.faultCode != 0)
	{
		dischargeAllowance = 0;
		chargeAllowance = 0;
	}

	//Determine analog output values
	//Analog 1
	if(cfg.analog1FunctionSelect & AFS_ENABLE)
	{
		//AFS_FUNCTION: 1 == charge
		unsigned int allowance = (cfg.analog1FunctionSelect & AFS_FUNCTION) ? chargeAllowance : dischargeAllowance;
		//In digital mode, any allowance is full allowance
		if(cfg.analog1FunctionSelect & AFS_DIGITAL)
			allowance = (allowance > 0) ? 0xFFFF : 0;
		//AFS_POLARITY: 1 == more signal decreases current
		setAnalog((cfg.analog1FunctionSelect & AFS_POLARITY) ? 0xFFFF - allowance : allowance, 1);
	}
	else
		setAnalog(0, 1);

	//Analog 2
	if(cfg.analog2FunctionSelect & AFS_ENABLE)
	{
		//AFS_FUNCTION: 1 == charge
		unsigned int allowance = (cfg.analog2FunctionSelect & AFS_FUNCTION) ? chargeAllowance : dischargeAllowance;
		//In digital mode, any allowance is full allowance
		if(cfg.analog2FunctionSelect & AFS_DIGITAL)
			allowance = (allowance > 0) ? 0xFFFF : 0;
		//AFS_POLARITY: 1 == more signal decreases current
		setAnalog((cfg.analog2FunctionSelect & AFS_POLARITY) ? 0xFFFF - allowance : allowance, 2);
	}
	else
		setAnalog(0, 2);

	//If the maximum cell voltage is over the limit or there's any fault code, disallow charging
	if((bal.maxV > cfg.cellVoltageMax) || (bal.faultCode != 0))
	{	//Disable charging
		CHARGE_ENABLE = cfg.chgEnActiveHigh ? 1 : 0;
		bal.chargeEnableState = FALSE;
	}
	else
	{	//Enable charging
		CHARGE_ENABLE = cfg.chgEnActiveHigh ? 0 : 1;
		bal.chargeEnableState = TRUE;
	}
}

//Gets the voltages of cell 'cell' in the pack, accounting for disabled cells
unsigned int getCellVoltage(unsigned char cell)
{
	unsigned char c = 0, count = 0;
	unsigned char balancer = 0;
	
	while(count < cfg.cellCount)	//Get out if no cell is found
	{
		if(cfg.cellEnable[balancer] & (1 << c))	//If this cell is enabled
		{
			//Return the voltage for the cellth enabled cell
			if(0 == cell)
				return pack.voltages[balancer * CELLS_PER_SLAVE + c];
			cell--;
			count++;
		}

		//Iterate through each balancer module and cell
		c++;
		if(CELLS_PER_SLAVE == c)
		{
			c = 0;
			balancer++;
		}
	} 

	return 0;	//Cell wasn't found???
}

//Packs the loadEnables array with the load enable data
void getLoadEnables(unsigned char * loadEnables)
{
	unsigned char pos = 0, i = 0;
	unsigned char c = 0, count = 0;
	unsigned char balancer = 0;

	//Zero the load enable array
	for(i = 0; i < ((MAX_CELLS+7) / 8); i++)
		loadEnables[i] = 0;

	i = 0;
	while(count < cfg.cellCount)
	{
		if(cfg.cellEnable[balancer] & (1 << c))	//If this cell is enabled
		{
			//Pack the load enable array
			loadEnables[i] |= ((!!(pack.loadEnable[balancer] & (1 << c))) << pos);
			pos++;
			if(pos >= 8)
			{
				pos = 0;
				i++;
			}
			count++;
		}

		//Iterate through each balancer module and cell
		c++;
		if(CELLS_PER_SLAVE == c)
		{
			c = 0;
			balancer++;
		}
	} 

}

void errorShutdown(void)
{
	//Charge enable line to disabled
	CHARGE_ENABLE = cfg.chgEnActiveHigh ? 1 : 0;
	bal.chargeEnableState = FALSE;

	//Analog values to most limiting
	setAnalog((cfg.analog1FunctionSelect & AFS_POLARITY) ? 0xFFFF : 0, 1);
	setAnalog((cfg.analog2FunctionSelect & AFS_POLARITY) ? 0xFFFF : 0, 2);

}

void setLoadEn(unsigned char loadEn)
{
	LOAD1 = 0 != (loadEn & (1 << 0));
	LOAD2 = 0 != (loadEn & (1 << 1));
	LOAD3 = 0 != (loadEn & (1 << 2));
	LOAD4 = 0 != (loadEn & (1 << 3));
	LOAD5 = 0 != (loadEn & (1 << 4));
	LOAD6 = 0 != (loadEn & (1 << 5));
}

void setAnalog(unsigned int value, unsigned char channel)
{
	switch(channel)
	{
		case 1:
			OC1R = (long)value * (ANALOG_PWM_PERIOD + 1) / 0xFFFF;
			bal.analog1Value = value;
			break;
		case 2:
			OC2R = (long)value * (ANALOG_PWM_PERIOD + 1) / 0xFFFF;
			bal.analog2Value = value;
			break;
	}
}

void __attribute__((__interrupt__,no_auto_psv)) _U1RXInterrupt(void)
{
	unsigned char data;
	static unsigned char rxState = RX_STATE_IDLE;
	static unsigned char hopCount, balCount;
	static unsigned char loadEnCount;
	static unsigned short voltageCount, voltageRxCount;

	LED_GREEN = 1;
	data = U1RXREG;

	if(cfg.balBusPassthrough)
	{

		balBusRxData[balBusRxWPos++] = data;
		if(balBusRxWPos == BUS_RX_BUF_SIZE)
			balBusRxWPos = 0;
			
		balBusRxFlag = TRUE;
	}
	else
	{
		switch(rxState)
		{
			case RX_STATE_IDLE:
				switch(data)
				{
					case CMD_GET_BAL_INFO:
						rxState = RX_STATE_GET_HOP_COUNT;
					break;

					default:
						rxState = RX_STATE_IDLE;
					break;
				}
			break;

			case RX_STATE_GET_HOP_COUNT:
				hopCount = data;
				if(hopCount != cfg.slaveCount)
					bal.faultCode |= ERR_WRONG_SLAVE_COUNT;
				rxState = RX_STATE_GET_BAL_COUNT;
			break;

			case RX_STATE_GET_BAL_COUNT:
				loadEnCount = 0;
				rxState = RX_STATE_LOAD_EN;
			break;

			case RX_STATE_LOAD_EN:
				loadEnCount++;

				if(hopCount == loadEnCount)
				{
					rxState = RX_STATE_GET_VOLTAGES;
					voltageCount = hopCount * 12;
					voltageRxCount = 0;

				}
			break;

			case RX_STATE_GET_VOLTAGES:
				if(voltageRxCount < voltageCount)
				{
					*(((volatile unsigned char *)pack.voltages) + CELLS_PER_SLAVE*2 +
						voltageRxCount) = data;
				}
				voltageRxCount++;
				if(voltageRxCount == voltageCount)
				{
						rxState = RX_STATE_IDLE;
						bal.voltagesUpdated = TRUE;
						balReplyTmr.enabled = FALSE;
						balReplyTmr.count = 0;
				}
			break;

			case RX_STATE_EEPROM_READ:
				rxState = RX_STATE_IDLE;
			break;

			case RX_STATE_EEPROM_WRITE:
				rxState = RX_STATE_IDLE;
			break;

			default:
				rxState = RX_STATE_IDLE;
			break;
		}
	}
	LED_GREEN = 0;
//Clear IF
IFS0bits.U1RXIF = 0;
}