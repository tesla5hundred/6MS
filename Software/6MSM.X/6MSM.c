
/*
 * File:   6MSM.c
 * Author: User
 *
 * Created on September 18, 2013, 6:26 PM
 */

#define FOSC    (40000000ULL)
#define FCY     (FOSC/2 / 16)
#define FP		(FOSC/2)


#include "config.h"

#include "Balance.h"
#include "6MSM.h"
#include "Timer.h"
#include "ECAN1Config.h"
#include "common.h"
#include "types.h"
#include "Eeprom.h"
#include "ADC.h"

#include <stdio.h>
#include <stdlib.h>
#include <libpic30.h>
#define BOOL unsigned char
#define TRUE (1==1)
#define FALSE (!TRUE)

void balBusTx(unsigned char data);
void loadConfig(void);
unsigned short getADCBuf(unsigned char buf);
void getBal(int data);
void flashLED(int data);
void balReplyTimeout(int data);
void init(void);

volatile SysConfig cfg;
volatile Identity_t identity;

volatile BalData bal;
volatile PackData pack;

// CAN Messages in RAM


volatile Timer balReplyTmr;
Cal_t config;

int main(int argc, char** argv)
{

	char data;
	
	volatile Timer t, balPktTmr, statsTimer;
	cfg.cellCount = 12;
	cfg.cellEnable[0] = 0b111111;
	cfg.cellEnable[1] = 0b111111;
	cfg.slaveCount = 1;
	cfg.canID = 0x0000000A;
	cfg.canDstID = 0x0F000000;
	cfg.balPollPeriod = 1000;
	cfg.balReplyTimeout = 100;

	cfg.cellVoltageBalance = 3900;		//Minimum cell voltage for balancing to occur
	cfg.balanceOffset = 50;				//Amount cell has to be above average cell voltage to enable load
	cfg.cellVoltageMax = 4100;			//Maximum cell voltage. Charge is cut off completely at this voltage
	cfg.cellVoltageMin = 2700;			//Minimum cell voltage. Discharge is cut off completely at this voltage
	cfg.balBusPassthrough = FALSE;

	bal.sdICount = bal.sdLCount = bal.slICount = bal.slLCount = bal.svICount = bal.svLCount = 0;

	init();
	initADC();
	loadConfig();
	initTimer(&t, 199 /*period*/, TRUE /*rep*/, TRUE /*ena*/, flashLED /*callback*/, NULL /*userData*/);
	initTimer(&statsTimer, 99 /*period*/, TRUE /*rep*/, TRUE /*ena*/, NULL /*callback*/, NULL /*userData*/);
	initTimer(	&balPktTmr,
				cfg.balPollPeriod - 1 /*period*/,
				TRUE /*rep*/,
				TRUE /*ena*/,
				getBal /*callback*/,
				NULL /*userData*/);
	initTimer(	&balReplyTmr,
				cfg.balReplyTimeout - 1 /*period*/,
				FALSE /*rep*/,
				FALSE /*ena*/,
				balReplyTimeout /*callback*/,
				NULL /*userData*/);


	registerTimer(&t);
	registerTimer(&statsTimer);
	registerTimer(&balPktTmr);
	registerTimer(&balReplyTmr);
	
	t.enabled = TRUE;
	balPktTmr.enabled = TRUE;
	statsTimer.enabled = TRUE;
	int adcVal;
	while(1)
	{
		processComms();

		if(statsTimer.elapsed)
		{
			statsTimer.elapsed = FALSE;
			bal.current = ((long long)(readADC() - (int)cfg.currentOffset) * cfg.currentGain) >> 16;
		}
		

		if(bal.voltagesUpdated)
		{
			bal.voltagesUpdated = FALSE;
			bal.faultCode &= ~ERR_BALANCER_TIMEOUT;	//Clear balancer timeout error on successfull receive
			processBalData();
			setLoadEn(pack.loadEnable[0]);
		}
		if(bal.faultCode != 0)
		{
			LED_RED = 1;
			errorShutdown();
		}
		else
		{
			LED_RED = 0;
		}

		//If a UART receive overrun occurs, Rx interupts won't occur, so clear the error bit if an overrun occurs.
		if(U1STAbits.OERR)
		{
			U1STAbits.OERR = 0;
		}
	}

	return (EXIT_SUCCESS);
}

void flashLED(int data)
{
	//LED_RED = !LED_RED & 0;
}
void getBal(int data)
{
	//AD1CON1bits.SAMP = 0;	//Start AD conversion
	AD1CON1bits.SSRC = 0b111;	//Internal counter ends sampling and starts conversion

	if(!cfg.balBusPassthrough)
	{
            if(cfg.slaveCount != 0)
            {
		sendBalPkt();
		balReplyTmr.enabled = TRUE;
            }
	}
}

void balReplyTimeout(int data)
{
	bal.faultCode |= ERR_BALANCER_TIMEOUT;
}

void loadConfig()
{
	EepromReadBlock(&cfg, EEPROM_OFFSET_CONFIG, sizeof(cfg));
	EepromReadBlock(&config, EEPROM_OFFSET_CAL, sizeof(config));
	EepromReadBlock(&identity, EEPROM_OFFSET_IDENTITY, sizeof(identity));
}


void init(void)
{

	//Set up PLL
	//M = 152, N1 = 7, N2 = 4
	//Fout = 7.37 * M / N1 / N2 = 40.00857
	CLKDIV = 0b0101000001000101;	//Fcy/32, FRC/1, N2 = 4, N1 = 7
	CLKDIVbits.DOZE = 0b100;	//1:6
	CLKDIVbits.DOZEN = 1;

	PLLFBD = 152-2;	//M=152

	//Transition to FRCPLL
	__builtin_write_OSCCONH( 0x01 );
	__builtin_write_OSCCONL( OSCCON | 0x01 );

	 // Wait for Clock switch to occur
	 while ( OSCCONbits.COSC != 0b001 );
	 // Wait for PLL to lock
	 while ( OSCCONbits.LOCK != 1 );


	TRISA = 0b0000000000000011;
	TRISB = 0b0000001000000111;
	TRISC = 0b0000000000111111;

	//PORTCbits.RC4 = PORTCbits.RC5 = 1;

	//Set up pin mapping
	RPINR18 = 0x33;			//Battery board bus UART1 Rx on RPI51
	RPINR26 = 0x29;			//CAN1 Rx on RP41

	RPOR0bits.RP20R = 0x01;	//UART1 TX
	RPOR2bits.RP39R = 0x11;	//Analog2 PWM on OC2
	RPOR3bits.RP40R = 0x10;	//Analog1 PWM on OC1
	RPOR5bits.RP54R = 0x0E;	//CAN1 TX

	//Set up UART1
	U1MODE = 0b1000100000000000;	// Uart enabled, continue in idle, no flow
									// control, no wake up, BRG standard (16 clocks), 8N1
	U1STA = 0b1100010000000000;		// Interrupt on char -> tx buffer, TX idle
									// state 1, transmiter enabled, interrupt on char received,
	U1BRG = FP / 20833 / 16 - 1;	// = (Fp / Baudrate / 16) - 1 = (3,685,000 / 20,833 / 16) - 1

	//Set up Timer 2
	T2CONbits.TON = 0; // Disable Timer
	T2CONbits.TCS = 0; // Select internal instr
	T2CONbits.TGATE = 0; // Disable Gated Timer mode
	T2CONbits.TCKPS = 0b00; // Select 1:8 Prescaler
	PR2 = FP / TICK_FREQUENCY - 1;		//Set frequency
	TMR2 = 0;						//Clear timer register
	T2CONbits.TON = 1; // Enable Timer


	//Set up output compare 1 (Analog 1 PWM)
	OC1CON1 = 0;				//It is a good practice to initially clear the control bits
	OC1CON2 = 0;
	OC1CON1bits.OCTSEL = 0x07;	//Peripheral clock
	OC1R = 500;					//PWM output of 0
	OC1RS = ANALOG_PWM_PERIOD;	//PWM Period
	OC1CON1bits.OCM = 6; /* This selects the Edge Aligned PWM mode*/
	OC1CON2bits.SYNCSEL = 0x1F; /* No trigger or sync source is selected */

	//Set up output compare 2 (Analog 2 PWM)
	OC2CON1 = 0;				//It is a good practice to initially clear the control bits
	OC2CON2 = 0;
	OC2CON1bits.OCTSEL = 0x07;	//Peripheral clock
	OC2R = 1500;				//PWM output of 0
	OC2RS = ANALOG_PWM_PERIOD;	//PWM Period
	OC2CON1bits.OCM = 6; /* This selects the Edge Aligned PWM mode*/
	OC2CON2bits.SYNCSEL = 0x1F; /* No trigger or sync source is selected */

	//Set up output compare 2 (Analog 2 PWM)

	AD1CON1 = 0b0000001000000110;	//Module off, continue in idle, 10 bit, fractional (left justified) result, manual sample end, Sampling begins automatically after last conversion
	AD1CON2 = 0b0010010000011100;	//Vref+/Avss, scan, convert CH0, interrupt every 7 conversion results, fill buffer from start address, always use sample A selects
	AD1CON3 = 0b1001111111111111;	//Internal RC clock, 31TAC sample time
	AD1CON4 = 0b0000000000000000;	//No DMA
	AD1CHS123 = 0;
	AD1CHS0 = 0b0000000000000000;	//Channel 0 negative Vref Vrefl
	AD1CSSH = 0;
	AD1CSSL = 0b0000000111010111;	//Scan AD channels 0, 1, 2, 6, 7, 8
	ANSELA =  0b0000000000000011;
	ANSELB =  0b0000000000000111;	//VREF+ on RB1
	ANSELC =  0b0000000000000111;

	AD1CON1bits.ADON = 1;		//Turn AD module on

	//I2C
	I2C1BRG = 395;	//100kHz
	I2C1CON = 0b1000000000000000;


/* ECAN1 Initialisation
   Configure DMA Channel 0 for ECAN1 Transmit
   Configure DMA Channel 2 for ECAN1 Receive */
	ecan1Init(cfg.canID);
	dma0init();
	dma2init();

/* Enable ECAN1 Interrupt */

	IEC2bits.C1IE = 1;
	C1INTEbits.TBIE = 1;
	C1INTEbits.RBIE = 1;


	// Interrupt nesting enabled here
	INTCON1bits.NSTDIS = 0;


	IPC2bits.U1RXIP = 5;	//Set UART 1 interrupt priority
	IPC1bits.T2IP = 4;		//Set Timer 2 interrupt priority
	IPC8bits.C1IP = 3;		//Set CAN1 interrupt priority

	IFS0bits.T2IF = 0;		//Clear IF and enable timer interrupt
	IEC0bits.T2IE = 1;

	IFS0bits.U1RXIF = 0;	//Clear IF and enable UART1 interrupt
	IEC0bits.U1RXIE = 1;

	IEC0bits.AD1IE = 1;		//Enable ADC1 interrupts

	INTCON2bits.GIE = 1;	//Enable global interrupts

}




void __attribute__((__interrupt__, auto_psv)) _T2Interrupt(void)
{
	processTimers();
	IFS0bits.T2IF = 0;	//Clear IF
}

const unsigned char CHANNELS[] = {	ADC_BUF_VBATT1,
									ADC_BUF_VBATT2,
									ADC_BUF_VBATT3,
									ADC_BUF_VBATT4,
									ADC_BUF_VBATT5,
									ADC_BUF_VBATT6};

unsigned short getADCBuf(unsigned char buf)
{
	switch(buf)
	{
		case 0: return ADC1BUF0;
		case 1: return ADC1BUF1;
		case 2: return ADC1BUF2;
		case 3: return ADC1BUF3;
		case 4: return ADC1BUF4;
		case 5: return ADC1BUF5;
		case 6: return ADC1BUF6;
		case 7: return ADC1BUF7;
		case 8: return ADC1BUF8;
		case 9: return ADC1BUF9;
		case 10: return ADC1BUFA;
		case 11: return ADC1BUFB;
		case 12: return ADC1BUFC;
		case 13: return ADC1BUFD;
		case 14: return ADC1BUFE;
		case 15: return ADC1BUFF;
	}
}

typedef struct { long x; long y; } coord_t;

//RT curve for NTCLE400E3103H thermistor, R in Ohms, T in tenths of degrees C
coord_t c[26] =
{
	//{R,T}
	{1070, 850},
	{1256, 800},
	{1481, 750},
	{1753, 700},
	{2084, 650},
	{2490, 600},
	{2989, 550},
	{3605, 500},
	{4372, 450},
	{5330, 400},
	{6535, 350},
	{8059, 300},
	{10000, 250},
	{12488, 200},
	{15698, 150},
	{19872, 100},
	{25339, 50},
	{32554, 0},
	{42157, -50},
	{55046, -100},
	{72500, -150},
	{96358, -200},
	{129287, -250},
	{175200, -300},
	{239900, -350},
	{332094, -400}
};

#define THERMISTOR_PULL	10000	//Thermistor pullup resistance in ohms

long interp( coord_t* c, long x, int n )
{
    int i;

    for( i = 0; i < n-1; i++ )
    {
        if ( c[i].x <= x && c[i+1].x >= x )
        {
            long diffx = x - c[i].x;
            long diffn = c[i+1].x - c[i].x;

            return c[i].y + ( c[i+1].y - c[i].y ) * diffx / diffn;
        }
    }

    return 0; // Not in Range
}

void __attribute__((interrupt, auto_psv))_AD1Interrupt(void)
{
	unsigned short * adcBuf = &ADC1BUF0;
	unsigned short res;
	long resistance;
	char i;

	AD1CON1bits.SSRC = 0b000;	//Manual conversion start (don't do any more conversions automatically)

	for(i = 0; i < CELLS_PER_SLAVE; i++)
	{
		pack.voltages[i] = (((unsigned long)getADCBuf(CHANNELS[i]) * config.gain[i]) >> 16) - config.offset[i];
	}
	pack.voltages[1] -= pack.voltages[0];		//Battery 2 voltages is battery 2 sense minus battery 1 sense
	
	//V = Rt / (Rt+10k)
	//V(Rt+10k) = Rt = V*Rt + V*10k
	//V*Rt + V*10k =  Rt
	//V*Rt - Rt = -V*10k
	//Rt(V - 1) = -V*10k
	//Rt = -V*10k / (V - 1)
	//Rt = V*10k / (1 - V)
	//Rt = V*10k / (1 - V)
	//Rt = ADCVal*10k / 65536 / (1 - ADCVal/65536)
	//Rt = ADCVal*10k / 65536(1 - ADCVal/65536)
	//Rt = ADCVal*10k / (65536 - ADCVal)
	res = getADCBuf(ADC_BUF_TEMP);
	
	if(res != 0xFFFF)
		resistance = (long)res * THERMISTOR_PULL / (0xFFFF - res);
	else
		resistance = 0x7FFFFFFF;

	bal.temp = interp( c, resistance, 27 );

        //If we are running master only, set the flag that voltages are updated (would normally be set in balancer bus receive)
        if(cfg.slaveCount == 0)
            bal.voltagesUpdated = TRUE;

	IFS0bits.AD1IF = 0;
}

void __attribute__((interrupt,no_auto_psv)) _AddressError(void)
{
	while(1)
	{
		LED_RED = 1;
		LED_RED = 1;
		LED_RED = 0;
		LED_RED = 0;
		LED_RED = 0;
		LED_RED = 1;
	}

}
void __attribute__((interrupt,no_auto_psv)) _HardTrapError(void)
{
	while(1)
	{
		LED_RED = 1;
		LED_RED = 1;
		LED_RED = 0;
		LED_RED = 0;
		LED_RED = 1;
		LED_RED = 0;
	}
}
void __attribute__((interrupt,no_auto_psv)) _StackError(void)
{
	while(1)
	{
		LED_RED = 1;
		LED_RED = 1;
		LED_RED = 0;
		LED_RED = 0;
		LED_RED = 1;
		LED_RED = 1;
	}

}
void __attribute__((interrupt,no_auto_psv)) _MathError(void)
{
	while(1)
	{
		LED_RED = 1;
		LED_RED = 1;
		LED_RED = 0;
		LED_RED = 1;
		LED_RED = 0;
		LED_RED = 0;
	}

}
void __attribute__((interrupt,no_auto_psv)) _DMACError(void)
{
	while(1)
	{
		LED_RED = 1;
		LED_RED = 1;
		LED_RED = 0;
		LED_RED = 1;
		LED_RED = 0;
		LED_RED = 1;
	}

}
void __attribute__((interrupt,no_auto_psv)) _SoftTrapError(void)
{
	while(1)
	{
		LED_RED = 1;
		LED_RED = 1;
		LED_RED = 0;
		LED_RED = 1;
		LED_RED = 1;
		LED_RED = 0;
	}

}
