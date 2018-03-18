using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubnetEffector : Effector {

    // PROPERTIES //
    public bool subnetActive;
    public string netLoadName;
    public int subnetIndex;
    
    // Core Methods
    public override void buildEffector(Controller controllerInput) {
        base.buildEffector(controllerInput);
        subnetIndex = rootController.loadNet(netLoadName);
    }
    public override void actuate() {
        Controller c = rootController;
        c.setNetImportance(subnetIndex, value);
        //if (c.getActiveNetImportance() <= value) {
        //    c.setActiveNet(subnetIndex);
        //}
        //else
        //    c.setNetImportance(subnetIndex, -1);
    }
    protected override void Update() {
        base.Update();
    }
}
