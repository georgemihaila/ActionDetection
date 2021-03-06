#include "Camera.h"

#define PART_BOUNDARY "123456789000000000000987654321"

#define PWDN_GPIO_NUM 32
#define RESET_GPIO_NUM -1
#define XCLK_GPIO_NUM 0
#define SIOD_GPIO_NUM 26
#define SIOC_GPIO_NUM 27

#define Y9_GPIO_NUM 35
#define Y8_GPIO_NUM 34
#define Y7_GPIO_NUM 39
#define Y6_GPIO_NUM 36
#define Y5_GPIO_NUM 21
#define Y4_GPIO_NUM 19
#define Y3_GPIO_NUM 18
#define Y2_GPIO_NUM 5
#define VSYNC_GPIO_NUM 25
#define HREF_GPIO_NUM 23
#define PCLK_GPIO_NUM 22

static const char *_STREAM_CONTENT_TYPE = "multipart/x-mixed-replace;boundary=" PART_BOUNDARY;
static const char *_STREAM_BOUNDARY = "\r\n--" PART_BOUNDARY "\r\n";
static const char *_STREAM_PART = "Content-Type: image/jpeg\r\nContent-Length: %u\r\n\r\n";

#define LED_BUILTIN 4

camera_config_t camConfig;

Camera::Camera()
{
    pinMode(LED_BUILTIN, OUTPUT);
    camConfig.ledc_channel = LEDC_CHANNEL_0;
    camConfig.ledc_timer = LEDC_TIMER_0;
    camConfig.pin_d0 = Y2_GPIO_NUM;
    camConfig.pin_d1 = Y3_GPIO_NUM;
    camConfig.pin_d2 = Y4_GPIO_NUM;
    camConfig.pin_d3 = Y5_GPIO_NUM;
    camConfig.pin_d4 = Y6_GPIO_NUM;
    camConfig.pin_d5 = Y7_GPIO_NUM;
    camConfig.pin_d6 = Y8_GPIO_NUM;
    camConfig.pin_d7 = Y9_GPIO_NUM;
    camConfig.pin_xclk = XCLK_GPIO_NUM;
    camConfig.pin_pclk = PCLK_GPIO_NUM;
    camConfig.pin_vsync = VSYNC_GPIO_NUM;
    camConfig.pin_href = HREF_GPIO_NUM;
    camConfig.pin_sscb_sda = SIOD_GPIO_NUM;
    camConfig.pin_sscb_scl = SIOC_GPIO_NUM;
    camConfig.pin_pwdn = PWDN_GPIO_NUM;
    camConfig.pin_reset = RESET_GPIO_NUM;
    camConfig.xclk_freq_hz = 20000000;
    camConfig.pixel_format = PIXFORMAT_JPEG;

    if (psramFound())
    {
        camConfig.frame_size = FRAMESIZE_UXGA;
        camConfig.jpeg_quality = 10;
        camConfig.fb_count = 2;
    }
    else
    {
        camConfig.frame_size = FRAMESIZE_SVGA;
        camConfig.jpeg_quality = 12;
        camConfig.fb_count = 1;
    }

    esp_err_t err = esp_camera_init(&camConfig);
    if (err != ESP_OK)
    {
        Serial.printf("Camera init failed with error 0x%x", err);
    }
}

void Camera::respondWithFrame(WebServer *server, framesize_t resolution)
{
    digitalWrite(LED_BUILTIN, HIGH);
    delayMicroseconds(10);
    digitalWrite(LED_BUILTIN, LOW);
    camera_fb_t *fb = NULL;
    esp_err_t res = ESP_OK;
    size_t _jpg_buf_len = 0;
    uint8_t *_jpg_buf = NULL;
    char *part_buf[64];

    if (camConfig.frame_size != resolution)
    {
        esp_camera_deinit();
        camConfig.frame_size = resolution;
        esp_camera_init(&camConfig);
    }
    fb = esp_camera_fb_get();
    if (!fb)
    {
        Serial.println("Camera capture failed");
        res = ESP_FAIL;
    }
    else
    {
        if (fb->format != PIXFORMAT_JPEG)
        {
            bool jpeg_converted = frame2jpg(fb, 80, &_jpg_buf, &_jpg_buf_len);
            esp_camera_fb_return(fb);
            fb = NULL;
            if (!jpeg_converted)
            {
                Serial.println("JPEG compression failed");
                res = ESP_FAIL;
            }
        }
        else
        {
            _jpg_buf_len = fb->len;
            _jpg_buf = fb->buf;
        }
    }

    // size_t hlen = snprintf((char *)part_buf, 64, _STREAM_PART, _jpg_buf_len);
    // server->sendContent((const char *)part_buf, hlen);
    server->setContentLength(_jpg_buf_len);
    server->sendHeader("Content-Type", "image/jpeg");
    server->send(200);
    server->sendContent((const char *)_jpg_buf, _jpg_buf_len);
    if (fb)
    {
        esp_camera_fb_return(fb);
        fb = NULL;
        _jpg_buf = NULL;
    }
    else if (_jpg_buf)
    {
        free(_jpg_buf);
        _jpg_buf = NULL;
    }
}

void Camera::postFrame(String address, framesize_t resolution)
{
    camera_fb_t *fb = NULL;
    esp_err_t res = ESP_OK;
    size_t _jpg_buf_len = 0;
    uint8_t *_jpg_buf = NULL;
    char *part_buf[64];

    if (camConfig.frame_size != resolution)
    {
        esp_camera_deinit();
        camConfig.frame_size = resolution;
        esp_camera_init(&camConfig);
    }
    fb = esp_camera_fb_get();
    if (!fb)
    {
        Serial.println("Camera capture failed");
        res = ESP_FAIL;
    }
    else
    {
        if (fb->format != PIXFORMAT_JPEG)
        {
            bool jpeg_converted = frame2jpg(fb, 80, &_jpg_buf, &_jpg_buf_len);
            esp_camera_fb_return(fb);
            fb = NULL;
            if (!jpeg_converted)
            {
                Serial.println("JPEG compression failed");
                res = ESP_FAIL;
            }
        }
        else
        {
            _jpg_buf_len = fb->len;
            _jpg_buf = fb->buf;
        }
    }

    //<actual POST>

    _http.begin(_client, address);
    _http.addHeader("Content-Type", "x-binary");
    int response = _http.POST(_jpg_buf, _jpg_buf_len);
    if (!(response == 200 || response == 201))
    {
        Serial.println("POST frame to " + address + " failed with code " + String(response));
    }
    //</actual POST>
    if (fb)
    {
        esp_camera_fb_return(fb);
        fb = NULL;
        _jpg_buf = NULL;
    }
    else if (_jpg_buf)
    {
        free(_jpg_buf);
        _jpg_buf = NULL;
    }
}