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

import ApiClient from '../ApiClient';
import DetectedObject from './DetectedObject';

/**
 * The ObjectDetectionResponse model module.
 * @module model/ObjectDetectionResponse
 * @version 1.0
 */
class ObjectDetectionResponse {
    /**
     * Constructs a new <code>ObjectDetectionResponse</code>.
     * @alias module:model/ObjectDetectionResponse
     */
    constructor() { 
        
        ObjectDetectionResponse.initialize(this);
    }

    /**
     * Initializes the fields of this object.
     * This method is used by the constructors of any subclasses, in order to implement multiple inheritance (mix-ins).
     * Only for internal use.
     */
    static initialize(obj) { 
    }

    /**
     * Constructs a <code>ObjectDetectionResponse</code> from a plain JavaScript object, optionally creating a new instance.
     * Copies all relevant properties from <code>data</code> to <code>obj</code> if supplied or a new instance if not.
     * @param {Object} data The plain JavaScript object bearing properties of interest.
     * @param {module:model/ObjectDetectionResponse} obj Optional instance to populate.
     * @return {module:model/ObjectDetectionResponse} The populated <code>ObjectDetectionResponse</code> instance.
     */
    static constructFromObject(data, obj) {
        if (data) {
            obj = obj || new ObjectDetectionResponse();

            if (data.hasOwnProperty('detectedObjects')) {
                obj['detectedObjects'] = ApiClient.convertToType(data['detectedObjects'], [DetectedObject]);
            }
        }
        return obj;
    }


}

/**
 * @member {Array.<module:model/DetectedObject>} detectedObjects
 */
ObjectDetectionResponse.prototype['detectedObjects'] = undefined;






export default ObjectDetectionResponse;

