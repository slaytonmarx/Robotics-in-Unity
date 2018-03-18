using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPGSensor : Sensor {

    // PROPERTIES //
    private Director d;
    private float offsetInRadiens;
    public int cpgInterval;
    public int offsetInDegrees;
    public int cpgCycle;

    // Core Methods
    public override void buildSensor(Controller controllerInput) {
        base.buildSensor(controllerInput);
        d = rootController.rootUnit.rootModule.rootDirector;
        offsetInRadiens = offsetInDegrees * Mathf.PI / 180;
    }
    public override float parseValue(float input) {
        if(d.time % cpgInterval == 0) {
            float temp = d.time % cpgCycle;
            value = Mathf.Sin((temp / cpgCycle) * 2 * Mathf.PI + offsetInRadiens);
        }
        return value;
    }
    protected override void Update() {
        base.Update();
    }
}
