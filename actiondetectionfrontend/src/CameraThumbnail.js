import { React, Component } from "react";
import globalCameraAPI from "./services/globals"

export default class CameraThumbnail extends Component {

    constructor(props) {
        super(props);

        this.state = {
            name: props.name,
            source: `${props.name}/vga.jpg`,
            detectedObjects: []
        };

    }

    objectDetectionRunning = false;

    componentDidMount() {
        setInterval((() => {
            this.setState({ source: `${this.state.name}/vga.jpg?ts=${Date.now()}` });

            //this.setState({ source: `http://localhost:5219/Camera/GetDetectionImage?cameraIP=${this.state.name.replace('http://', '')}&imageSize=2&ts=${Date.now()}` });
        }).bind(this), 1000);
        return;
        setInterval((() => {
            if (!this.objectDetectionRunning) {
                this.objectDetectionRunning = true;
                globalCameraAPI.cameraDetectObjectsInCameraViewGet({ cameraIP: this.state.name.replace('http://', ''), imageSize: 2 }, (err, data, res) => {
                    //console.log(data);
                    this.setState({ detectedObjects: data.detectedObjects });
                    this.objectDetectionRunning = false;
                });
            }
        }).bind(this), 1000);
    }

    componentDidUpdate(state, props) {
        if (this.state == null)
            return;

        if (this.props.name !== props.name) {
            this.setState({ name: this.props });
        }
    }

    render() {
        return <>
            <div className={"img-container"}>
                <img className="img-fluid"
                    width={400}
                    src={this.state.source}
                    alt={this.state.name} />
                <div className={"top-left"}>
                    {this.state.name}
                    <p className={'bottom-left'}>
                        {[...new Set(this.state.detectedObjects.map(x => x.name))].join('\n')}
                    </p>
                </div>

            </div>
        </>;
    }
}