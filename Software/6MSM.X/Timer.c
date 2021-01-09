#include <xc.h>
#include "types.h"
#include "Timer.h"

Timer * pFirst = NULL;

void initTimer(Timer * t, unsigned int period, BOOL repetitive, BOOL enabled, void (*callback)(int), int userData)
{
	t->callback = callback;
	t->count = 0;
	t->period = period;
	t->repetitive = repetitive;
	t->enabled = enabled;
	t->elapsed = FALSE;
	t->userData = userData;
	t->pNext = (Timer *)NULL;
}

void registerTimer(Timer * t)
{
	Timer * tmr;
	if(NULL == pFirst)	//If this is the first timer
	{
		pFirst = t;
	}
	else	//Not the first timer, traverse the linked list and add it to the end
	{
		tmr = pFirst;

		while(NULL != tmr->pNext)
		{
			tmr = tmr->pNext;
		}
		//tmr now points to the last timer

		tmr->pNext = t;	//Add t to the end of the linked list
		t->pNext = NULL;
	}
}
void deRegisterTimer(Timer * t)
{
	//Search the list of timers and delink it

	Timer * tmr = pFirst;

	if(NULL == tmr)
		return;	//Error, there are no timers
	else if(pFirst == t)
	{
		pFirst = t->pNext;
	}
	else
	{
		//Go through the list until we find the timer just before the one to remove
		while((tmr->pNext != t) && (NULL != tmr->pNext))
			tmr = tmr->pNext;

		//tmr is now either the timer before the one to be removed, or the last one
		if(NULL == tmr->pNext)
			return;		//Error, timer was not found

		//If we're removingn the last timer
		if(NULL == t->pNext)
		{
			tmr->pNext = NULL;
		}
		else	//Not removing the last timer
		{
			tmr->pNext = t->pNext;
		}

	}
}

void processTimers(void)
{
	Timer * tmr;
	if(NULL == pFirst)
		return;
	else
	{
		tmr = pFirst;

		while(NULL != tmr)
		{
			if(tmr->enabled)
			{
				if(tmr->count < tmr->period)
				{
					tmr->count++;
				}
				else
				{
					tmr->count = 0;
					tmr->elapsed = TRUE;
					if(NULL != tmr->callback)
					{
						tmr->callback(tmr->userData);
					}

					if(FALSE == tmr->repetitive)
						tmr->enabled = FALSE;
				}
			}
			tmr = tmr->pNext;
		}
	}

}