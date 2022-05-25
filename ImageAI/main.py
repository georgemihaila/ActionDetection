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

def doDetection(detector, inFile, outFile):
    import time
    start_time = time.time()
    detections = detector.detectObjectsFromImage(input_image=os.path.join(execution_path , inFile), output_image_path=os.path.join(execution_path , outFile), minimum_percentage_probability=30)
    print("--- %s seconds ---" % (time.time() - start_time))
    for eachObject in detections:
        print(eachObject["name"] , " : ", eachObject["percentage_probability"], " : ", eachObject["box_points"] )
        print("--------------------------------")

doDetection(accurate_detector, "uxga.jpg", "accurate.jpg")
doDetection(fast_detector, "uxga.jpg", "fast.jpg")