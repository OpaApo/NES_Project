/*
*
*/

configuration ActuatorC {
}
implementation {
  components MainC, ActuatorP, LedsC;
  components ActiveMessageC as Radio;
	components PlatformSerialC as SerialCom;

  MainC.Boot <- ActuatorP;

  ActuatorP.RadioControl -> Radio;
	ActuatorP.Serial -> SerialCom;
	ActuatorP.SerialControl -> SerialCom;

  ActuatorP.RadioReceive -> Radio.Receive;
  ActuatorP.RadioSnoop -> Radio.Snoop;
  ActuatorP.RadioPacket -> Radio;
  ActuatorP.RadioAMPacket -> Radio;
  
  ActuatorP.Leds -> LedsC;
}
