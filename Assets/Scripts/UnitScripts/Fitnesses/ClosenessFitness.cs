using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosenessFitness : Fitness {

    public GameObject focus;
    public GameObject affection;
    public string focusTag;
    public string goalTag;

    public override void buildFitness(Unit unitInput, List<GameObject> unitObjectsInput) {
        base.buildFitness(unitInput, unitObjectsInput);
        foreach (GameObject obj in unitObjectsInput) {
            if (obj.tag == focusTag) {
                focus = obj;
            }
            if (obj.tag == goalTag) {
                affection = obj;
            }
        }
    }
    public override void updateFitness() {
        fitness = -Mathf.Abs(focus.transform.position.x - affection.transform.position.x) - Mathf.Abs(focus.transform.position.z - affection.transform.position.z);
    }
    public override float getFitness() {
        return fitness;
    }
}
