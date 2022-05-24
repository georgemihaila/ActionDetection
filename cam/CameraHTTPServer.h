#ifndef CameraHTTPServer_h_
#define CameraHTTPServer_h_

#include "Arduino.h"
#include "Config.h"
#include "WebServer.h"
#include "ESP32SSDP.h"
#include <ArduinoJson.h>
#include "Camera.h"

class CameraHTTPServer
{
public:
    CameraHTTPServer(Config *config, Camera* camera, int port);
    void handleClient();

private:
    Config *_config;
};

#endif