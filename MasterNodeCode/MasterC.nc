/*
*
*/

configuration MasterC {
}
implementation {
  components MainC, MasterP, LedsC;
  components ActiveMessageC as Radio;
  components new TimerMilliC() as Timer0;
	components PlatformSerialC as SerialCom;
	components new AdcReadClientC() as ADC;
	components new AMReceiverC(AM_SENSORVALUE);

  MainC.Boot <- MasterP;

  MasterP.RadioControl -> Radio;
	MasterP.Serial -> SerialCom;
	MasterP.SerialControl -> SerialCom;
  
	MasterP.ADCRead -> ADC;
  ADC.AdcConfigure -> MasterP.config;

  MasterP.RadioReceive -> AMReceiverC;
  
  MasterP.Leds -> LedsC;
}
