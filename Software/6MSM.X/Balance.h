/* 
 * File:   Balance.h
 * Author: User
 *
 * Created on September 22, 2013, 2:33 PM
 */

#ifndef BALANCE_H
#define	BALANCE_H

#ifdef	__cplusplus
extern "C" {
#endif

#include "types.h"


#define CELLS_PER_SLAVE		6
#define MAX_SLAVE_BOARDS	42
#define	MAX_CELLS			((MAX_SLAVE_BOARDS + 1) * CELLS_PER_SLAVE)

#define ADC_BUF_VBATT1		5
#define ADC_BUF_VBATT2		6
#define ADC_BUF_VBATT3		1
#define ADC_BUF_VBATT4		0
#define ADC_BUF_VBATT5		4
#define ADC_BUF_VBATT6		2
#define	ADC_BUF_TEMP		3

#define	LOAD1				PORTBbits.RB15
#define	LOAD2				PORTBbits.RB14
#define	LOAD3				PORTAbits.RA7
#define	LOAD4				PORTAbits.RA10
#define	LOAD5				PORTBbits.RB13
#define	LOAD6				PORTBbits.RB12

//EEPROM organization

#define EEPROM_SIZE_IDENTITY	sizeof(Identity_t)
#define EEPROM_SIZE_CAL			sizeof(Cal_t)
#define EEPROM_SIZE_CFG			sizeof(SysConfig)


#define EEPROM_OFFSET_IDENTITY	0x00
#define EEPROM_OFFSET_CAL		0x80
#define EEPROM_OFFSET_CONFIG	0xA0


typedef struct
{
	unsigned short gain[6];
	short offset[6];
} Cal_t;

enum {
	AFS_ENABLE = 1,
	AFS_POLARITY = 2,
	AFS_FUNCTION = 4,
	AFS_DIGITAL = 8
};

typedef struct {
	unsigned char cellCount;
	unsigned char cellEnable[MAX_SLAVE_BOARDS+1];
	unsigned char slaveCount;
	unsigned long canID;		//ID of this unit
	unsigned long canDstID;	//ID of destination packets
	unsigned int balPollPeriod;	//period at which to poll the balancer units (ms)
	unsigned int balReplyTimeout;

	//Balancing parameters
	unsigned int cellVoltageBalance;		//Minimum cell voltage for balancing to occur
	unsigned int balanceOffset;				//Amount cell has to be above average cell voltage to enable load
	unsigned int cellVoltageMax;			//Maximum cell voltage. Charge is cut off completely at this voltage
	unsigned int cellVoltageMin;			//Minimum cell voltage. Discharge is cut off completely at this voltage
	BOOL balBusPassthrough;
	BOOL chgEnActiveHigh;					//TRUE indicates that charge enable is active high (not grounded)
	unsigned char analog1FunctionSelect;	//Selects function of analog 1 output
	unsigned char analog2FunctionSelect;	//Selects function of analog 2 output
											//Bits
											//3: Digital		0: Signal is analog  1: Signal is TTL digital output
											//2: Function		0: discharge limit  1: charge limit
											//1: Polarity		0: lower signal decreases draw  1: higher signal decreases draw
											//0: enable			0: disabled  1: enabled
	unsigned int maxVFilterMult;			//Typical value ~6000
	unsigned int minVFilterMult;
	unsigned int chargeLimitGain;
	unsigned int dischargeLimitGain;		//typical value ~300
	unsigned long currentGain;
	unsigned int currentOffset;


} SysConfig;

typedef struct {
	unsigned char model[16];
	unsigned char serial[16];
	unsigned char version[16];
} Identity_t;

typedef struct {
	unsigned short voltages[MAX_CELLS];
	unsigned char loadEnable[MAX_SLAVE_BOARDS+1];
} PackData;

typedef struct {
	BOOL voltagesUpdated;
	unsigned int minV;
	unsigned int maxV;
	unsigned int averageV;
	unsigned long totalV;
	BOOL chargeEnableState;		//TRUE = charger enabled
	unsigned int analog1Value;	// 0xFFFF = max analog value
	unsigned int analog2Value;
	long current;				//Measured battery current
	unsigned int faultCode;
	int temp;					//Temperature in tenths of degrees C
	long long couloumbCount;
	long long lifetimeColoumbs;
	unsigned long svICount;
	unsigned long slICount;
	unsigned long sdICount;
	unsigned long svLCount;
	unsigned long slLCount;
	unsigned long sdLCount;
} BalData;

enum {
	CMD_GET_BAL_INFO = 0x1,
	CMD_READ_EEPROM,
	CMD_WRITE_EEPROM
};

enum {
	RX_STATE_IDLE = 0,
	RX_STATE_GET_HOP_COUNT,
	RX_STATE_GET_BAL_COUNT,
	RX_STATE_LOAD_EN,
	RX_STATE_GET_VOLTAGES,

	RX_STATE_EEPROM_READ,
	RX_STATE_EEPROM_WRITE
};

enum {
	ERR_WRONG_SLAVE_COUNT = 1,
	ERR_BALANCER_TIMEOUT = 2
	
};

void sendBalPkt(void);
void balBusTx(unsigned char data);
void setLoadEn(unsigned char loadEn);
void setAnalog(unsigned int value, unsigned char channel);
void errorShutdown(void);
unsigned int getCellVoltage(unsigned char cell);
void getLoadEnables(unsigned char * loadEnables);
#ifdef	__cplusplus
}
#endif

#endif	/* BALANCE_H */

