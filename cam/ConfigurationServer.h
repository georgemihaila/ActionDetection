#ifndef ConfigurationServer_h_
#define ConfigurationServer_h_

#include "Arduino.h"
#include "Config.h"
#include "WebServer.h"
#include "ESP32SSDP.h"
#include <ArduinoJson.h>

class ConfigurationServer
{
public:
    ConfigurationServer(Config *config, int port);
    void handleClient();

private:
    Config *_config;
};

#endif