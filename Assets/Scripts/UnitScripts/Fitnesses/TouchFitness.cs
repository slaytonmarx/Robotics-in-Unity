using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchFitness : Fitness {
    bool hasTouched;
    bool resetWatch;
    TouchSensor ts;
    GameObject goal;
    GameObject focus;
    public override void buildFitness(Unit unitInput, List<GameObject> unitObjectsInput) {
        base.buildFitness(unitInput, unitObjectsInput);
        foreach (GameObject obj in unitObjects) {
            if (obj.tag == "Goal")
                goal = obj;
            if (obj.tag == "Focus") {
                focus = obj;
                ts = obj.GetComponent<TouchSensor>();
            }
        }
    }
    public override void updateFitness() {
        float touched = 0;
        if (ts.isTouching) {
            touched = 10;
        }
        fitness = 10 - (goal.transform.position.magnitude - focus.transform.position.magnitude) + touched;
    }
    public override float getFitness() {
        return fitness;
    }
    public override void reset() {
        base.reset();
        resetWatch = true;
    }
}
