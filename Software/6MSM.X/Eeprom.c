#include <xc.h>
#include "Eeprom.h"
#include "I2C.h"


unsigned char EepromRead(unsigned short address)
{
	//Send start with upper 3 bits of address
	I2CStart(0b1010000 | ((address >> 8) & 0b111), FALSE);

	//Send lower bits of address
	I2CWrite(address & 0xFF, FALSE);

	//Begin read
	I2CRepStart(0b1010000 | ((address >> 8) & 0b111), TRUE);

	return I2CRead(FALSE, TRUE);
}

void EepromReadBlock(unsigned char * data, unsigned short address, unsigned int length)
{
	unsigned int i;

	//Send start with upper 3 bits of address
	I2CStart(0b1010000 | ((address >> 8) & 0b111), FALSE);

	//Send lower bits of address
	I2CWrite(address & 0xFF, FALSE);

	//Begin read
	I2CRepStart(0b1010000 | ((address >> 8) & 0b111), TRUE);

	//Read the bulk of the data, sending ACKs
	for(i = 0; i < (length - 1); i++)
	{
		data[i] = I2CRead(TRUE, FALSE);
	}
	//Read the last data with NACK and stop bit
	data[i] = I2CRead(FALSE, TRUE);
}

void EepromWrite(unsigned char data, unsigned short address)
{
	BOOL ack;
	//Send start with upper 3 bits of address
	I2CStart(0b1010000 | ((address >> 8) & 0b111), FALSE);

	//Send lower bits of address
	I2CWrite(address & 0xFF, FALSE);

	//Send data and stop
	I2CWrite(data, TRUE);

	do
	{
		//Send start condition for write to determine if write is complete. Device will not acknowledge until write is complete
		ack = I2CStart(0b1010000 | ((address >> 8) & 0b111), FALSE);
		I2CStop();
	} while(FALSE == ack);

}