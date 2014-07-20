/*
*	Code for the sensor nodes (ZigBee adresses 2-5). 
* Only purpose: Collect light sensor values and send them to the master node (ZigBee adresse 1). 
*/

#include "Timer.h"
#include "SensorValue.h"

module SensorP @safe()
{
  uses {
    interface Boot;
    interface SplitControl as RadioControl;
    interface AMSend as RadioSend;
    interface Packet as RadioPacket;
    interface Timer<TMilli>;
    interface Read<uint16_t>;
    interface Leds;
  }
}
implementation
{
	enum {
    SAMPLE_PERIOD = 1000,		//Timer period in ms
		MASTER_ADDRESS = 1
  };

	message_t pkt;		//Message packet to fill later

  void report_problem() { call Leds.led0Toggle(); }		//Red LED
  void report_sent() { call Leds.led1Toggle(); }			//Green LED
  void report_timer_fired() { call Leds.led2Toggle(); }	//Blue LED

	bool busy = FALSE;

  event void Boot.booted() {
		call Timer.startPeriodic(SAMPLE_PERIOD);		//seccessfull -> start timer for data collection
  }

  event void RadioControl.startDone(error_t error) {		//ISR called after attempt to start radio communication
	  if (error != SUCCESS) {
	    report_problem();			
			call RadioControl.start();			//not successfull -> try again
		} else {
			if(!busy) {	//check if raido still busy
				busy = TRUE;			
				call RadioSend.send(AM_BROADCAST_ADDR, &pkt, sizeof(SensorValueMsg));	//Send data to Master node (adress 1) via ZigBee
			}
		}
  }

  event void RadioControl.stopDone(error_t error) {}

//  event message_t* Receive.receive(message_t* msg, void* payload, uint8_t len) {}


  event void Timer.fired() {
		report_timer_fired();		//toggle blue led (broken on most boards)

		call RadioControl.start();	//start radio communication
	  
    if (call Read.read() != SUCCESS)	
      report_problem();		//Error reading sensor data -> toggle red led
  }

  event void RadioSend.sendDone(message_t* msg, error_t error) {		//ISR called after a package is sent
		if (&pkt == msg) {
			busy = FALSE;
		  if (error == SUCCESS)
		    report_sent();		//everything ok -> toggle gerren led
		  else
		    report_problem();	//error -> toggle red led
			call RadioControl.stop();	//stop radio communication, save power
		}
  }

  event void Read.readDone(error_t result, uint16_t data) {
		SensorValueMsg* msg;
    if (result != SUCCESS)		//Set data invalid if a problem occured reading the sensor data
    {
			data = 0xffff;
			report_problem();
    }

		msg = (SensorValueMsg*)(call RadioPacket.getPayload(&pkt, sizeof(SensorValueMsg)));	//Get pointer to message payload
		msg->value = data;		//Write sensor data to payload
		msg->node_id = TOS_NODE_ID;		//Send node id, for debugging
  }
}
