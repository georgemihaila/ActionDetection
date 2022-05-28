import { React, Component } from "react";
import {globalCameraAPI} from "./services/globals"
import CameraThumbnail from "./CameraThumbnail";
import FloatingSettingsBar from "./FloatingSettingsBar";
export default class CameraList extends Component {
    constructor(props) {
        super(props);

        this.state = {
            cameras: [],
            motionDetectionChunks: 34
        };
    }

    componentDidMount() {
        globalCameraAPI.cameraListGet((err, data, res) => {
            if (data !== null) {
                this.setState({ cameras: data })
            }
        });
    }

    motionDetectionSliderChanged(v){
        this.setState({motionDetectionChunks: v});
    }

    render() {
        return <>
            {(this.state?.cameras?.length == 0) ? "No cameras online" : this.state?.cameras?.map(x => <CameraThumbnail motionDetectionChunks={this.state.motionDetectionChunks} key={x} name={x} />)}
            <FloatingSettingsBar onMotionDetectionSliderChanged={this.motionDetectionSliderChanged.bind(this)}/>
        </>;
    }
}