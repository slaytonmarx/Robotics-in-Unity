using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceSensor : Sensor {

    // PROPERTIES //
    public GameObject affectionObject;
    public int xMultiplier;
    public int yMultiplier;
    public int zMultiplier;

    // Core Methods
    public override void buildSensor(Controller controllerInput) {
        base.buildSensor(controllerInput);
        foreach (GameObject obj in rootController.rootUnit.stage.components) {
            if (obj.tag == "Goal") {
                affectionObject = obj;
            }
        }
        if (affectionObject == null)
            Debug.Log("No Object in Unit had Tag 'Goal'");
    }
    public override float parseValue(float input) {
        value = xMultiplier * judgeDistance("x") / 20 + yMultiplier * judgeDistance("y") / 20 + zMultiplier * judgeDistance("z") / 20;
        return value;
    }
    protected override void Update() {
        base.Update();
    }

    // Unique Methods
    private float judgeDistance(string direction) {
        if (direction == "x")
            return affectionObject.transform.position.x - gameObject.transform.position.x;
        if (direction == "y")
            return affectionObject.transform.position.y - gameObject.transform.position.y;
        if (direction == "z")
            return affectionObject.transform.position.z - gameObject.transform.position.z;
        return 0;
    }
}
