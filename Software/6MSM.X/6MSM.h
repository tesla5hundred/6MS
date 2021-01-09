/* 
 * File:   6MSM.h
 * Author: User
 *
 * Created on September 22, 2013, 11:47 PM
 */

#ifndef _6MSM_H
#define	_6MSM_H

#ifdef	__cplusplus
extern "C" {
#endif

#define TICK_FREQUENCY 1000		//Hz

#define	ANALOG_PWM_PERIOD	2048	//Period in Fp cycles (results in ~20kHz with 40MHz Fp)

#define LED_RED		PORTAbits.RA8
#define LED_GREEN	PORTBbits.RB4

#define CHARGE_ENABLE	PORTAbits.RA9
#define ANALOG1PWM		PORTBbits.RB8
#define ANALOG2PWM		PORTBbits.RB7
/*
vbat4 AN0 RA0
vbat3 AN1 RA1
vbat5 AN6 RC0
vbat1 AN7 RC1
vbat2 AN8 RC2
vbat6 AN2 RB0
*/

#define CLIP(x, min, max)	((x) < (min) ? (min) : (x) > (max) ? (max) : (x))
#define	MAX(x, y)				((x) > (y) ? (x) : (y))
#define	MIN(x, y)				((x) < (y) ? (x) : (y))

#ifdef	__cplusplus
}
#endif

#endif	/* 6MSM_H */

