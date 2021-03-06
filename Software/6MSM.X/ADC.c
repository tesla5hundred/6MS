#include <xc.h>
#include "I2C.h"
#include "ADC.h"

//Initialize ADC for continuous conversion
int initADC(void)
{
	I2CStart(0b1101000, FALSE);
	I2CWrite(0b00011011, TRUE);		//Continuous conversion, 16 bit 15Sa/s, Gain of 8

}

//R d ADC conversion result
int readADC(void)
{
	int result;

	I2CStart(0b1101000, TRUE);
	result = (int)I2CRead(TRUE, FALSE) << 8;
	result |= (int)I2CRead(TRUE, FALSE) & 0xFF;
	I2CRead(FALSE, TRUE);

	return result;
}