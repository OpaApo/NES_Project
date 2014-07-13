/*
 * Cinfiguration for Sensor nodes
 */
configuration SensorC { }
implementation
{
  components SensorP, MainC, ActiveMessageC as Radio, LedsC,
    new TimerMilliC(),
	// default sensor board driver (Tmote - temp?)
	// new DemoSensorC() as Sensor, 
	// Temperature/Humididy
	// new SensirionSht11C() as Sensor,
	// Photosensor/Lightsensor
	new HamamatsuS10871TsrC() as Sensor;

  SensorP.Boot -> MainC;
  SensorP.RadioControl -> Radio;
  SensorP.RadioSend -> Radio;
	SensorP.RadioPacket -> Radio;
  //SensorP.RadioReceive -> Radio.Receive;
  SensorP.Timer -> TimerMilliC;
  SensorP.Read -> Sensor;
  SensorP.Leds -> LedsC;

  
}
