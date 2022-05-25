# ActionDetectionApi.CameraApi

All URIs are relative to *http://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**cameraDetectObjectsInCameraViewGet**](CameraApi.md#cameraDetectObjectsInCameraViewGet) | **GET** /Camera/DetectObjectsInCameraView | 
[**cameraGetDetectionImageGet**](CameraApi.md#cameraGetDetectionImageGet) | **GET** /Camera/GetDetectionImage | 
[**cameraListGet**](CameraApi.md#cameraListGet) | **GET** /Camera/List | 



## cameraDetectObjectsInCameraViewGet

> ObjectDetectionResponse cameraDetectObjectsInCameraViewGet(opts)



### Example

```javascript
import ActionDetectionApi from 'action_detection_api';

let apiInstance = new ActionDetectionApi.CameraApi();
let opts = {
  'cameraIP': "cameraIP_example", // String | 
  'imageSize': new ActionDetectionApi.ImageSize() // ImageSize | 
};
apiInstance.cameraDetectObjectsInCameraViewGet(opts, (error, data, response) => {
  if (error) {
    console.error(error);
  } else {
    console.log('API called successfully. Returned data: ' + data);
  }
});
```

### Parameters


Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **cameraIP** | **String**|  | [optional] 
 **imageSize** | [**ImageSize**](.md)|  | [optional] 

### Return type

[**ObjectDetectionResponse**](ObjectDetectionResponse.md)

### Authorization

No authorization required

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: text/plain, application/json, text/json


## cameraGetDetectionImageGet

> cameraGetDetectionImageGet(opts)



### Example

```javascript
import ActionDetectionApi from 'action_detection_api';

let apiInstance = new ActionDetectionApi.CameraApi();
let opts = {
  'cameraIP': "cameraIP_example", // String | 
  'imageSize': new ActionDetectionApi.ImageSize() // ImageSize | 
};
apiInstance.cameraGetDetectionImageGet(opts, (error, data, response) => {
  if (error) {
    console.error(error);
  } else {
    console.log('API called successfully.');
  }
});
```

### Parameters


Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **cameraIP** | **String**|  | [optional] 
 **imageSize** | [**ImageSize**](.md)|  | [optional] 

### Return type

null (empty response body)

### Authorization

No authorization required

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: Not defined


## cameraListGet

> [String] cameraListGet()



### Example

```javascript
import ActionDetectionApi from 'action_detection_api';

let apiInstance = new ActionDetectionApi.CameraApi();
apiInstance.cameraListGet((error, data, response) => {
  if (error) {
    console.error(error);
  } else {
    console.log('API called successfully. Returned data: ' + data);
  }
});
```

### Parameters

This endpoint does not need any parameter.

### Return type

**[String]**

### Authorization

No authorization required

### HTTP request headers

- **Content-Type**: Not defined
- **Accept**: text/plain, application/json, text/json

