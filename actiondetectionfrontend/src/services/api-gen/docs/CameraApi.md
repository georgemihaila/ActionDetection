# ActionDetectionApi.CameraApi

All URIs are relative to *http://localhost*

Method | HTTP request | Description
------------- | ------------- | -------------
[**cameraGetFrameGet**](CameraApi.md#cameraGetFrameGet) | **GET** /Camera/GetFrame | 
[**cameraListGet**](CameraApi.md#cameraListGet) | **GET** /Camera/List | 



## cameraGetFrameGet

> cameraGetFrameGet(opts)



### Example

```javascript
import ActionDetectionApi from 'action_detection_api';

let apiInstance = new ActionDetectionApi.CameraApi();
let opts = {
  'cameraIP': "cameraIP_example", // String | 
  'imageSize': new ActionDetectionApi.ImageSize(), // ImageSize | 
  'sensitivity': 7, // Number | 
  'showMotion': true, // Boolean | 
  'chunks': 64 // Number | 
};
apiInstance.cameraGetFrameGet(opts, (error, data, response) => {
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
 **sensitivity** | **Number**|  | [optional] [default to 7]
 **showMotion** | **Boolean**|  | [optional] [default to true]
 **chunks** | **Number**|  | [optional] [default to 64]

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

