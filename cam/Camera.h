#ifndef Camera_h_
#define Camera_h_

#include "Arduino.h"
#include "esp_camera.h"
#include "esp_timer.h"
#include "img_converters.h"
#include "Arduino.h"
#include "fb_gfx.h"
#include "soc/soc.h"          //disable brownout problems
#include "soc/rtc_cntl_reg.h" //disable brownout problems
#include "WebServer.h"

class Camera
{
public:
    Camera();
    void respondWithFrame(WebServer *server, framesize_t resolution);
};

#endif