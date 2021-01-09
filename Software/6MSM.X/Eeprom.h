/* 
 * File:   Eeprom.h
 * Author: User
 *
 * Created on November 17, 2013, 2:19 PM
 */

#ifndef EEPROM_H
#define	EEPROM_H

#ifdef	__cplusplus
extern "C" {
#endif

#include "types.h"


unsigned char EepromRead(unsigned short address);
void EepromReadBlock(unsigned char * data, unsigned short address, unsigned int length);
void EepromWrite(unsigned char data, unsigned short address);


#ifdef	__cplusplus
}
#endif

#endif	/* EEPROM_H */

