using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointEffector : Effector {

    // PROPERTIES //
    HingeJoint joint;
    public float jointMultiplier;

    // Core Methods
    public override void buildEffector(Controller controllerInput) {
        base.buildEffector(controllerInput);
        joint = gameObject.GetComponent<HingeJoint>();
        jointMultiplier = rootController.effectorMultiplier;
        joint.useMotor = true;
    }
    public override float parseValue(float input) {
        value = input * jointMultiplier;
        return 0;
    }
    public override void actuate() {
        JointMotor j = joint.motor;
        j.targetVelocity = value;
        joint.motor = j;
    }
    protected override void Update() {
        base.Update();
    }
}
