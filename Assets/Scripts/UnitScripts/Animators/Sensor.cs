using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : SensorEffector {
    /* Sensor ----- Parent class of both all sensors, though techniqually unnecessary, needed for symantic
     * clarity (we couldn't have all sensors inheriting from "SensorEffector" after all. Sensors sense something
     * and pass that perception to an attached sensor neuron. The range of sensing must be between -1 and 1,
     * a conversion operation taking place in the parseValue function inherited from SensorEffector. */

    public virtual void buildSensor(Controller controllerInput) {
        base.buildSensorEffector(controllerInput);
    }
    public virtual void checkSwitchPosition() {

    }
    protected virtual void Update() {
        parseValue(0);
    }
}
