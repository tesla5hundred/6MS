/* 
 * File:   Comms.h
 * Author: User
 *
 * Created on September 29, 2013, 11:18 PM
 */

#ifndef COMMS_H
#define	COMMS_H

#ifdef	__cplusplus
extern "C" {
#endif


#include "types.h"
#include "common.h"
#include "ECAN1Config.h"


#define BUS_RX_BUF_SIZE	64

enum {
	CAN_CMD_SEND_VOLTAGES = 0,
	CAN_CMD_SEND_LOAD_EN,
	CAN_CMD_SEND_BAL_DATA,
	CAN_CMD_BALBUS_TX,
	CAN_CMD_BALBUS_RX,
	CAN_CMD_SET_BALBUS_PASSTHROUGH,
	CAN_CMD_CLEAR_BALBUS_PASSTHROUGH,
	CAN_CMD_TEST,
	CAN_CMD_READ_CAL,
	CAN_CMD_WRITE_CAL,
	CAN_CMD_GET_LOCAL_VOLTAGES,
	CAN_CMD_SAVE_CAL,
	CAN_CMD_GET_CONFIG,
	CAN_CMD_SET_CONFIG,
	CAN_CMD_SAVE_CONFIG,
	CAN_CMD_GET_IDENTITY,
	CAN_CMD_SET_IDENTITY,
	CAN_CMD_SAVE_IDENTITY,
	CAN_CMD_GET_BAL_DATA

};

#define COMM_FLAG_GET_CONFIG	0x1
#define COMM_FLAG_SET_CONFIG	0x2
#define COMM_FLAG_SAVE_CONFIG	0x4
#define COMM_FLAG_GET_IDENTITY	0x8
#define COMM_FLAG_SET_IDENTITY	0x10
#define COMM_FLAG_SAVE_IDENTITY	0x20
#define COMM_FLAG_SEND_VOLTAGES	0x40
#define COMM_FLAG_SEND_LOAD_EN	0x80
#define COMM_FLAG_SEND_BAL_DATA	0x100

void sendVoltages(void);
void sendLoadEn(void);
void sendErrorCode(void);
void sendByte(unsigned char data);
void processComms(void);
void canRxProcess(mID * rx_ecan1message);
void canTxByte(unsigned char d);
void canTxInt(unsigned int d);
void canTxCommit(void);
void rxECAN1(mID *message);
void clearFlag(unsigned int clear);


#ifdef	__cplusplus
}
#endif

#endif	/* COMMS_H */

