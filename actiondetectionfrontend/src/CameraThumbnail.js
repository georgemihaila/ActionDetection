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
            <img className="img-fluid"
                src={this.state.source}
                alt={this.state.name} />
        </>;
    }
}