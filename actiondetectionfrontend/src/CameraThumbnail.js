import { React, Component } from "react";
import globalCameraAPI from "./services/globals"

export default class CameraThumbnail extends Component {

    constructor(props) {
        super(props);

        this.state = {
            name: props.name,
            source: `${props.name}/vga.jpg`
        };

    }

    requestRunning = false;

    componentDidMount() {
        //setInterval((() => {
            if (!this.requestRunning) {
                //this.setState({ source: `${this.state.name}/vga.jpg?ts=${Date.now()}` });
                //http://localhost:5219/Camera/GetDetectionImage?cameraIP=10.10.0.138&imageSize=2
                this.setState({ source: `http://localhost:5219/Camera/GetDetectionImage?cameraIP=${this.state.name.replace('http://', '')}&imageSize=2&ts=${Date.now()}` });
            }
        //}).bind(this), 1000);
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
                </div>
            </div>
        </>;
    }
}