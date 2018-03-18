using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionSensor : Sensor {

    // PROPERTIES //
    public GameObject affectionObject;
    public string goalObject;
    
    // Core Methods
    public override void buildSensor(Controller controllerInput) {
        base.buildSensor(controllerInput);
        foreach (GameObject obj in rootController.rootUnit.unitObjects) {
            if (obj.tag == goalObject)
                affectionObject = obj;
        }
        if (affectionObject == null) {
            Debug.Log("No Goal found");
        }
    }
    public override float parseValue(float input) {
        value = findDifferenceInDegrees() / 180;
        return value;
    }
    protected override void Update() {
        base.Update();
    }

    // Unique Methods
    private float findDifferenceInDegrees() {
        Vector3 vObj = gameObject.transform.forward;
        Vector3 vAff = (affectionObject.transform.position - gameObject.transform.position);
        float angle = Vector3.Angle(vAff, vObj);
        Vector3 cross = Vector3.Cross(vAff, vObj);
        if (cross.y > 0)
            return -angle;
        return angle;
    }
}