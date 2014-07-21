/*
*
*/

configuration MasterC {
}
implementation {
  components MainC, MasterPAdvanced, LedsC;
  components ActiveMessageC as Radio;
  components new TimerMilliC() as Timer0;
	components PlatformSerialC as SerialCom;
	components new AdcReadClientC() as ADC;
	components new AMReceiverC(AM_SENSORVALUE);

  MainC.Boot <- MasterPAdvanced;

  MasterPAdvanced.RadioControl -> Radio;
	MasterPAdvanced.Serial -> SerialCom;
	MasterPAdvanced.SerialControl -> SerialCom;
  
	MasterPAdvanced.ADCRead -> ADC;
  ADC.AdcConfigure -> MasterPAdvanced.config;

  MasterPAdvanced.RadioReceive -> AMReceiverC;
  
  MasterPAdvanced.Leds -> LedsC;
}
