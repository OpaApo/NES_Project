/*
*	Code for the sensor nodes (ZigBee adresses 2-5). 
* Only purpose: Collect light sensor values and send them to the master node (ZigBee adresse 1). 
*/

#include "Timer.h"

module SensorP @safe()
{
  uses {
    interface Boot;
    interface SplitControl as RadioControl;
    interface AMSend as RadioSend[am_id_t id];
    interface Packet as RadioPacket;
    //interface Receive as RadioReceive[am_id_t id];
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

  uint16_t lastSensorData = 0;		//Last sensor value

  void report_problem() { call Leds.led0Toggle(); }		//Red LED
  void report_sent() { call Leds.led1Toggle(); }			//Green LED
  void report_timer_fired() { call Leds.led2Toggle(); }	//Blue LED

  event void Boot.booted() {
		call Timer.startPeriodic(SAMPLE_PERIOD);		//seccessfull -> start timer for data collection
  }

  event void RadioControl.startDone(error_t error) {		//ISR called after attempt to start radio communication
		atomic {
		  if (error != SUCCESS) {
		    report_problem();			
				call RadioControl.start();			//not successfull -> try again
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

  event void RadioSend.sendDone[am_id_t id](message_t* msg, error_t error) {		//ISR called after a package is sent
    if (error == SUCCESS)
      report_sent();		//everything ok -> toggle gerren led
    else
      report_problem();	//error -> toggle red led
		call RadioControl.stop();	//stop radio communication, save power
  }

  event void Read.readDone(error_t result, uint16_t data) {
		uint16_t* pl;
    message_t pkt;
    if (result != SUCCESS)		//Set data invalid if a problem occured reading the sensor data
    {
			data = 0xffff;
			report_problem();
    }
		lastSensorData = data;		//Save sensor data, just to be sure :)
		
		pl = (uint16_t*)call RadioPacket.getPayload(&pkt, sizeof lastSensorData);	//Get pointer to payload of 2 byte for the message
		*pl = lastSensorData;		//Write sensor data to payload
			
		call RadioSend.send[0](MASTER_ADDRESS, &pkt, sizeof lastSensorData);	//Send data to Master node (adress 1) via ZigBee
  }
}
