#ifndef SENSORVALUE_H
#define SENSORVALUE_H

enum {
  AM_SENSORVALUE = 6
};

typedef nx_struct SensorValueMsg {
  nx_uint16_t value;
	nx_uint8_t node_id;
} SensorValueMsg;

#endif
