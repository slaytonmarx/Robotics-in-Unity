using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Transition : MonoBehaviour {
    public GameObject body;
    public bool fullTransition;
    public State state;
    public State nextState;
    public SensorEffector animator;
    public string switchboxKey;
    public float valueThreshold;

    public void buildTransition() {
        state = gameObject.GetComponent<State>();
        try {
            SensorEffector[] anArray = body.GetComponents<SensorEffector>();
            foreach (var anim in anArray) {
                if (anim.switchboxKey == switchboxKey) {
                    animator = anim;
                }
            }
        }
        catch (NullReferenceException) {
            Debug.Log("Transition buildTransition Failure, likely that the robot body wasn't attached.");
        }
    }
    public void checkTransition() {
        if (animator.value == valueThreshold) {
            state.unit.transitionFlag = true;
            if (!state.unit.lockState) {
                Debug.Log(state.unit.gameObject.name);
                state.unit.stopVelocity();
                state.unit.mantleState(nextState);
            }
        }
    }

    // Use this for initialization
    void Start () {
		
	}
}