#include "ConfigurationServer.h"

WebServer *_server;
Camera *_csCam;

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

void handleGetFrame() // AsyncWebServerRequest *request)
{
    _csCam->respondWithFrame(_server);
}

ConfigurationServer::ConfigurationServer(Config *config, Camera *camera, int port)
{
    _config = config;
    _server = new WebServer(port);
    _csCam = camera;

    _server->on("/description.xml", handleSSDP);
    _server->on("/capabilities", handleListCapabilities);
    _server->on("/frame.jpg", handleGetFrame);
    _server->onNotFound(handle_NotFound);
    _server->begin();

    SSDP.setSchemaURL("description.xml");
    SSDP.setHTTPPort(80);
    SSDP.setName("ESP32-Cam");
    SSDP.setSerialNumber("ff00ff11");
    SSDP.setModelName("ESP32 WiFi camera v0.1");
    SSDP.setModelNumber("ESP32CAMv0");
    SSDP.setDeviceType("upnp:rootdevice");
    SSDP.begin();
}

void ConfigurationServer::handleClient()
{
    _server->handleClient();
}