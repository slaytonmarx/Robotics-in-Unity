using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fitness : MonoBehaviour {
// PROPERTIES //
    public float fitness;
    protected Unit rootUnit;
    protected List<GameObject> unitObjects;
    public virtual void buildFitness(Unit unitInput, List<GameObject> unitObjectsInput) {
        rootUnit = unitInput;
        unitObjects = unitObjectsInput;
    }
    public virtual float getFitness() {
        return 0;
    }
    public virtual void updateFitness() {

    }
    public virtual void reset() {
        fitness = 0;
    }
}
