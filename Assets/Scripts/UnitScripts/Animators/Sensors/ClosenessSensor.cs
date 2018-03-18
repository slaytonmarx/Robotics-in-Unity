using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosenessSensor : Sensor {

    // PROPERTIES //
    public GameObject goal;
    public float distanceFloat;

    // Core Methods
    public override void buildSensor(Controller controllerInput) {
        base.buildSensor(controllerInput);
        foreach (GameObject obj in rootController.rootUnit.unitObjects) {
            if (obj.tag == "Goal")
                goal = obj;
        }
    }
    public override float parseValue(float input) {
        value = distanceFloat - (goal.transform.position.magnitude - gameObject.transform.position.magnitude);
        return value;
    }
    protected override void Update() {
        base.Update();
    }
}
