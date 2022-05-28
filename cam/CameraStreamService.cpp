#include "CameraStreamService.h"

void CameraStreamService::start()
{
    _running = true;
}

void CameraStreamService::stop()
{
    _running = false;
}

void CameraStreamService::yield()
{
    if (_running)
    {
        if (millis() - _lastSendTimestamp >= 1000 / _maxFPS)
        {
            _camera->postFrame(_endpointAddress, _resolution);
            _lastSendTimestamp = millis();
        }
    }
}