/*
*	Master node: Receives light intensity values from every sensor node. 
* Calculates sun position from sensor values and sends them via UART 
* to the PC.
*
*	Node Adresses:
* Addr. 1: Master Node
* Addr. 2: Sensor Node 1
* Addr. 3: Sensor Node 2
*				.
*				.
*				.
* Addr. SENSOR_NODE_COUNT+1: Sensor Node SENSOR_NODE_COUNT
*
*/

#include "AM.h"
#include "Serial.h"
#include "Msp430Adc12.h"
#include "SensorValue.h"

module MasterP @safe() {
  provides {
    interface AdcConfigure<const msp430adc12_channel_config_t*> as config;
  }
  uses {
    interface Boot;
    interface SplitControl as RadioControl;
		
		interface StdControl as SerialControl;		//Uart Byte-Stream Controller, HIL
		interface UartStream as Serial;						//Uart Byte-Stream, HIL
    
    interface Receive as RadioReceive;

		interface Read<uint16_t> as ADCRead;

    interface Leds;
  }
}

implementation
{
  enum {
    SENSOR_NODE_COUNT = 4,					//Sensor nodes
  };

	const msp430adc12_channel_config_t configVal = {		//Configuration for ADC (Solar panel)
      inch: 0,
      sref: REFERENCE_VREFplus_AVss,
      ref2_5v: REFVOLT_LEVEL_1_5,
      adc12ssel: SHT_SOURCE_ACLK,
      adc12div: SHT_CLOCK_DIV_1,
      sht: SAMPLE_HOLD_4_CYCLES,
      sampcon_ssel: SAMPCON_SOURCE_SMCLK,
      sampcon_id: SAMPCON_CLOCK_DIV_1
  };

	uint16_t nodePayload[SENSOR_NODE_COUNT];	//Payload of last package from nodes with address 2 TO SENSOR_NODE_COUNT+2
  uint16_t calc_result = 0;		//Result of sun pos. calculation
	uint16_t solar = 0;		//Read ADC value form solar panel
	uint8_t lastUpdatedNode = 0;
	//uint16_t serialTaskAction = 0; // 0: sun data; 1: ADC voltage

  void report_problem() { call Leds.led0Toggle(); }		//Red
  void report_received() { call Leds.led1Toggle(); }			//Green
  void report_calc_error() { call Leds.led2Toggle(); }	//Blue

  task void uartSendTask();

	task void calculateResultTask();

	event void Boot.booted() {
		uint8_t i; 

		for (i = 0; i < SENSOR_NODE_COUNT; i++)
			nodePayload[i] = 0;

		call RadioControl.start();		//Start ZigBee connection

		call SerialControl.start();		//Start serial connection to PC

		//call Timer0.startPeriodic(CALCULATION_PERIOD);		//start the timer
	}

	/*event void Timer0.fired() {		//Timer interrupt		
		call ADCRead.read();
		serialTaskAction = 1;
		post uartSendTask();			//Send calc_result to PC (Uart)
	}*/


  event void RadioControl.startDone(error_t error) {		//attempted to start radio communication
		if (error != SUCCESS) {		//Retry if attempt failed
      report_problem();
			call RadioControl.start();
		}			

	}

	async event void Serial.receivedByte(uint8_t byte) {/*Never used*/}
	async event void Serial.receiveDone(uint8_t *buf, uint16_t len, error_t error) {
		if (error != SUCCESS)		//red Led if receiving via ZigBee not successful
			report_problem();
	}
	async event void Serial.sendDone(uint8_t *buf, uint16_t len, error_t error) {
		if (error != SUCCESS)	//red Led if sending via UART not successful
			report_problem();
	}

  event void RadioControl.stopDone(error_t error) {}

  message_t* ONE receive(message_t* ONE msg, void* payload, uint8_t len);
 

/*
*
*	Communication Sensors (ZigBee) -> Master (ZigBee)
*
*/
  
  event message_t *RadioReceive.receive(message_t *msg, void *payload, uint8_t len) {	//Receiving ZigBee package
    atomic
		{
			SensorValueMsg* pkt = (SensorValueMsg*)payload;		//Get pointer to package paylaod
			uint8_t addr = pkt->node_id;		//Get node ID
			//1: maser, 6: servor controller
			if (addr-2 >= 0 && addr < SENSOR_NODE_COUNT+2 && pkt->value != 0xffff) {	//Prevent pointing to wrong memory location due to corrupted packet source address and wrong calculation due to corrupted sensor value (0xffff)
				nodePayload[addr-2] = pkt->value; //put message payload (16bit unsigned int) into array slot corresponding to node address-2.
				lastUpdatedNode = addr;
			}
		}
		
		report_received();	//Green LED
		call ADCRead.read(); //Read soar ADC
		post calculateResultTask();	//Calc new sun position
		return msg;
  }

	task void calculateResultTask() {
		// --------------------
		// sun angle calc
		// --------------------
		uint16_t data2, data3, data4, data5;
		data2 = nodePayload[0];
  	data3 = nodePayload[1];
		data4 = nodePayload[2];
		data5 = nodePayload[3];
		if (data2 != 0 && data3 != 0 && data4 != 0 && data5 != 0 )
		{
			if (data5 > data2 && data2 > data4 && data4 > data3){
				calc_result = 270;
			}
			else if (data2 > data5 && data5 > data3 && data3 > data4){
				calc_result = 295;
			}
			else if (data2 > data3 && data3 > data5 && data5 > data4){
				calc_result = 0;
			}
			else if (data3 > data2 && data2 > data4 && data4 > data5){
				calc_result = 30;
			}
			else if (data3 > data4 && data4 > data2 && data2 > data5){
				calc_result = 90;
			}
			else if (data4 > data3 && data3 > data5 && data5 > data2){
				calc_result = 120;
			}
			else if (data4 > data5 && data5 > data3 && data3 > data2){
				calc_result = 180;			
			}
			else if (data5 > data4 && data4 > data2 && data2 > data3){
				calc_result = 210;
			}
	  	else {
				report_calc_error();
	  	}
		}

		//SerialTaskAction = 0;
		post uartSendTask();			//Send result to PC (Uart)
	}



/*
*
*	Communication Sensors (ZigBee) -> PC (UART)
*
*/
  
  task void uartSendTask() {		//Send data via UART to PC
		uint8_t buf[8];		
		atomic {
			// 0xee
			// last updated node nr
			// measured intensity of node
			// calculated angle
			//0xdd			
			// send sun data
			buf[0] = 0xee;			
			buf[1] = (uint8_t)lastUpdatedNode-2;
			buf[2] = (uint8_t)nodePayload[lastUpdatedNode-2];
			buf[3] = (uint8_t)(nodePayload[lastUpdatedNode-2] >> 8);
			buf[4] = (uint8_t)calc_result;	//Decompose 16bit calc_result into two seperatee byte, MSB first
			buf[5] = (uint8_t)(calc_result >> 8);			
			buf[6] = (uint8_t)solar;
			buf[7] = (uint8_t)(solar >> 8);
		} 
		call Serial.send(buf, 8);	//Send the 8 byte via uart to PC
  }

/*
*	ADC to read voltage from solar panel
*
*/

  event void ADCRead.readDone( error_t result, uint16_t val )		//ADC connected to solar panel read
  {
    if (result == SUCCESS){
			solar = val;
    }
  }

  async command const msp430adc12_channel_config_t* config.getConfiguration()
  {
    return &configVal; // configuration for ADC
  }
}

