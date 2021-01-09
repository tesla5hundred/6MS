#include <xc.h>
#include "I2C.h"

BOOL I2CStart(unsigned char addr, BOOL read)
{
	//Send start condition and wait for completion
	I2C1CONbits.SEN = 1;
	while(I2C1CONbits.SEN);

	//Transmit address and R/W and wait for completion
	I2C1TRN = (addr << 1) | (read == TRUE ? 1 : 0);
	while(I2C1STATbits.TRSTAT);

	//Determine of an ACK was received
	return I2C1STATbits.ACKSTAT ? FALSE : TRUE;
}

BOOL I2CRepStart(unsigned char addr, BOOL read)
{
	//Send repeated start condition and wait for completion
	I2C1CONbits.RSEN = 1;
	while(I2C1CONbits.RSEN);

	//Transmit address and R/W and wait for completion
	I2C1TRN = (addr << 1) | (read == TRUE ? 1 : 0);
	while(I2C1STATbits.TRSTAT);

	//Determine of an ACK was received
	return I2C1STATbits.ACKSTAT ? FALSE : TRUE;
}

BOOL I2CWrite(unsigned char data, BOOL stop)
{
	BOOL ack;
	//Transmit data and wait for completion
	I2C1TRN = data;
	while(I2C1STATbits.TRSTAT);

	//Determine of an ACK was received
	ack = I2C1STATbits.ACKSTAT ? FALSE : TRUE;

	//Transmit a stop event if required
	if(stop)
	{
		I2C1CONbits.PEN = 1;
		while(I2C1CONbits.PEN);
	}

	return ack;
}

unsigned char I2CRead(BOOL ack, BOOL stop)
{
	//Start receive and wait for completion
	I2C1CONbits.RCEN = 1;
	while(!I2C1STATbits.RBF);

	//Send an ACK or NACK as requested and wait for completion
	I2C1CONbits.ACKDT = ack ? 0 : 1;
	I2C1CONbits.ACKEN = 1;
	while(I2C1CONbits.ACKEN);

	//Transmit a stop event if required and wait for completion
	if(stop)
	{
		I2C1CONbits.PEN = 1;
		while(I2C1CONbits.PEN);
	}

	return I2C1RCV;
}



void I2CStop(void)
{
	I2C1CONbits.PEN = 1;
	while(I2C1CONbits.PEN);
}
