using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnEffector : Effector {
    Vector3 rotationForce;
    float multiplier;
    Rigidbody rb;
    // Use this for initialization
    public override void buildEffector(Controller controllerInput) {
        base.buildEffector(controllerInput);
        multiplier = rootController.effectorMultiplier;
        rb = gameObject.GetComponent<Rigidbody>();
    }
    public override float parseValue(float input) {
        value = input * multiplier;
        rotationForce = new Vector3(0, value, 0);
        return base.parseValue(input);
    }
    public override void actuate() {
        rb.AddTorque(rotationForce);
    }
    protected override void Update() {
        base.Update();
    }
}
