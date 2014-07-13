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

module MasterP @safe() {
  uses {
    interface Boot;
    interface SplitControl as RadioControl;
		
		interface StdControl as SerialControl;		//Uart Byte-Stream Controller, HIL
		interface UartStream as Serial;						//Uart Byte-Stream, HIL
    
    //interface AMSend as RadioSend[am_id_t id];
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
    SENSOR_NODE_COUNT = 4,					//Sensor nodes
		CALCULATION_PERIOD = 1000,			//Timer period for calculation / UART to Servo
  };

	uint16_t nodePayload[SENSOR_NODE_COUNT];	//Payload of last package from nodes with address 2 TO SENSOR_NODE_COUNT+2
  uint16_t result = 0;

  // Use LEDs to report various status issues.
  void report_problem() { call Leds.led0Toggle(); }		//Red
  void report_received() { call Leds.led1Toggle(); }			//Green
  void report_timer_fired() { call Leds.led2Toggle(); }	//Blue

  task void uartSendTask();
  //task void radioSendTask();

	event void Boot.booted() {
		uint8_t i; 

		for (i = 0; i < SENSOR_NODE_COUNT; i++)
			nodePayload[i] = 0;

		call RadioControl.start();

		call SerialControl.start();		//Start serial connection to PC

		call Timer0.startPeriodic(CALCULATION_PERIOD);		//start the timer
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
			//post radioSendTask();
			report_timer_fired();
		}
	}


  event void RadioControl.startDone(error_t error) {
		if (error != SUCCESS) {
      report_problem();
			call RadioControl.start();
		}			

	}

	async event void Serial.receivedByte(uint8_t byte) {}
	async event void Serial.receiveDone(uint8_t *buf, uint16_t len, error_t error) {}
	async event void Serial.sendDone(uint8_t *buf, uint16_t len, error_t error) {}

  event void RadioControl.stopDone(error_t error) {}

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
		uint16_t buffer = 0;		
		atomic
		{
			am_addr_t addr = call RadioAMPacket.source(msg);
			//1: maser, 6: servor controller
			if (addr-2 >= 0 && addr < SENSOR_NODE_COUNT+2) {	//Prevent pointing to wrong memory location due to corrupted packet source address
				buffer = *((uint16_t*)payload);	//put message payload (16bit unsigned int) into array slot corresponding to node address-2.
				if (buffer != 0xffff)			//0xffff means that the sensor node had problems reading the sensor value
					nodePayload[addr-2] = buffer;
			}
		}
		report_received();
		return msg;
  }

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
			if(call Serial.send(buf, 2) != SUCCESS)		//Send the two byte via uart to PC
				report_problem();
		}
  }


/*
*
*	Communication Master (ZigBee) -> Servo (ZigBee)		OBSOLETE: Servo control now over PC
*
*/

/* task void radioSendTask() {
		uint8_t len;
		uint16_t* pl;
    message_t pkt;
    
    atomic {
			len = 2;
			pl = (uint16_t*)call RadioPacket.getPayload(&pkt, len);	//Get pointer to payload of the message
			*pl = result;		//Write result of calculation to payload
			
			call RadioSend.send[0](SENSOR_NODE_COUNT+2, &pkt, len);	//Send result to Servo node, has highest address (=SENSOR_NODE_COUNT+2)
		}
	}

  event void RadioSend.sendDone[am_id_t id](message_t* msg, error_t error) {} */
}  
