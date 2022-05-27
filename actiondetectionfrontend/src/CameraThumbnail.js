import { React, Component } from "react";
import { globalCameraAPI, apiAddress } from "./services/globals"

export default class CameraThumbnail extends Component {

    constructor(props) {
        super(props);
        let messageAlts = {
            initial: props.name,
            connecting: "Connecting to " + props.name + "...",
            waitingForFrame: "Waiting for data from " + props.name + "..."
        };
        this.state = {
            name: props.name,
            wsSource: `${apiAddress.replace("http://", "ws://")}/Camera/FrameSubscription?cameraIP=${props.name.replace('http://', '')}&imageSize=0`,
            motionDetectionChunks: props.motionDetectionChunks,
            messageAlts: messageAlts,
            alt: messageAlts.initial,
            frameData: []
        };

    }

    sendClientUp(){
        this.ws.send(JSON.stringify({ data: 'client up' }));
    }

    reopenWS() {
        this.setState({ alt: this.state.messageAlts.connecting });
        this.ws = new WebSocket(this.state.wsSource);
        this.ws.onopen = this.handleWSOpened.bind(this);
        this.ws.onmessage = this.handleWSMessage.bind(this);
        this.ws.onclose = this.handleWSClosed.bind(this);
    }

    handleWSOpened(e) {
        //console.log(this.state.name + ' ws opened');
        if (this.alt !== this.state.messageAlts.waitingForFrame) {
            this.setState({ alt: this.state.messageAlts.waitingForFrame });
            this.sendClientUp();
        }
    }

    handleWSMessage(e) {
        //console.log(e.data);
        this.setState({frameData: e.data});
        this.sendClientUp();
    }

    handleWSClosed(e) {
        //console.log(this.state.name + ' ws closed');
        this.reopenWS();
    }

    ws = {};
    componentDidMount() {
        this.reopenWS();
    }

    componentDidUpdate(state, props) {
        if (this.state == null)
            return;

        if (this.props.name !== props.name) {
            this.setState({ name: this.props.name });
        }
        if (this.state.motionDetectionChunks !== this.props.motionDetectionChunks) {
            this.setState({ motionDetectionChunks: this.props.motionDetectionChunks });
        }
    }

    render() {
        let ip = <div className={"top-left"}>
            {this.state.name}
        </div>;
        return <>
            <div className={"img-container"}>
                <img className="img-fluid"
                    width={400}
                    src={`data:image/jpeg;base64,${this.state.frameData}`}
                    alt={this.state.alt} />
            </div>
        </>;
    }
}