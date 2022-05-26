# ActionDetectionApi.ObjectDetectionApi

All URIs are relative to *http://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**objectDetectionDetectObjectsInCameraViewGet**](ObjectDetectionApi.md#objectDetectionDetectObjectsInCameraViewGet) | **GET** /ObjectDetection/DetectObjectsInCameraView | 
[**objectDetectionGetDetectionImageGet**](ObjectDetectionApi.md#objectDetectionGetDetectionImageGet) | **GET** /ObjectDetection/GetDetectionImage | 



## objectDetectionDetectObjectsInCameraViewGet

> ObjectDetectionResponse objectDetectionDetectObjectsInCameraViewGet(opts)



### Example

```javascript
import ActionDetectionApi from 'action_detection_api';

let apiInstance = new ActionDetectionApi.ObjectDetectionApi();
let opts = {
  'cameraIP': "cameraIP_example", // String | 
  'imageSize': new ActionDetectionApi.ImageSize() // ImageSize | 
};
apiInstance.objectDetectionDetectObjectsInCameraViewGet(opts, (error, data, response) => {
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


## objectDetectionGetDetectionImageGet

> objectDetectionGetDetectionImageGet(opts)



### Example

```javascript
import ActionDetectionApi from 'action_detection_api';

let apiInstance = new ActionDetectionApi.ObjectDetectionApi();
let opts = {
  'cameraIP': "cameraIP_example", // String | 
  'imageSize': new ActionDetectionApi.ImageSize() // ImageSize | 
};
apiInstance.objectDetectionGetDetectionImageGet(opts, (error, data, response) => {
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

