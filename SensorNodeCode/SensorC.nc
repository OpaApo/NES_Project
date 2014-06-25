/*
 *
 */
configuration SensorC { }
implementation
{
  components SensorP, MainC, ActiveMessageC, LedsC,
    new TimerMilliC(),
	// default sensor board driver (Tmote - temp?)
	// new DemoSensorC() as Sensor, 
	// Temperature/Humididy
	// new SensirionSht11C() as Sensor,
	// Photosensor/Lightsensor
	new HamamatsuS10871TsrC() as Sensor,
    new AMSenderC(AM_OSCILLOSCOPE), new AMReceiverC(AM_OSCILLOSCOPE);

  SensorP.Boot -> MainC;
  SensorP.RadioControl -> ActiveMessageC;
  SensorP.AMSend -> AMSenderC;
  SensorP.Receive -> AMReceiverC;
  SensorP.Timer -> TimerMilliC;
  SensorP.Read -> Sensor;
  SensorP.Leds -> LedsC;

  
}
