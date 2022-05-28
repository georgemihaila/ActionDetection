#ifndef CameraStreamService_h_
#define CameraStreamService_h_

#include "Arduino.h"
#include "WebServer.h"
#include "ESP32SSDP.h"
#include <ArduinoJson.h>
#include "Camera.h"

class CameraStreamService
{
public:
    CameraStreamService(Camera* camera, String endpointAddress, framesize_t resolution, double maxFPS = 1){
        _camera = camera;
        _endpointAddress = endpointAddress;
        _resolution = resolution;
        _maxFPS = maxFPS;
    }
    void start();
    void stop();
    void yield();
private:
    Camera* _camera;
    String _endpointAddress;
    framesize_t _resolution;
    double _maxFPS;
    bool _running;
    unsigned long _lastSendTimestamp = 0;
};

#endif