#ifndef CameraHTTPServer_h_
#define CameraHTTPServer_h_

#include "Arduino.h"
#include "WebServer.h"
#include "ESP32SSDP.h"
#include <ArduinoJson.h>
#include "Camera.h"

class CameraHTTPServer
{
public:
    CameraHTTPServer(Camera* camera, int port);
    void handleClient();
};

#endif