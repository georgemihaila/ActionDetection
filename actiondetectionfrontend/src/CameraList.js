import { React, Component } from "react";
import globalCameraAPI from "./services/globals"
import CameraThumbnail from "./CameraThumbnail";

export default class CameraList extends Component {

    constructor(props) {
        super(props);

        this.state = {
            cameras: []
        };
    }

    componentDidMount() {
        globalCameraAPI.cameraListGet((err, data, res) => {
            if (data !== null) {
                this.setState({ cameras: data })
            }
        });
    }

    render() {
        return <>
            {(this.state?.cameras?.length == 0) ? "No cameras online" : this.state?.cameras?.map(x => <CameraThumbnail key={x} name={x} />)}
        </>;
    }
}