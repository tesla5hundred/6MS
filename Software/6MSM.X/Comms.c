
#include <xc.h>
#include "6MSM.h"
#include "Comms.h"
#include "ECAN1Drv.h"
#include "common.h"
#include "types.h"
#include "Balance.h"
#include "Eeprom.h"

extern SysConfig cfg;
extern volatile BalData bal;
extern volatile PackData pack;
extern volatile Identity_t identity;
extern Cal_t config;

mID rx_ecan1message;
__eds__ ECAN1MSGBUF ecan1msgBuf __attribute__((eds,aligned(ECAN1_MSG_BUF_LENGTH*16)));

volatile BOOL sendVoltagesFlag = FALSE;
volatile BOOL sendErrorCodeFlag = FALSE;
volatile BOOL sendLoadEnFlag = FALSE;
volatile BOOL testFlag = FALSE;
volatile BOOL readCalFlag = FALSE;
volatile BOOL writeCalFlag = FALSE;
volatile BOOL getLocalVoltagesFlag = FALSE;
volatile BOOL saveCalToEEPROMFlag = FALSE;

volatile unsigned int commsFlags = 0;


volatile unsigned char balBusTxData;
volatile unsigned char balBusRxData[BUS_RX_BUF_SIZE];
volatile unsigned int balBusRxWPos = 0;
volatile unsigned int balBusRxRPos = 0;
volatile BOOL balBusTxFlag = FALSE;
volatile BOOL balBusRxFlag = FALSE;



void sendVoltages(void)
/*
{
	int i;
	for(i = 0; i <= (cfg.cellCount - 4); i = i + 4)
	{
		ecan1WriteTxMsgBufId(0,cfg.canDstID,1,0);
		ecan1WriteTxMsgBufData(0,8,	pack.voltages[i],
									pack.voltages[i+1],
									pack.voltages[i+2],
									pack.voltages[i+3]);
		C1TR01CONbits.TXREQ0=1;
		while(C1TR01CONbits.TXREQ0 == 1);

	}
	if(i < cfg.cellCount)		//Send the last voltages if they didn't fit exactly into a frame
	{
		ecan1WriteTxMsgBufId(0,cfg.canDstID,1,0);
		ecan1WriteTxMsgBufData(0,cfg.cellCount - i,
									pack.voltages[i],
									(i+1) < cfg.cellCount ? pack.voltages[i+1] : 0,
									(i+2) < cfg.cellCount ? pack.voltages[i+2] : 0,
									(i+3) < cfg.cellCount ? pack.voltages[i+3] : 0);
		C1TR01CONbits.TXREQ0=1;
		while(C1TR01CONbits.TXREQ0 == 1);

	}
}
*/
{
	ecan1WriteTxMsgBufId(0,cfg.canDstID,1,0);

	canTxInt(CAN_CMD_SEND_VOLTAGES);
	canTxInt(cfg.cellCount);

	int i;
	for(i = 0; i < (cfg.cellCount); i++)
	{
		canTxInt(getCellVoltage(i));
	}
	canTxCommit();

}

//Send the status of the loads
void sendLoadEn(void)
{
	unsigned char loadEnables[(MAX_CELLS+7) / 8];	//Array big enough to hold MAX_CELLS bits
	unsigned char len = (cfg.cellCount+7) / 8;
	getLoadEnables(loadEnables);

	ecan1WriteTxMsgBufId(0,cfg.canDstID,1,0);
	canTxInt(CAN_CMD_SEND_LOAD_EN);
	canTxInt(cfg.cellCount);

	int i;
	for(i = 0; i < len; i++)
	{
		canTxByte(loadEnables[i]);
	}
	canTxCommit();
}

//Send data received from the balancer bus to the host
void sendBalBusRxData(unsigned char data)
{
	ecan1WriteTxMsgBufId(0,cfg.canDstID,1,0);

	canTxInt(CAN_CMD_BALBUS_RX);
	canTxByte(data);
	canTxCommit();
}

void sendBalData(void)
{
	unsigned char * data = (unsigned char *)&bal;
	ecan1WriteTxMsgBufId(0,cfg.canDstID,1,0);
	canTxInt(CAN_CMD_SEND_BAL_DATA);

	int i;
	for(i = 0; i < sizeof(BalData); i++)
	{
		canTxByte(data[i]);
	}
	canTxCommit();
}

void sendByte(unsigned char data)
{
	ecan1WriteTxMsgBufId(0,cfg.canDstID,1,0);
	ecan1WriteTxMsgBufData(0,1,	data,
								0,
								0,
								0);
	C1TR01CONbits.TXREQ0=1;
	while(C1TR01CONbits.TXREQ0 == 1);
}

void processComms(void)
{
	int i;

	if(commsFlags & COMM_FLAG_SEND_VOLTAGES)
	{
		sendVoltages();
		clearFlag(COMM_FLAG_SEND_VOLTAGES);
		bal.svLCount++;
	}

	if(commsFlags & COMM_FLAG_SEND_LOAD_EN)
	{
		sendLoadEn();
		clearFlag(COMM_FLAG_SEND_LOAD_EN);
		bal.slLCount++;
	}

	if(commsFlags & COMM_FLAG_SEND_BAL_DATA)
	{
		sendBalData();
		clearFlag(COMM_FLAG_SEND_BAL_DATA);
		bal.sdLCount++;
	}

	if(balBusTxFlag)
	{
		balBusTx(balBusTxData);
		balBusTxFlag = FALSE;
	}

	if(balBusRxWPos != balBusRxRPos)
	{
		sendBalBusRxData(balBusRxData[balBusRxRPos++]);
		if(balBusRxRPos == BUS_RX_BUF_SIZE)
			balBusRxRPos = 0;
		balBusRxFlag = FALSE;
	}
	//Read config
	if(readCalFlag)
	{
		ecan1WriteTxMsgBufId(0,cfg.canDstID,1,0);
		for(i = 0; i < sizeof(config); i++)
		{
			canTxByte(*((volatile unsigned char *)(((volatile unsigned char *)(&config)) + i)));
		}
		canTxCommit();
		readCalFlag = FALSE;
	}

	//Write config
	if(writeCalFlag)
	{
		ecan1WriteTxMsgBufId(0,cfg.canDstID,1,0);
		canTxByte(CAN_CMD_WRITE_CAL);
		canTxCommit();
		writeCalFlag = FALSE;
	}

	//Get voltages
	if(getLocalVoltagesFlag)
	{
		ecan1WriteTxMsgBufId(0,cfg.canDstID,1,0);
		for(i = 0; i < 6; i++)
		{
			canTxInt(pack.voltages[i]);
		}
		canTxCommit();
		getLocalVoltagesFlag = FALSE;
	}

	//Save to EEPROM
	if(saveCalToEEPROMFlag)
	{
		for(i = 0; i < sizeof(config); i++)
		{
			EepromWrite(*((volatile unsigned char *)(((volatile unsigned char *)(&config)) + i)), i + EEPROM_OFFSET_CAL);
		}
		ecan1WriteTxMsgBufId(0,cfg.canDstID,1,0);
		canTxByte(CAN_CMD_SAVE_CAL);
		canTxCommit();
		saveCalToEEPROMFlag = FALSE;
	}

	if(commsFlags & COMM_FLAG_GET_CONFIG)
	{
		ecan1WriteTxMsgBufId(0,cfg.canDstID,1,0);
		for(i = 0; i < sizeof(cfg); i++)
		{
			canTxByte(*((volatile unsigned char *)(((volatile unsigned char *)(&cfg)) + i)));
		}
		canTxCommit();
		clearFlag(COMM_FLAG_GET_CONFIG);
	}

	if(commsFlags & COMM_FLAG_SET_CONFIG)
	{
		ecan1WriteTxMsgBufId(0,cfg.canDstID,1,0);
		canTxByte(CAN_CMD_SET_CONFIG);
		canTxCommit();
		clearFlag(COMM_FLAG_SET_CONFIG);
	}

	if(commsFlags & COMM_FLAG_SAVE_CONFIG)
	{
		for(i = 0; i < sizeof(cfg); i++)
		{
			EepromWrite(*((volatile unsigned char *)(((volatile unsigned char *)(&cfg)) + i)), i + EEPROM_OFFSET_CONFIG);
		}

		ecan1WriteTxMsgBufId(0,cfg.canDstID,1,0);
		canTxByte(CAN_CMD_SAVE_CONFIG);
		canTxCommit();
		clearFlag(COMM_FLAG_SAVE_CONFIG);
	}

	if(commsFlags & COMM_FLAG_GET_IDENTITY)
	{
		ecan1WriteTxMsgBufId(0,cfg.canDstID,1,0);
		for(i = 0; i < sizeof(identity); i++)
		{
			canTxByte(*((volatile unsigned char *)(((volatile unsigned char *)(&identity)) + i)));
		}
		canTxCommit();
		clearFlag(COMM_FLAG_GET_IDENTITY);
	}

	if(commsFlags & COMM_FLAG_SET_IDENTITY)
	{
		ecan1WriteTxMsgBufId(0,cfg.canDstID,1,0);
		canTxByte(CAN_CMD_SET_IDENTITY);
		canTxCommit();
		clearFlag(COMM_FLAG_SET_IDENTITY);
	}

	if(commsFlags & COMM_FLAG_SAVE_IDENTITY)
	{
		for(i = 0; i < sizeof(identity); i++)
		{
			EepromWrite(*((volatile unsigned char *)(((volatile unsigned char *)(&identity)) + i)), i + EEPROM_OFFSET_IDENTITY);
		}

		ecan1WriteTxMsgBufId(0,cfg.canDstID,1,0);
		canTxByte(CAN_CMD_SAVE_IDENTITY);
		canTxCommit();
		clearFlag(COMM_FLAG_SAVE_IDENTITY);
	}

	if(testFlag)
	{
		unsigned char eeBuf[32];

		int i;
		for(i = 0; i < 256; i++)
		{
			sendByte(EepromRead(i));
		}


		testFlag = FALSE;
	}

}

//Clear flag while interrupts are disabled to prevent incorrect clearing of
//flags in an interrupt occurs in the middle of the read-modify-write operation
void clearFlag(unsigned int clear)
{
	INTCON2bits.GIE = 0;	//Disable global interrupts
	commsFlags = commsFlags & ~clear;
	INTCON2bits.GIE = 1;	//Enable global interrupts
}


enum {
	CAN_RX_STATE_IDLE = 0,
	CAN_RX_STATE_WRITE_CAL,
	CAN_RX_STATE_SET_CONFIG,
	CAN_RX_STATE_SET_IDENTITY
};

unsigned int canRxState = CAN_RX_STATE_IDLE;
void canRxProcess(mID * rx_ecan1message)
{
	int i;
	static unsigned int dataCount = 0, maxLen = 0;

	switch(canRxState)
	{
		case CAN_RX_STATE_IDLE:
		switch(rx_ecan1message->data[0])
		{
			case CAN_CMD_SEND_VOLTAGES:
				commsFlags |= COMM_FLAG_SEND_VOLTAGES;
				bal.svICount++;
			break;

			case CAN_CMD_SEND_LOAD_EN:
				commsFlags |= COMM_FLAG_SEND_LOAD_EN;
				bal.slICount++;
			break;

			case CAN_CMD_SEND_BAL_DATA:
				commsFlags |= COMM_FLAG_SEND_BAL_DATA;
				bal.sdICount++;
			break;

			case CAN_CMD_SET_BALBUS_PASSTHROUGH:
				cfg.balBusPassthrough = TRUE;
			break;

			case CAN_CMD_CLEAR_BALBUS_PASSTHROUGH:
				cfg.balBusPassthrough = FALSE;
			break;

			case CAN_CMD_BALBUS_TX:
				balBusTxData = rx_ecan1message->data[2];
				balBusTxFlag = TRUE;
			break;

			case CAN_CMD_TEST:
				testFlag = TRUE;
				balBusTxFlag = TRUE;
			break;

			case CAN_CMD_READ_CAL:
				readCalFlag = TRUE;
			break;

			case CAN_CMD_WRITE_CAL:
				maxLen = sizeof(config);
				dataCount = 0;
				canRxState = CAN_RX_STATE_WRITE_CAL;
			break;

			case CAN_CMD_GET_LOCAL_VOLTAGES:
				getLocalVoltagesFlag = TRUE;
			break;

			case CAN_CMD_SAVE_CAL:
				saveCalToEEPROMFlag = TRUE;
			break;

			case CAN_CMD_GET_CONFIG:
				commsFlags |= COMM_FLAG_GET_CONFIG;
			break;

			case CAN_CMD_SET_CONFIG:
				maxLen = sizeof(cfg);
				dataCount = 0;
				canRxState = CAN_RX_STATE_SET_CONFIG;
			break;
			
			case CAN_CMD_SAVE_CONFIG:
				commsFlags |= COMM_FLAG_SAVE_CONFIG;
			break;

			case CAN_CMD_GET_IDENTITY:
				commsFlags |= COMM_FLAG_GET_IDENTITY;
			break;

			case CAN_CMD_SET_IDENTITY:
				maxLen = sizeof(identity);
				dataCount = 0;
				canRxState = CAN_RX_STATE_SET_IDENTITY;
			break;

			case CAN_CMD_SAVE_IDENTITY:
				commsFlags |= COMM_FLAG_SAVE_IDENTITY;
			break;
		}
		break;
		
		case CAN_RX_STATE_WRITE_CAL:
			for(i = 0; (i < rx_ecan1message->data_length) && (dataCount <= maxLen); i++)
			{
				*((volatile unsigned char *)(((volatile unsigned char *)(&config)) + dataCount)) = rx_ecan1message->data[i];
				dataCount++;
			}

			if(dataCount >= maxLen)
			{
				dataCount = 0;
				writeCalFlag = TRUE;

				canRxState = CAN_RX_STATE_IDLE;
			}
			break;

		case CAN_RX_STATE_SET_CONFIG:
			for(i = 0; (i < rx_ecan1message->data_length) && (dataCount <= maxLen); i++)
			{
				*((volatile unsigned char *)(((volatile unsigned char *)(&cfg)) + dataCount)) = rx_ecan1message->data[i];
				dataCount++;
			}

			if(dataCount >= maxLen)
			{
				dataCount = 0;

				commsFlags |= COMM_FLAG_SET_CONFIG;
				canRxState = CAN_RX_STATE_IDLE;
			}
			break;

		case CAN_RX_STATE_SET_IDENTITY:
			for(i = 0; (i < rx_ecan1message->data_length) && (dataCount <= maxLen); i++)
			{
				*((volatile unsigned char *)(((volatile unsigned char *)(&identity)) + dataCount)) = rx_ecan1message->data[i];
				dataCount++;
			}

			if(dataCount >= maxLen)
			{
				dataCount = 0;

				commsFlags |= COMM_FLAG_SET_IDENTITY;
				canRxState = CAN_RX_STATE_IDLE;
			}
			break;
	}

}

unsigned char canTxDataCount = 0;
unsigned short canTxData[4] = {0,0,0,0};
void canTxByte(unsigned char d)
{
	//Add the data to the send buffer
	((unsigned char *)canTxData)[canTxDataCount++] = d;

	//If this is the 8th byte, send the send buffer
	if(canTxDataCount >= 8)
	{
		canTxDataCount = 0;
		ecan1WriteTxMsgBufData(0,8,	canTxData[0],
									canTxData[1],
									canTxData[2],
									canTxData[3]);
		C1TR01CONbits.TXREQ0=1;
		while(C1TR01CONbits.TXREQ0 == 1);

		canTxData[0] = canTxData[1] = canTxData[2] = canTxData[3] = 0;
	}
}

void canTxInt(unsigned int d)
{
	canTxByte(d & 0xFF);
	canTxByte(d >> 8);
}

void canTxCommit(void)
{
	//If there's any data to send, send it
	if(canTxDataCount > 0)
	{
		ecan1WriteTxMsgBufData(0,canTxDataCount,
									canTxData[0],
									canTxData[1],
									canTxData[2],
									canTxData[3]);
		C1TR01CONbits.TXREQ0=1;
		while(C1TR01CONbits.TXREQ0 == 1);

		canTxDataCount = 0;
		canTxData[0] = canTxData[1] = canTxData[2] = canTxData[3] = 0;
	}

}

/******************************************************************************
*
*    Function:			rxECAN1
*    Description:       moves the message from the DMA memory to RAM
*
*    Arguments:			*message: a pointer to the message structure in RAM
*						that will store the message.
*	 Author:            Jatinder Gharoo
*
*
******************************************************************************/
void rxECAN1(mID *message)
{
	unsigned int ide=0;
	unsigned int srr=0;
	unsigned long id=0,d;

	/*
	Standard Message Format:
	Word0 : 0bUUUx xxxx xxxx xxxx
			     |____________|||
 					SID10:0   SRR IDE(bit 0)
	Word1 : 0bUUUU xxxx xxxx xxxx
			   	   |____________|
						EID17:6
	Word2 : 0bxxxx xxx0 UUU0 xxxx
			  |_____||	     |__|
			  EID5:0 RTR   	  DLC
	word3-word6: data bytes
	word7: filter hit code bits

	Substitute Remote Request Bit
	SRR->	"0"	 Normal Message
			"1"  Message will request remote transmission

	Extended  Identifier Bit
	IDE-> 	"0"  Message will transmit standard identifier
	   		"1"  Message will transmit extended identifier

	Remote Transmission Request Bit
	RTR-> 	"0"  Message transmitted is a normal message
			"1"  Message transmitted is a remote message
	*/
	/* read word 0 to see the message type */
	ide=ecan1msgBuf[message->buffer][0] & 0x0001;
	srr=ecan1msgBuf[message->buffer][0] & 0x0002;

	/* check to see what type of message it is */
	/* message is standard identifier */
	if(ide==0)
	{
		message->id=(ecan1msgBuf[message->buffer][0] & 0x1FFC) >> 2;
		message->frame_type=CAN_FRAME_STD;
	}
	/* mesage is extended identifier */
	else
	{
		id=ecan1msgBuf[message->buffer][0] & 0x1FFC;
		message->id=id << 16;
		id=ecan1msgBuf[message->buffer][1] & 0x0FFF;
		message->id=message->id+(id << 6);
		id=(ecan1msgBuf[message->buffer][2] & 0xFC00) >> 10;
		message->id=message->id+id;
		message->frame_type=CAN_FRAME_EXT;
	}
	/* check to see what type of message it is */
	/* RTR message */
	if(srr==1)
	{
		message->message_type=CAN_MSG_RTR;
	}
	/* normal message */
	else
	{
		message->message_type=CAN_MSG_DATA;
		message->data[0]=(unsigned char)ecan1msgBuf[message->buffer][3];
		message->data[1]=(unsigned char)((ecan1msgBuf[message->buffer][3] & 0xFF00) >> 8);
		message->data[2]=(unsigned char)ecan1msgBuf[message->buffer][4];
		message->data[3]=(unsigned char)((ecan1msgBuf[message->buffer][4] & 0xFF00) >> 8);
		message->data[4]=(unsigned char)ecan1msgBuf[message->buffer][5];
		message->data[5]=(unsigned char)((ecan1msgBuf[message->buffer][5] & 0xFF00) >> 8);
		message->data[6]=(unsigned char)ecan1msgBuf[message->buffer][6];
		message->data[7]=(unsigned char)((ecan1msgBuf[message->buffer][6] & 0xFF00) >> 8);
		message->data_length=(unsigned char)(ecan1msgBuf[message->buffer][2] & 0x000F);
	}
}

void __attribute__((interrupt, auto_psv))_C1Interrupt(void)
{
	IFS2bits.C1IF = 0;        // clear interrupt flag
	if(C1INTFbits.TBIF)
    {
    	C1INTFbits.TBIF = 0;
    }

    if(C1INTFbits.RBIF)
    {
		// read the message
	    if(C1RXFUL1bits.RXFUL1==1)
	    {
	    	rx_ecan1message.buffer=1;
	    	C1RXFUL1bits.RXFUL1=0;
	    }
	    rxECAN1(&rx_ecan1message);
		canRxProcess(&rx_ecan1message);
		C1INTFbits.RBIF = 0;
	}
}



//------------------------------------------------------------------------------
//    DMA interrupt handlers
//------------------------------------------------------------------------------

void __attribute__((interrupt, no_auto_psv)) _DMA0Interrupt(void)
{
   IFS0bits.DMA0IF = 0;          // Clear the DMA0 Interrupt Flag;
}

void __attribute__((interrupt, no_auto_psv)) _DMA2Interrupt(void)
{
   IFS1bits.DMA2IF = 0;          // Clear the DMA2 Interrupt Flag;
}

