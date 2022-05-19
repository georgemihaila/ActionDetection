#include "WiFiService.h"
#include "Config.h"
#include "ConfigurationServer.h"
#include "Arduino.h"

WiFiService *_wifiService = new WiFiService();
Config *_config = new Config();
ConfigurationServer *_configServer;

void setup()
{
  Serial.begin(115200);
  while (!_wifiService->initWiFi(3)) ;
  _configServer = new ConfigurationServer(_config, 80);
}

void loop()
{
  _wifiService->makeSureWiFiConnectionUp();
  _configServer->handleClient();
}