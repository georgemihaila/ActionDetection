#include "CameraHTTPServer.h"
#include "CameraStreamService.h"

WebServer *_server;
Camera *_csCam;
CameraStreamService *_csStreamService;

void handleListCapabilities()
{
    const size_t capacity = JSON_ARRAY_SIZE(1);
    DynamicJsonDocument doc(capacity);
    doc.add("camera");
    doc.add("frame");
    String result;
    serializeJson(doc, result);
    _server->setContentLength(result.length());
    _server->send(200, "application/json", result);
}

void handle_NotFound()
{
    _server->send(404, "text/plain", "Not found");
}

void handleSSDP()
{
    SSDP.schema(_server->client());
}

void handleGetUXGA()
{
    _csCam->respondWithFrame(_server, FRAMESIZE_UXGA);
}

void handleGetVGA()
{
    _csCam->respondWithFrame(_server, FRAMESIZE_VGA);
}

void handleGetQQVGA()
{
    _csCam->respondWithFrame(_server, FRAMESIZE_QQVGA);
}

void startStream()
{
    _csStreamService->start();
    _server->send(200, "text/plain", "Stream started");
}

void stopStream()
{
    _csStreamService->stop();
    _server->send(200, "text/plain", "Stream stopped");
}

void streamSize()
{
    _server->send(200, "text/plain", String(_csStreamService->resolution));
}

void return200()
{
    _server->send(200, "text/plain", "ok");
}

void handleStreamUXGA()
{
    _csStreamService->resolution = FRAMESIZE_UXGA;
    return200();
}

void handleStreamVGA()
{
    _csStreamService->resolution = FRAMESIZE_VGA;
    return200();
}

void handleStreamQQVGA()
{
    _csStreamService->resolution = FRAMESIZE_QQVGA;
    return200();
}

void handleStreamLowFPS()
{
    _csStreamService->maxFPS = 1;
    return200();
}

void handleStreamHighFPS()
{
    _csStreamService->maxFPS = 10;
    return200();
}

CameraHTTPServer::CameraHTTPServer(Camera *camera, int port)
{
    _server = new WebServer(port);
    _csCam = camera;
    _csStreamService = new CameraStreamService(_csCam, "http://10.10.0.157:5219/Camera/SetFrame", FRAMESIZE_VGA, 3);

    _server->on("/description.xml", handleSSDP);
    _server->on("/capabilities", handleListCapabilities);

    _server->on("/uxga.jpg", handleGetUXGA);
    _server->on("/vga.jpg", handleGetVGA);
    _server->on("/qqvga.jpg", handleGetQQVGA);

    _server->on("/startStream", startStream);
    _server->on("/streamSize", startStream);
    _server->on("/stopStream", stopStream);
    _server->on("/streamLowFPS", handleStreamLowFPS);
    _server->on("/streamHighFPS", handleStreamHighFPS);

    _server->on("/streamuxga", handleStreamUXGA);
    _server->on("/streamvga", handleStreamVGA);
    _server->on("/streamqqvga", handleStreamQQVGA);
    /*_server->on("/setResolution", HTTP_GET, [](AsyncWebServerRequest * request)
    {

    });*/
    _server->onNotFound(handle_NotFound);
    _server->begin();

    SSDP.setSchemaURL("description.xml");
    SSDP.setHTTPPort(80);
    SSDP.setName("ESP32-Cam");
    SSDP.setModelName("ESP32 WiFi camera v0.1");
    SSDP.setModelNumber("ESP32CAMv0");
    SSDP.setDeviceType("upnp:rootdevice");
    SSDP.begin();
}

void CameraHTTPServer::handleClient()
{
    _server->handleClient();
    _csStreamService->yield();
}