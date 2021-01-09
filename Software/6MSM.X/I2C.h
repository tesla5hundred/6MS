/*
 * File:   I2C.h
 * Author: User
 *
 * Created on November 17, 2013, 2:19 PM
 */

#ifndef I2C_H
#define	I2C_H

#ifdef	__cplusplus
extern "C" {
#endif

#include "types.h"


BOOL I2CStart(unsigned char addr, BOOL read);
BOOL I2CRepStart(unsigned char addr, BOOL read);
BOOL I2CWrite(unsigned char data, BOOL stop);
unsigned char I2CRead(BOOL ack, BOOL stop);
void I2CStop(void);


#ifdef	__cplusplus
}
#endif

#endif	/* I2C_H */



