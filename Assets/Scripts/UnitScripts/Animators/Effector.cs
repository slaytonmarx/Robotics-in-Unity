using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effector : SensorEffector {
    /* Effector ----- Parent class of all effectors. Effectors take a value from a neuron and effect some change
     * in the robots body from that value. Note: Effectors parse value must be limited between a minimum and 
     * maxium value, changing their output accordingly. */

    public virtual void buildEffector(Controller controllerInput) {
        base.buildSensorEffector(controllerInput);
    }
    public virtual void actuate() {
        /* actuate ----- Effects the behavior of the associated effector. */
    }
    protected virtual void Update() {
        actuate();
    }
}
