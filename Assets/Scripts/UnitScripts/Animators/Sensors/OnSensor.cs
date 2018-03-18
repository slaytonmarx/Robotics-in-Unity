using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSensor : Sensor {

    public override void buildSensor(Controller controllerInput) {
        base.buildSensor(controllerInput);
    }
    public override float parseValue(float input) {
        return value;
    }
    protected override void Update() {
        base.Update();
    }
}
