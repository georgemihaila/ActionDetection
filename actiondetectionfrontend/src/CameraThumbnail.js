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

    componentDidMount() {
        setInterval((() => {
            this.setState({ source: `${this.state.name}/vga.jpg?ts=${Date.now()}` });
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
            </div>
        </div>
        </>;
    }
}