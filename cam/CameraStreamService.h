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
    CameraStreamService(Camera* camera, String endpointAddress, framesize_t res, double maxfps = 1){
        _camera = camera;
        _endpointAddress = endpointAddress;
        resolution = res;
        maxFPS = maxfps;
    }
    void start();
    void stop();
    void yield();
    framesize_t resolution;
    double maxFPS;
private:
    Camera* _camera;
    String _endpointAddress;
    bool _running;
    unsigned long _lastSendTimestamp = 0;
};

#endif