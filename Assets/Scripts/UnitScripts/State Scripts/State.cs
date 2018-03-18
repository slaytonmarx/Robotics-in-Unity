using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class State : MonoBehaviour {
    public Unit unit;
    public bool initialState;
    public bool processed;
    public NeuralNetwork network;
    public Fitness fitness;
    public Transition[] transitions;

    public void buildState(Unit unitInput) {
        unit = unitInput;
        try {
            fitness = gameObject.GetComponent<Fitness>();
            transitions = gameObject.GetComponents<Transition>();
            foreach (var transition in transitions) {
                transition.buildTransition();
            }
            if (network.controllerLoadPath != "")
                processed = true;
        }
        catch(NullReferenceException) {
            Debug.Log("State buildState failure, " + name + " has no attached neural net.");
        }
    }
    
    public void checkTransition() {
        foreach (var transition in transitions) {
            transition.checkTransition();
        }
    }

    public NeuralNetwork getNetwork() {
        return network;
    }

}
