import ApiClient from './api-gen/src/ApiClient'
import CameraApi from './api-gen/src/api/CameraApi'

var client = new ApiClient("http://localhost:5219");
var globalCameraAPI = new CameraApi(client);
export default globalCameraAPI;