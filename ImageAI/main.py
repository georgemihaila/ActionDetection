from imageai.Detection import ObjectDetection
import os

execution_path = "/home/triton/Desktop/image detection"

accurate_detector = ObjectDetection()
accurate_detector.setModelTypeAsRetinaNet()
accurate_detector.setModelPath( os.path.join(execution_path , "resnet50_coco_best_v2.1.0.h5"))
accurate_detector.loadModel()
print("Accurate model loaded")

fast_detector = ObjectDetection()
fast_detector.setModelTypeAsTinyYOLOv3()
fast_detector.setModelPath( os.path.join(execution_path , "yolo-tiny.h5"))
fast_detector.loadModel()
print("Fast model loaded")

import json

def getImageFromDetection(detector, image, outFile):
    return detector.detectObjectsFromImage(input_image=image, input_type="array", output_type="array", minimum_percentage_probability=20)

def getJSONFromDetection(detector, image, outFile):
    detections = detector.detectObjectsFromImage(input_image=image, input_type="array", output_image_path='/dev/null/img.jpg', minimum_percentage_probability=20)
    return json.dumps(detections)

import sys
import cgi
from http.server import HTTPServer, SimpleHTTPRequestHandler
from urllib.parse import urlparse, parse_qs
import subprocess
from os import path
import skimage
from PIL import Image
import numpy as np
from array import array
import io

def image_to_byte_array(image: Image) -> bytes:
  imgByteArr = io.BytesIO()
  image.save(imgByteArr, format="PNG")
  imgByteArr = imgByteArr.getvalue()
  return imgByteArr

def download_file(url, directory, file_name, dl_path):
    #print(subprocess.check_output(['cd', '/home/triton/Desktop/']))
    if (path.exists(directory + file_name)):
        print(subprocess.check_call(['rm', directory + file_name]))
    print(subprocess.check_call(['wget', '-O', directory + file_name, dl_path]))
    
class NumpyEncoder(json.JSONEncoder):
    def default(self, obj):
        if isinstance(obj, np.ndarray):
            return obj.tolist()
        return json.JSONEncoder.default(self, obj)

HOST_NAME = "10.10.0.249"
PORT = 9544
class PythonServer(SimpleHTTPRequestHandler):
    def do_GET(self):
        query = parse_qs(self.path)
        print(query)
        ip = "http://" + query['/?ip'][0]
        filename =  query['size'][0] + ".jpg"
        path = ip + "/" + filename
        detection_method = getImageFromDetection
        return_type = query['type'][0].strip()
        if return_type == "json":
            detection_method = getJSONFromDetection
        elif return_type != "image":
            self.send_response(400, "Query parameter \"type\" missing or invalid. Accepted values: [\"json\", \"image\"]")
            self.end_headers()
            return
        print(return_type)
        
        #download_file(path, '/home/triton/Desktop/image detection/', filename, path)
        image_numpy = skimage.io.imread(path)
        detection_result = detection_method(accurate_detector, image_numpy, "accurate.jpg")
        self.send_response(200, "OK")
        if return_type == "json":
            self.wfile.write(bytes(detection_result, "utf-8"))
        else:
            print(detection_result)
            img = Image.fromarray(detection_result[0], 'RGB')
            self.wfile.write(bytes(json.dumps(detection_result[0], cls=NumpyEncoder), "utf-8"))
            #self.send_head()
            #self.send_header("Content-Type", "image/jpeg")
            #self.end_headers()
            #self.wfile.write(bytes(image_to_byte_array(img)))
        self.end_headers()
if __name__ == "__main__":
    server = HTTPServer((HOST_NAME, PORT), PythonServer)
    print(f"Server started http://{HOST_NAME}:{PORT}")
    try:
        server.serve_forever()
    except KeyboardInterrupt:
        server.server_close()
        print("Server stopped successfully")
        sys.exit(0)