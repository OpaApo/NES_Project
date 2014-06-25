/*
*
*/

#include "AM.h"
#include "Serial.h"

module ActuatorP @safe() {
  uses {
    interface Boot;
    interface SplitControl as RadioControl;
		
		interface StdControl as SerialControl;		//Uart Byte-Stream Controller, HIL
		interface UartStream as Serial;						//Uart Byte-Stream, HIL
    
    interface Receive as RadioReceive[am_id_t id];
    interface Receive as RadioSnoop[am_id_t id];
    interface Packet as RadioPacket;
    interface AMPacket as RadioAMPacket;

    interface Leds;
  }
}

implementation
{
  enum {
    UART_QUEUE_LEN = 12,
    RADIO_QUEUE_LEN = 12,
  };

  message_t  radioQueueBufs[RADIO_QUEUE_LEN];
  message_t  * ONE_NOK radioQueue[RADIO_QUEUE_LEN];
  uint8_t    radioIn, radioOut;
  bool       radioBusy, radioFull;

	uint16_t angle;		//Contains last received sun angle from master

  task void uartSendTask();

  void dropBlink() {
    call Leds.led2Toggle();
  }

  void failBlink() {
    call Leds.led2Toggle();
  }

	event void Boot.booted() {
	  uint8_t i; 

  for (i = 0; i < RADIO_QUEUE_LEN; i++)
    radioQueue[i] = &radioQueueBufs[i];
  radioIn = radioOut = 0;
  radioBusy = FALSE;
  radioFull = TRUE;

  if (call RadioControl.start() == EALREADY)
    radioFull = FALSE;

	call SerialControl.start();		//Start serial connection to PC for debugging
	}

  event void RadioControl.startDone(error_t error) {
    if (error == SUCCESS) {
      radioFull = FALSE;
    }
  }

	async event void Serial.receivedByte(uint8_t byte) {}
	async event void Serial.receiveDone(uint8_t *buf, uint16_t len, error_t error) {}
	async event void Serial.sendDone(uint8_t *buf, uint16_t len, error_t error) {}

  event void RadioControl.stopDone(error_t error) {}

  message_t* ONE receive(message_t* ONE msg, void* payload, uint8_t len);
 

/*
*
*	Communication Master (ZigBee) -> Actuator (ZigBee)
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
		atomic {
			am_addr_t addr = call RadioAMPacket.source(msg);
			//1: maser, 6: servor controller
			if (addr == 1) {	//Source is Master node	
				angle = *((uint16_t*)payload);		//get message payload (16bit unsigned int)
				//TODO: Move Actuator code here!
				post uartSendTask();
				call Leds.led2Toggle();
			}
			return msg;
		}
  }

/*
*
*	Communication Actuator -> PC (UART) Debugging
*
*/
  
  task void uartSendTask() {
		atomic {
			uint8_t buf[2];
			buf[1] = (uint8_t)angle;		//Decompose 16bit result into two seperatee byte, MSB first
			buf[0] = (uint8_t)(angle >> 8); 
			if (call Serial.send(buf, 2) == SUCCESS){		//Send the two byte via uart to PC
				call Leds.led1Toggle();		//toggle green led
			}
		}
  }
}  
