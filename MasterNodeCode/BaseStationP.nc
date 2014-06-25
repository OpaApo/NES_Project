/*
*	Node Adresses:
* Addr. 1: Master Node
* Addr. 2: Sensor Node 1
* Addr. 3: Sensor Node 2
*				.
*				.
*				.
* Addr. SENSOR_NODE_COUNT+1: Sensor Node SENSOR_NODE_COUNT
* Addr. SENSOR_NODE_COUNT+2: Servo Node
*
*/

#include "AM.h"
#include "Serial.h"
#include "Timer.h"

module BaseStationP @safe() {
  uses {
    interface Boot;
    interface SplitControl as RadioControl;
		
		interface StdControl as SerialControl;		//Uart Byte-Stream Controller, HIL
		interface UartStream as Serial;						//Uart Byte-Stream, HIL
    
    interface AMSend as RadioSend[am_id_t id];
    interface Receive as RadioReceive[am_id_t id];
    interface Receive as RadioSnoop[am_id_t id];
    interface Packet as RadioPacket;
    interface AMPacket as RadioAMPacket;

	interface Timer<TMilli> as Timer0;

    interface Leds;
  }
}

implementation
{
  enum {
    UART_QUEUE_LEN = 12,
    RADIO_QUEUE_LEN = 12,
		SENSOR_NODE_COUNT = 4,
  };

  uint16_t  uartQueueBufs[UART_QUEUE_LEN];
  uint16_t  * ONE_NOK uartQueue[UART_QUEUE_LEN];
  uint8_t    uartIn, uartOut;
  bool       uartBusy, uartFull;

  message_t  radioQueueBufs[RADIO_QUEUE_LEN];
  message_t  * ONE_NOK radioQueue[RADIO_QUEUE_LEN];
  uint8_t    radioIn, radioOut;
  bool       radioBusy, radioFull;

	uint16_t nodePayload[SENSOR_NODE_COUNT];	//Payload of last package from nodes with address 2 TO SENSOR_NODE_COUNT+2
  uint16_t result = 0;

  task void uartSendTask();
  task void radioSendTask();

  void dropBlink() {
    call Leds.led2Toggle();
  }

  void failBlink() {
    call Leds.led2Toggle();
  }

	event void Boot.booted() {
	  uint8_t i; 

  for (i = 0; i < UART_QUEUE_LEN; i++)
    uartQueue[i] = &uartQueueBufs[i];
  uartIn = uartOut = 0;
  uartBusy = FALSE;
  uartFull = TRUE;

  for (i = 0; i < RADIO_QUEUE_LEN; i++)
    radioQueue[i] = &radioQueueBufs[i];
  radioIn = radioOut = 0;
  radioBusy = FALSE;
  radioFull = TRUE;

	for (i = 0; i < SENSOR_NODE_COUNT; i++)
		nodePayload[i] = 0;

  if (call RadioControl.start() == EALREADY)
    radioFull = FALSE;
  //if (call SerialControl.start() == EALREADY)
  //  uartFull = FALSE;
	call SerialControl.start();		//Start serial connection to PC

	call Timer0.startPeriodic(1000);		//start the timer with
}

	event void Timer0.fired() {		//Timer interrupt
		atomic {
			uint8_t i;
			//TODO: Calculation here!
			//result = ...

			//TODO: Temporary:
			result = 0;
			for (i = 0; i < SENSOR_NODE_COUNT; i++)
				result = result + nodePayload[i];

	    post uartSendTask();			//Send result to PC (Uart)
			post radioSendTask();			//Send result to Servo node (ZigBee)
			call Leds.led0Toggle();		//Toggle red led
		}
	}


  event void RadioControl.startDone(error_t error) {
    if (error == SUCCESS) {
      radioFull = FALSE;
    }
  }

  
	async event void Serial.receivedByte(uint8_t byte) {}
	async event void Serial.receiveDone(uint8_t *buf, uint16_t len, error_t error) {}
	async event void Serial.sendDone(uint8_t *buf, uint16_t len, error_t error) {uartBusy = FALSE;}

  event void RadioControl.stopDone(error_t error) {}

  uint8_t count = 0;

  message_t* ONE receive(message_t* ONE msg, void* payload, uint8_t len);
 

/*
*
*	Communication Sensors (ZigBee) -> Master (ZigBee)
*
*/
 
  event message_t *RadioSnoop.receive[am_id_t id](message_t *msg,
						    void *payload,
						    uint8_t len) {
    return receive(msg, payload, len);
  }
  
  event message_t *RadioReceive.receive[am_id_t id](message_t *msg,
						    void *payload,
						    uint8_t len) {
    return receive(msg, payload, len);
  }

  message_t* receive(message_t *msg, void *payload, uint8_t len) {
	
		am_addr_t addr = call RadioAMPacket.source(msg);

		//1: maser, 6: servor controller
		if (addr-2 >= 0 && addr < SENSOR_NODE_COUNT+2)		
			nodePayload[addr-2] = *((uint16_t*)payload);	//put message payload (16bit unsigned int) into array slot corresponding to node address-2.

		return msg;
  }

  uint8_t tmpLen;

/*
*
*	Communication Sensors (ZigBee) -> PC (UART)
*
*/
  
  task void uartSendTask() {
		atomic {
			uint8_t buf[2];
			buf[1] = (uint8_t)result;		//Decompose 16bit result into two seperatee byte, MSB first
			buf[0] = (uint8_t)(result >> 8); 
			if (call Serial.send(buf, 2) == SUCCESS){		//Send the two byte via uart to PC
				call Leds.led1Toggle();		//toggle green led
			}
		}
  }


/*
*
*	Communication Master (ZigBee) -> Servo (ZigBee)
*
*/

 task void radioSendTask() {
		uint8_t len;
		uint16_t* pl;
    message_t* pkt;
    
    atomic
      if (radioIn == radioOut && !radioFull)
			{
				radioBusy = FALSE;
				return;
			}

			pl = (uint16_t*)call RadioPacket.getPayload(pkt, NULL);	//Get pointer to payload of the message
			*pl = result;		//Write result of calculation to payload
			len = 2;
			
			if (call RadioSend.send[0](SENSOR_NODE_COUNT+2, pkt, len) == SUCCESS)	//Send result to Servo node, has highest address (=SENSOR_NODE_COUNT+2)
			  call Leds.led0Toggle();
			else
		  {
				failBlink();
				post radioSendTask();
		  }
		}

  event void RadioSend.sendDone[am_id_t id](message_t* msg, error_t error) {
    if (error != SUCCESS)
      failBlink();
    else 
		  atomic
			if (msg == radioQueue[radioOut])
			{
				if (++radioOut >= RADIO_QUEUE_LEN)
				  radioOut = 0;
				if (radioFull)
				  radioFull = FALSE;
			}
			post radioSendTask();
		}
}  
