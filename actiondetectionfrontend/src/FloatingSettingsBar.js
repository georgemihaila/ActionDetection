import { React, Component } from "react";
import { globalCameraAPI, apiAddress } from "./services/globals"
import { Slider } from "@mui/material";
import { Typography } from "@mui/material";
import { FormGroup, FormControlLabel, Checkbox } from "@mui/material";

export default class FloatingSettingsBar extends Component {

    constructor(props) {
        super(props);

        this.state = {

        };

    }

    componentDidMount() {

    }

    componentDidUpdate(state, props) {
        if (this.state == null)
            return;

        if (this.props.name !== props.name) {
            this.setState({ name: this.props });
        }
    }

    motionDetectionSliderChanged(e, v) {
        if (Object.keys(this.props).includes("onMotionDetectionSliderChanged")) {
            this.props.onMotionDetectionSliderChanged(v);
        }
    }

    render() {
        return <>
            <div className={"float-bottom"}>
                <Typography id="input-slider" gutterBottom>
                    Chunk size
                </Typography>
                <Slider
                    aria-label="Motion chunks"
                    defaultValue={34}
                    getAriaValueText={this.valuetext}
                    valueLabelDisplay="auto"
                    step={10}
                    marks
                    min={4}
                    max={64}
                    width={100}
                    onChange={this.motionDetectionSliderChanged.bind(this)}
                />
                <FormGroup>
                    <FormControlLabel control={<Checkbox defaultChecked />} label="Motion detection" />
                </FormGroup>
            </div>
        </>;
    }
}