using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelFitness : Fitness {

    public string direction; // can be +x, -x, +z, or -z
    GameObject focus;
    Vector3 initialPosition;
    bool hasFallen;
    bool resetWatch;
    List<TouchSensor> fallSensors;

    public override void buildFitness(Unit unitInput, List<GameObject> unitObjectsInput) {
        base.buildFitness(unitInput, unitObjectsInput);
        initialPosition = rootUnit.stage.position;
        fallSensors = new List<TouchSensor>();
        string directionInput = rootUnit.rootModule.rootDirector.bart.experimentParameters;
        switch (directionInput) {
            case ("-x"): direction = directionInput; break;
            case ("+z"): direction = directionInput; break;
            case ("-z"): direction = directionInput; break;
            default:
                direction = "+x";
                break;
        }

        foreach (GameObject obj in unitObjectsInput) {
            if (obj.tag == "Focus") {
                focus = obj;
            }
        }

        foreach (GameObject obj in unitObjectsInput) {
            if (obj.tag == "FallSensor") {
                fallSensors.Add(obj.GetComponent<TouchSensor>());
            }
        }
    }
    public override void updateFitness() {
        float fallen = 0;
        switch (direction) {
            case ("+x"): fitness = focus.transform.position.x - initialPosition.x - Mathf.Abs(focus.transform.position.z - initialPosition.z) - (fallen); // +x set to default
                break;
            case ("-x"): fitness = -1 * (focus.transform.position.x - initialPosition.x) - Mathf.Abs(focus.transform.position.z - initialPosition.z);
                break;
            case ("+z"): fitness = focus.transform.position.z - initialPosition.z - Mathf.Abs(focus.transform.position.x - initialPosition.x);
                break;
            case ("-z"): fitness = -1 * (focus.transform.position.z - initialPosition.z) - Mathf.Abs(focus.transform.position.x - initialPosition.x);
                break;
            default:
                break;
        }
        foreach (var item in fallSensors) {
            if (item.isTouching & resetWatch == false) {
                hasFallen = true;
            }
        }
        resetWatch = false;
        if (hasFallen)
            fallen = 100;

    }
    public override float getFitness() {
        return fitness;
    }
    public override void reset() {
        hasFallen = false;
        resetWatch = true;
    }
}
