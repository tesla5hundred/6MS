/* 
 * File:   6MSS.c
 * Author: User
 *
 * Created on September 4, 2013, 2:26 PM
 */
#include "config.h"     //Configuration bit settings
#include "types.h"
#include <xc.h>
#include <stdio.h>
#include <stdlib.h>

#include "6MSS.h"

__EEPROM_DATA(0xF1, 0x13, 0xE2, 0x27, 0xF1, 0x13, 0xF1, 0x13);
__EEPROM_DATA(0xF1, 0x13, 0xF1, 0x13, 0x00, 0x00, 0x00, 0x00);
__EEPROM_DATA(0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00);

#define _XTAL_FREQ 2000000
#define __delay_us(x) _delay((unsigned long)((x)*(_XTAL_FREQ/4000000.0)))
#define __delay_ms(x) _delay((unsigned long)((x)*(_XTAL_FREQ/4000.0)))

#define TICK_FREQUENCY 10
#define TMR1_PRESCALER 8
#define TMR1_LOAD_VAL (0x10000 - (_XTAL_FREQ / TMR1_PRESCALER / 4 / TICK_FREQUENCY))


volatile unsigned char loadEn = 0;
volatile short voltages[6];

volatile unsigned char eepromAddress;
volatile unsigned char eepromData;

void init(void);
unsigned short adConvert(unsigned char chan);
void loadConfig(void);
void saveConfig();
void getVoltages(unsigned short * voltages);
void transmitVoltages(void);
void setLoadEn(unsigned char loadEn);
void transmitEEPROMData(unsigned char addr);
void transmitData(unsigned char data);
void writeConfigStruct(unsigned char addr, unsigned char data);
unsigned char readConfigStruct(unsigned char addr);

const unsigned char ADCHANNELS[NUM_CELLS] = {	ADC_CHAN_VBAT1,
												ADC_CHAN_VBAT2,
												ADC_CHAN_VBAT3,
												ADC_CHAN_VBAT4,
												ADC_CHAN_VBAT5,
												ADC_CHAN_VBAT6 };


typedef struct
{
	unsigned short gain[6];
	short offset[6];
} Config_t;

volatile Config_t config;
volatile Flags_t flags;

volatile unsigned char loadTimer = 0;

/*
 * 
 */
void main(void)
{
	flags.val = 0;
	init();
	loadConfig();
	
	while(1)
	{
		if(flags.getVoltages)
		{
			setLoadEn(0);
			__delay_ms(LOAD_EN_OFF_SETTLE_TIME);
			getVoltages(voltages);
			flags.getVoltages = 0;
		}

		if(flags.sendVoltages)
		{
			transmitVoltages();
			setLoadEn(loadEn);
			flags.sendVoltages = 0;
			LED = 1;
			__delay_us(500);
			LED = 0;
		}

		if(flags.eepromRead)
		{
			transmitData(eeprom_read(eepromAddress));
			flags.eepromRead = 0;
		}

		if(flags.eepromWrite)
		{
			eeprom_write(eepromAddress, eepromData);
			while(EECON1bits.WR);	//Wait for write to complete
			transmitData(CMD_WRITE_EEPROM);
			flags.eepromWrite = 0;
		}

		if(flags.readCfg)
		{
			transmitData(readConfigStruct(eepromAddress));
			flags.readCfg = 0;
		}

		if(flags.saveConfig)
		{
			saveConfig();
			transmitData(CMD_SAVE_CFG);
			flags.saveConfig = 0;
		}

		if(PIR1bits.TMR1IF)
		{
			PIR1bits.TMR1IF = 0;
			T1CONbits.TMR1ON = 0;
			TMR1H = TMR1_LOAD_VAL >> 8;
			TMR1L = TMR1_LOAD_VAL & 0xFF;
			T1CONbits.TMR1ON = 1;

			if(LOAD_TIMEOUT == loadTimer)
			{
				loadEn = 0;
				setLoadEn(0);		//All loads off if timeout
			}
			else
				loadTimer++;

		}
		
	}
    
}

void getVoltages(unsigned short * voltages)
{
	unsigned short res;
	unsigned short temp;

	for(char i = 0; i < NUM_CELLS; i++)
	{
		res = adConvert(ADCHANNELS[i]);

		//Compute the following using temporary storage becaue xc8 compiler chokes on the long expression:
		//voltages[i] = (((unsigned long)res * config.gain[i]) >> 16) - config.offset[i];
		temp = (((unsigned long)res * config.gain[i]) >> 16);
		voltages[i] = temp - config.offset[i];
	}
	voltages[1] -= voltages[0];		//Battery 2 voltages is battery 2 sense minus battery 1 sense

}

unsigned short adConvert(unsigned char chan)
{
	ADCON0 = ((chan & 0x1F) << 2) | 0x01;	//Select channel while leaving ADC on
	__delay_us(8);			//Wait acquisition time
	ADCON0bits.ADGO = 1;	//Start covnersion
	while(ADCON0bits.ADGO);	//Wait for conversion to complete
	return (ADRESH << 8 | ADRESL);
}

//1mV per count

void loadConfig()
{
	for(char i = 0; i < sizeof(config); i++)
	{
		*((volatile unsigned char *)(((volatile unsigned char *)(&config)) + i)) = eeprom_read(i);

	}
}

void saveConfig()
{
	for(char i = 0; i < sizeof(config); i++)
	{
		eeprom_write(i, *((volatile unsigned char *)(((volatile unsigned char *)(&config)) + i)));
		while(EECON1bits.WR);	//Wait for write to complete
	}
}

void init(void)
{

	//OSCCON
	OSCCON = 0b01100010;	//Internal RC oscillator at 2MHz
	OSCTUNE = 0b00100000;	//Minimum frequency
	//IO pin setup
	TRISA = 0b00001111;
	TRISB = 0b00000000;
	TRISC = 0b10101111;
	ANSELA = 0b00000100;
	ANSELC = 0b10001111;

	//AD Converter setup
	FVRCON = 0b10000010;	//FVR on, no temp indicator, no Comp/DAC, ADC reference x2 (2.048V)
	ADCON0 = 0b00000001;	//Converter on, channel 0
	ADCON1 = 0b00000011;	//MSB justified, FOSC/2 (ADC Clk max 1MHz), VREF- to GND, VREF+ to FVR module

	//Timer1
	T1CON = 0b00110000;		//1:8 prescaler, fosc/4, timer off
	T1GCON = 0b00000000;	//No gating
	PIR1bits.TMR1IF = 0;
	TMR1H = TMR1_LOAD_VAL >> 8;
	TMR1L = TMR1_LOAD_VAL & 0xFF;
	T1CONbits.TMR1ON = 1;

	//UART

	APFCON0 = 0b10000100;	//RX on RC5, TX on RC4
	TXSTA = 0b00100100;	//Transmit enabled, BRG high range
	RCSTA = 0b10010000;	//Serial port enabled, Receive enabled
	BAUDCON = 0b00010000;	//TX inverted
	SPBRGH = 0;
	SPBRGL = 5;
	RCIE = 1;



	PEIE = 1;	//Enable peripheral interrupts

	ei();	//Enable all interrupts


}

void transmitVoltages(void)
{

	for(char i = 0; i < 12; i++)
	{
		while(0 == TRMT);	//Wait for TSR to empty
		TXREG = *(((unsigned char *)voltages) + i);
	}
}

void transmitEEPROMData(unsigned char addr)
{
	while(0 == TRMT);	//Wait for TSR to empty
	TXREG = eeprom_read(addr);

}

void transmitData(unsigned char data)
{
	while(0 == TRMT);	//Wait for TSR to empty
	TXREG = data;
}

void writeConfigStruct(unsigned char addr, unsigned char data)
{
	*((volatile unsigned char *)(((volatile unsigned char *)(&config)) + addr)) = data;
}

unsigned char readConfigStruct(unsigned char addr)
{
	return *((volatile unsigned char *)(((volatile unsigned char *)(&config)) + addr));
}

void setLoadEn(unsigned char loadEn)
{
	
	LOAD1 = (loadEn & (1 << 0)) ? 1 : 0;
	LOAD2 = (loadEn & (1 << 1)) ? 1 : 0;
	LOAD3 = (loadEn & (1 << 2)) ? 1 : 0;
	LOAD4 = (loadEn & (1 << 3)) ? 1 : 0;
	LOAD5 = (loadEn & (1 << 4)) ? 1 : 0;
	LOAD6 = (loadEn & (1 << 5)) ? 1 : 0;
	
	
}

void interrupt isr_func(void)
{
	static char rxState = RX_STATE_IDLE;
	static unsigned char hopCount;
	static unsigned char balCount;
	static unsigned char loadEnCount;
	static unsigned short voltageReTxCount;
	unsigned char data;

	if(RCIE & RCIF)
	{
		data = RCREG;
		switch(rxState)
		{
			case RX_STATE_IDLE:
				switch(data)
				{
					case CMD_GET_BAL_INFO:
						TXREG = CMD_GET_BAL_INFO;
						flags.getVoltages = 1;
						loadTimer = 0;
						rxState = RX_STATE_GET_HOP_COUNT;
					break;

					case CMD_READ_EEPROM:
						flags.eepromRead = 1;
						rxState = RX_STATE_IDLE;
					break;

					case CMD_WRITE_EEPROM:
						rxState = RX_STATE_EEPROM_WRITE;
					break;

					case CMD_READ_CFG:
						flags.readCfg = 1;
						rxState = RX_STATE_IDLE;
					break;

					case CMD_WRITE_CFG:
						rxState = RX_STATE_WRITE_CFG_DATA;
					break;

					case CMD_SAVE_CFG:
						flags.saveConfig = 1;
						rxState = RX_STATE_IDLE;
					break;

					case CMD_SET_ADDR:
						rxState = RX_STATE_SET_ADDR;
					break;
				}
			break;
			
			case RX_STATE_GET_HOP_COUNT:
				hopCount = data;
				TXREG = hopCount + 1;
				rxState = RX_STATE_GET_BAL_COUNT;
			break;

			case RX_STATE_GET_BAL_COUNT:
				balCount = data;
				loadEnCount = 0;
				TXREG = balCount;
				rxState = RX_STATE_LOAD_EN;
			break;

			case RX_STATE_LOAD_EN:
				if(hopCount == loadEnCount++)
				{
					loadEn = data;
					TXREG = data;
				}
				if(balCount == loadEnCount)
				{
					if(0 == hopCount)
					{
						flags.sendVoltages = 1;
						rxState = RX_STATE_IDLE;
					}
					else
					{
						rxState = RX_STATE_GET_VOLTAGES;
						voltageReTxCount = hopCount * 12;
						loadEnCount = 0;
					}
				}
			break;

			case RX_STATE_GET_VOLTAGES:
				if(voltageReTxCount--)
				{
					TXREG = data;
				}
				if(0 == voltageReTxCount)
				{
						flags.sendVoltages = 1;
						rxState = RX_STATE_IDLE;
				}
			break;

			case RX_STATE_SET_ADDR:
				eepromAddress = data;
				rxState = RX_STATE_IDLE;
			break;

			case RX_STATE_WRITE_CFG_DATA:
				writeConfigStruct(eepromAddress, data);
				rxState = RX_STATE_IDLE;
			break;

			case RX_STATE_EEPROM_WRITE:
				eepromData = data;
				flags.eepromWrite = 1;
				rxState = RX_STATE_IDLE;
			break;

			default:
				rxState = RX_STATE_IDLE;
			break;
		}
		RCIF = 0;
	}

}

/*
 * Balance packet
 Send
		|CMD_GET_BAL_INFO	|	Command code
		|hopCount			|	Increments each time packet is retransmited. 1st balancer receives 0 in this position
		|balCount			|	Number of slave boards in the system
		|loadEn0			|	//Indicates which loads to enable. LSb controls load across cell 1, bit 5 controls load across cell 6
		|loadEn1			|	loadEn0 is used by the 1st slave board, loadEn1 by the 2nd, ect. selected by hopCount
		|...				|
		|loadEnN			|
		|voltageL0_0		|	Voltages for the first slave board. 16 bit value, 1 mV/ bit, little endian
		|voltageH0_0		|
		|voltageL1_0		|
		|voltageH1_0		|
		|...				|
		|voltageL5_0		|
		|voltageH5_0		|
		|...				|
		|voltageL0_X		|
		|voltageH0_X		|
		|voltageL1_X		|
		|voltageH1_X		|
		|...				|
		|voltageL5_X		|
		|voltageH5_X		|

 Response
		|CMD_GET_BAL_INFO	|	Command code
		|hopCount			|	Increments each time packet is retransmited. 1st balancer receives 0 in this position
		|balCount			|	Number of slave boards in the system
		|loadEn0			|	//Indicates which loads to enable. LSb controls load across cell 1, bit 5 controls load across cell 6
		|loadEn1			|	loadEn0 is used by the 1st slave board, loadEn1 by the 2nd, ect. selected by hopCount
		|...				|
		|loadEnN			|
		|voltageL0_0		|	Voltages for the first slave board. 16 bit value, 1 mV/ bit, little endian
		|voltageH0_0		|
		|voltageL1_0		|
		|voltageH1_0		|
		|...				|
		|voltageL5_0		|
		|voltageH5_0		|
		|...				|
		|voltageL0_X+1		|
		|voltageH0_X+1		|
		|voltageL1_X+1		|
		|voltageH1_X+1		|
		|...				|
		|voltageL5_X+1		|
		|voltageH5_X+1		|

 * Read EEPROM
 Send
		|CMD_READ_EEPROM	|	Command code
		|addressL			|	Address to read from
		|addressH			|

 Response
		|CMD_READ_EEPROM	|	Command code
		|data				|	data that was read


 * Write EEPROM
		|CMD_WRITE_EEPROM	|	Command code
		|EE_WRITE_CODE0		|	Key to enable EEPROM write
		|EE_WRITE_CODE1		|
		|EE_WRITE_CODE2		|
		|EE_WRITE_CODE3		|
		|addressL			|	Address to write to
		|addressH			|
		|data				|	Data to write

 Response
 		|CMD_WRITE_EEPROM	|	Command code
		|data				|	Data read back out of eeprom after write







*/
