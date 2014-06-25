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

  MainC.Boot <- MasterP;

  MasterP.RadioControl -> Radio;
	MasterP.Serial -> SerialCom;
	MasterP.SerialControl -> SerialCom;
  
  
  MasterP.RadioSend -> Radio;
  MasterP.RadioReceive -> Radio.Receive;
  MasterP.RadioSnoop -> Radio.Snoop;
  MasterP.RadioPacket -> Radio;
  MasterP.RadioAMPacket -> Radio;

  MasterP.Timer0 -> Timer0;
  
  MasterP.Leds -> LedsC;
}
