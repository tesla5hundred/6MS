/* 
 * File:   6MSS.h
 * Author: User
 *
 * Created on September 13, 2013, 10:42 PM
 */

#ifndef _6MSS_H
#define	_6MSS_H

#ifdef	__cplusplus
extern "C" {
#endif

#define LED		PORTAbits.RA5
#define LOAD1	LATBbits.LATB4
#define LOAD2	LATBbits.LATB5
#define LOAD3	LATBbits.LATB6
#define LOAD4	LATCbits.LATC6
#define LOAD5	LATBbits.LATB7
#define LOAD6	LATAbits.LATA4//PORTAbits.RA4
/*
	#define LOAD1	PORTBbits.RB4
#define LOAD2	PORTBbits.RB5
#define LOAD3	PORTBbits.RB6
#define LOAD4	PORTCbits.RC6
#define LOAD5	PORTBbits.RB7
#define LOAD6	LATAbits.LATA4//PORTAbits.RA4
	*/

#define ADC_CHAN_VBAT1	6
#define ADC_CHAN_VBAT2	5
#define ADC_CHAN_VBAT3	9
#define ADC_CHAN_VBAT4	7
#define ADC_CHAN_VBAT5	4
#define ADC_CHAN_VBAT6	2

#define NUM_CELLS		6

#define LED_PULSE_TIME	500	//LED is held on for this many us on each pulse

#define	LOAD_TIMEOUT	20	//Loads will turn off after not receiving a balance command for this many ticks
#define LOAD_EN_OFF_SETTLE_TIME	5	//Loads are turned off for this much time in ms
									//before sampling voltages to remove wiring voltage drop errors
typedef union {
    struct {
		unsigned						:2;
		unsigned saveConfig				:1;
		unsigned readCfg				:1;
		unsigned eepromWrite			:1;
		unsigned eepromRead				:1;
		unsigned getVoltages			:1;
        unsigned sendVoltages			:1;
    };
	unsigned char val;
} Flags_t;

enum {
	RX_STATE_IDLE = 0,
	RX_STATE_GET_HOP_COUNT,
	RX_STATE_GET_BAL_COUNT,
	RX_STATE_LOAD_EN,
	RX_STATE_GET_VOLTAGES,

	RX_STATE_SET_ADDR,
	RX_STATE_EEPROM_WRITE,
	RX_STATE_WRITE_CFG_DATA

};

enum {
	CMD_GET_BAL_INFO = 0x1,
	CMD_READ_EEPROM,
	CMD_WRITE_EEPROM,
	CMD_READ_CFG,
	CMD_WRITE_CFG,
	CMD_SAVE_CFG,
	CMD_SET_ADDR
};

#ifdef	__cplusplus
}
#endif

#endif	/* 6MSS_H */

