using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosenessTouchFitness : Fitness {

    public GameObject focus;
    public GameObject affection;
    public string focusTag;
    public string goalTag;
    public bool isTouching;

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
        if (isTouching)
            fitness += 50;
    }
    public override float getFitness() {
        return fitness;
    }
    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == goalTag) {
            isTouching = true;
        }
    }
    private void OnCollisionExit(Collision collision) {
        if (collision.gameObject.tag == goalTag) {
            isTouching = false;
        }
    }
}
