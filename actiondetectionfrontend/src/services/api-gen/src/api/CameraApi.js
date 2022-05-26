/**
 * ActionDetection.API
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 * The version of the OpenAPI document: 1.0
 * 
 *
 * NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
 * https://openapi-generator.tech
 * Do not edit the class manually.
 *
 */


import ApiClient from "../ApiClient";
import ImageSize from '../model/ImageSize';

/**
* Camera service.
* @module api/CameraApi
* @version 1.0
*/
export default class CameraApi {

    /**
    * Constructs a new CameraApi. 
    * @alias module:api/CameraApi
    * @class
    * @param {module:ApiClient} [apiClient] Optional API client implementation to use,
    * default to {@link module:ApiClient#instance} if unspecified.
    */
    constructor(apiClient) {
        this.apiClient = apiClient || ApiClient.instance;
    }


    /**
     * Callback function to receive the result of the cameraGetFrameGet operation.
     * @callback module:api/CameraApi~cameraGetFrameGetCallback
     * @param {String} error Error message, if any.
     * @param data This operation does not return a value.
     * @param {String} response The complete HTTP response.
     */

    /**
     * @param {Object} opts Optional parameters
     * @param {String} opts.cameraIP 
     * @param {module:model/ImageSize} opts.imageSize 
     * @param {Number} opts.sensitivity  (default to 7)
     * @param {Boolean} opts.showMotion  (default to true)
     * @param {Number} opts.chunks  (default to 64)
     * @param {module:api/CameraApi~cameraGetFrameGetCallback} callback The callback function, accepting three arguments: error, data, response
     */
    cameraGetFrameGet(opts, callback) {
      opts = opts || {};
      let postBody = null;

      let pathParams = {
      };
      let queryParams = {
        'cameraIP': opts['cameraIP'],
        'imageSize': opts['imageSize'],
        'sensitivity': opts['sensitivity'],
        'showMotion': opts['showMotion'],
        'chunks': opts['chunks']
      };
      let headerParams = {
      };
      let formParams = {
      };

      let authNames = [];
      let contentTypes = [];
      let accepts = [];
      let returnType = null;
      return this.apiClient.callApi(
        '/Camera/GetFrame', 'GET',
        pathParams, queryParams, headerParams, formParams, postBody,
        authNames, contentTypes, accepts, returnType, null, callback
      );
    }

    /**
     * Callback function to receive the result of the cameraListGet operation.
     * @callback module:api/CameraApi~cameraListGetCallback
     * @param {String} error Error message, if any.
     * @param {Array.<String>} data The data returned by the service call.
     * @param {String} response The complete HTTP response.
     */

    /**
     * @param {module:api/CameraApi~cameraListGetCallback} callback The callback function, accepting three arguments: error, data, response
     * data is of type: {@link Array.<String>}
     */
    cameraListGet(callback) {
      let postBody = null;

      let pathParams = {
      };
      let queryParams = {
      };
      let headerParams = {
      };
      let formParams = {
      };

      let authNames = [];
      let contentTypes = [];
      let accepts = ['text/plain', 'application/json', 'text/json'];
      let returnType = ['String'];
      return this.apiClient.callApi(
        '/Camera/List', 'GET',
        pathParams, queryParams, headerParams, formParams, postBody,
        authNames, contentTypes, accepts, returnType, null, callback
      );
    }


}
