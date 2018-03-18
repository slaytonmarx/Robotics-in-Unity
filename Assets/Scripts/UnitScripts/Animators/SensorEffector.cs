using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorEffector : MonoBehaviour {
    /* SensorEffector ----- Parent class of both sensors and effectors, and thus grandparent class of all sensors
     * and effectors placed on robots. Attached to an embodied gameobject which will either sense or effect. */

    // PROPERTIES //

    // Identifying Properties
    protected Controller rootController; // Anatomy the effector belongs to
    public string netTag;
    public string connectionTag;
    public float value; // Value changed by connected neuron
    public string switchboxKey;

    // CORE METHODS //
    public void buildSensorEffector(Controller controllerInput) {
        rootController = controllerInput;
    }
    public virtual float parseValue(float input) {
        /* parseValue ----- In sensors parseValue is used to "check up" on some value which the sensor is sensing
         * for, such as distance traveled or other relevant thing. In Effectors it is used to change the value
         * passed to it by the neuron (always between -1 and 1 to a value suitable to the thing it's effecting. */
        value = input;
        return 0;
    }

}
