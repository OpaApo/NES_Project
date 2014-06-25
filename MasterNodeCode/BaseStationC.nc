/*
*
*/

configuration BaseStationC {
}
implementation {
  components MainC, BaseStationP, LedsC;
  components ActiveMessageC as Radio;
  components new TimerMilliC() as Timer0;
	components PlatformSerialC as SerialCom;

  MainC.Boot <- BaseStationP;

  BaseStationP.RadioControl -> Radio;
	BaseStationP.Serial -> SerialCom;
	BaseStationP.SerialControl -> SerialCom;
  
  
  BaseStationP.RadioSend -> Radio;
  BaseStationP.RadioReceive -> Radio.Receive;
  BaseStationP.RadioSnoop -> Radio.Snoop;
  BaseStationP.RadioPacket -> Radio;
  BaseStationP.RadioAMPacket -> Radio;

  BaseStationP.Timer0 -> Timer0;
  
  BaseStationP.Leds -> LedsC;
}
