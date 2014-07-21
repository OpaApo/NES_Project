/*
*
*/

configuration MasterC {
}
implementation {
  components MainC, MasterPSimple, LedsC;
  components ActiveMessageC as Radio;
  components new TimerMilliC() as Timer0;
	components PlatformSerialC as SerialCom;
	components new AdcReadClientC() as ADC;
	components new AMReceiverC(AM_SENSORVALUE);

  MainC.Boot <- MasterPSimple;

  MasterPSimple.RadioControl -> Radio;
	MasterPSimple.Serial -> SerialCom;
	MasterPSimple.SerialControl -> SerialCom;
  
	MasterPSimple.ADCRead -> ADC;
  ADC.AdcConfigure -> MasterPSimple.config;

  MasterPSimple.RadioReceive -> AMReceiverC;
  
  MasterPSimple.Leds -> LedsC;
}
