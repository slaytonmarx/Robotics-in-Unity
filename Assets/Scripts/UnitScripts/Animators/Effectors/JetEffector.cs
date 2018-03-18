using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetEffector : Effector {

    // PROPERTIES //
    public bool forwardThrust;
    private float jetMultiplier;
    private Vector3 JetForce;
    private Rigidbody rb;
    public int xMultiplier;
    public int zMultiplier;
    
    // Core Methods
    public override void buildEffector(Controller controllerInput) {
        base.buildEffector(controllerInput);
        rb = gameObject.GetComponent<Rigidbody>();
        jetMultiplier = rootController.effectorMultiplier;
        JetForce = new Vector3(0, 0, 0);
    }
    public override float parseValue(float input) {
        value = input * jetMultiplier;
        Vector3 thrust;
        if (!forwardThrust)
            thrust = new Vector3(xMultiplier * value, 0, zMultiplier * value);
        else
            thrust = gameObject.transform.forward * jetMultiplier;
        JetForce = thrust;
        return 0;
    }
    public override void actuate() {
        rb.velocity = new Vector3();
        rb.AddForce(JetForce);
    }
    protected override void Update() {
        base.Update();
    }
}
