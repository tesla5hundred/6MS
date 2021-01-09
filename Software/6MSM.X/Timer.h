/* 
 * File:   Timer.h
 * Author: User
 *
 * Created on September 30, 2013, 6:03 PM
 */

#ifndef TIMER_H
#define	TIMER_H

#include "Timer.h"
#include "types.h"

#define TIMER_MODE_SINGLE	0
#define TIMER_MODE_CONT		1

typedef struct Timer Timer;
struct Timer {
	unsigned int count;
	unsigned int period;
	BOOL enabled;
	BOOL elapsed;
	BOOL repetitive;
	int userData;
	void (*callback)(int userData);
	Timer * pNext;
};

void initTimer(Timer * t, unsigned int period, BOOL repetitive, BOOL enabled, void (*callback)(int), int userData);
void registerTimer(Timer * t);
void deRegisterTimer(Timer * t);

void processTimers(void);

#endif	/* TIMER_H */

