#include "WiFiService.h"
#include "CameraHTTPServer.h"
#include "Arduino.h"
#include "Camera.h"

WiFiService *_wifiService = new WiFiService();
Camera *_camera;
CameraHTTPServer *_cameraServer;

void setup()
{
  Serial.begin(115200);
  while (!_wifiService->initWiFi(3))
    ;
  _camera = new Camera();
  _cameraServer = new CameraHTTPServer(_camera, 80);
}

void loop()
{
  _wifiService->makeSureWiFiConnectionUp();
  _cameraServer->handleClient();
}