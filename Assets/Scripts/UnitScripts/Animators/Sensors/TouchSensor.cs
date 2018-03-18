using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchSensor : Sensor {

    // PROPERTIES //
    public bool triggerTouch;
    public bool isTouching;
    public string touchTag;

    // Core Methods
    public override void buildSensor(Controller controllerInput) {
        base.buildSensor(controllerInput);
    }
    public override float parseValue(float input) {
        if (isTouching) {
            value = 1;
            return value;
        }
        else {
            value = 0;
            return value;
        }
        return value;
    }
    protected override void Update() {
        base.Update();
    }

    // Unique Methods
    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == touchTag) {
            isTouching = true;
        }
    }
    private void OnCollisionExit(Collision collision) {
        if (collision.gameObject.tag == touchTag) {
            isTouching = false;
        }
    }
    void OnTriggerEnter(Collider collision) {
        if (triggerTouch) {
            if (collision.gameObject.tag == touchTag) {
                isTouching = true;
            }
        }
    }
    private void OnTriggerExit(Collider collision) {
        if (triggerTouch) {
            if (collision.gameObject.tag == touchTag) {
                isTouching = false;
            }
        }
    }
}
