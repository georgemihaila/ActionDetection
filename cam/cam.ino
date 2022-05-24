#include "WiFiService.h"
#include "Config.h"
#include "CameraHTTPServer.h"
#include "Arduino.h"
#include "Camera.h"

WiFiService *_wifiService = new WiFiService();
Config *_config = new Config();
Camera *_camera;
CameraHTTPServer *_cameraServer;

void setup()
{
  Serial.begin(115200);
  while (!_wifiService->initWiFi(3))
    ;
  _camera = new Camera(_config);
  _cameraServer = new CameraHTTPServer(_config, _camera, 80);
}

void loop()
{
  _wifiService->makeSureWiFiConnectionUp();
  _cameraServer->handleClient();
}