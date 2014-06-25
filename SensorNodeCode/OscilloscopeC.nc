/*
*
*/
#include "Timer.h"
#include "Oscilloscope.h"

module OscilloscopeC @safe()
{
  uses {
    interface Boot;
    interface SplitControl as RadioControl;
    interface AMSend;
    interface Receive;
    interface Timer<TMilli>;
    interface Read<uint16_t>;
    interface Leds;
  }
}
implementation
{
  message_t sendBuf;
  bool sendBusy;

  uint16_t currentData = 0;

  bool suppressCountChange;

  // Use LEDs to report various status issues.
  void report_problem() { call Leds.led0Toggle(); }
  void report_sent() { call Leds.led1Toggle(); }
  void report_received() { call Leds.led2Toggle(); }

  event void Boot.booted() {
    if (call RadioControl.start() != SUCCESS)
      report_problem();
  }

  void startTimer() {
		call Timer.startPeriodic(1000);    
  }

  event void RadioControl.startDone(error_t error) {
    startTimer();
  }

  event void RadioControl.stopDone(error_t error) {
  }

  event message_t* Receive.receive(message_t* msg, void* payload, uint8_t len) {
    oscilloscope_t *omsg = payload;

    report_received();

    return msg;
  }


  event void Timer.fired() {
		if (!sendBusy && sizeof currentData <= call AMSend.maxPayloadLength())
			{
			  memcpy(call AMSend.getPayload(&sendBuf, sizeof(currentData)), &currentData, sizeof currentData);
			  if (call AMSend.send(AM_BROADCAST_ADDR, &sendBuf, sizeof currentData) == SUCCESS)
			    sendBusy = TRUE;
			}
		if (!sendBusy)
			report_problem();

    if (call Read.read() != SUCCESS)
      report_problem();
  }

  event void AMSend.sendDone(message_t* msg, error_t error) {
    if (error == SUCCESS)
      report_sent();
    else
      report_problem();

    sendBusy = FALSE;
  }

  event void Read.readDone(error_t result, uint16_t data) {
    if (result != SUCCESS)
    {
			data = 0xffff;
			report_problem();
    }
		currentData = data;
  }
}
