/*
 * Cinfiguration for Sensor nodes
 */
configuration SensorC { }
implementation
{
  components SensorP, MainC, ActiveMessageC, LedsC, new TimerMilliC(), new AMSenderC(AM_SENSORVALUE), new HamamatsuS10871TsrC() as Sensor;

  SensorP.Boot -> MainC;
  SensorP.RadioControl -> ActiveMessageC;
  SensorP.RadioSend -> AMSenderC;
	SensorP.RadioPacket -> AMSenderC;
  SensorP.Timer -> TimerMilliC;
  SensorP.Read -> Sensor;
  SensorP.Leds -> LedsC;

  
}
