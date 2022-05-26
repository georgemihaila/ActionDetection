import ApiClient from './api-gen/src/ApiClient'
import CameraApi from './api-gen/src/api/CameraApi'

var apiAddress = "http://10.10.0.157:5219";
var client = new ApiClient(apiAddress);
var globalCameraAPI = new CameraApi(client);
export { globalCameraAPI, apiAddress };