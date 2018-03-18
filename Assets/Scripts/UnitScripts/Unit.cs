using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Unit : MonoBehaviour {
    
    // Identifying Properties
    public Module rootModule;
    public string unitName;
    public int unitId;
    public bool lockState;

    // Fixed Properties
    public List<GameObject> unitObjects;
    public Controller controller;
    public Anatomy robot;
    public Stage stage;

    // Unfixed Properties
    public State activeState;
    public NeuralNetwork activeNetwork;
    public Fitness activeFitness;
    public string controllerString;

    // Object Properties
    public Vector3 unitPosition;

    // Temporary Properties
    public bool transitionFlag;

    // Robot Properties

    // INITIALIZER METHODS //

    // Core Methods
    public virtual void buildUnit(Module moduleInput, int idInput, Vector3 rootPositionInput, bool lockStateInput) {
        rootModule = moduleInput;
        unitId = idInput;
        unitName = gameObject.name + " " + unitId.ToString();
        unitPosition = rootPositionInput;
        lockState = lockStateInput;
        transitionFlag = false;

        initializeComponents(idInput);
    }
    // Helper Methods
    private void initializeComponents(int idInput) {
        // Initialize Important Values
        unitObjects = new List<GameObject>();
        populateUnitObjects();
        initializeStage(idInput);
        initializeAnatomy(idInput);
        initializeController();
        initializeFitness();
        initializeStates();
    }
    private void populateUnitObjects() {
        /* populateUnitObjects ----- Adds all child objects from both the unit's environment and robot
         * to it's list of objects. Does not add the environment or robot itself. */
        Rigidbody[] rbs = gameObject.GetComponentsInChildren<Rigidbody>();
        for (int i = 0; i < rbs.Length; i++) {
            unitObjects.Add(rbs[i].gameObject);
        }
    }
    private void initializeStage(int idInput) {
        try {
            stage = gameObject.GetComponentInChildren<Stage>();
            stage.buildStage(this, "stage", idInput, unitPosition);
        }
        catch (NullReferenceException) {
            Debug.Log("Unit initializeStage failure, missing Stage component in children");
        }
    } 
    private void initializeAnatomy(int idInput) {
        try {
            robot = gameObject.GetComponentInChildren<Anatomy>();
            robot.buildAnatomy(this, "robot", idInput);
        }
        catch (NullReferenceException) {
            Debug.Log("Unit initializeAnatomy failure, missing Anatomy component in children");
        }
    }
    private void initializeController() {
        try {
            controller = gameObject.GetComponentInChildren<Controller>();
            controller.buildController(this);
        }
        catch (NullReferenceException) {
            Debug.Log("Unit initializeController failure, missing controller component in children");
        }
    }
    private void initializeFitness() {
        if (activeFitness == null) {
            Fitness[] fitnesses = gameObject.GetComponentsInChildren<Fitness>();
            if (fitnesses.Length == 0) {
                Debug.Log("Unit initializeFitness failure, no fitnesses in children");
            }
            try {
                foreach (var fitness in fitnesses) {
                    fitness.buildFitness(this, unitObjects);
                }
            }
            catch (NullReferenceException) {
                Debug.Log("Unit initializeFitness failure, fitness references null object");
            }
        }
    }
    private void initializeStates() {
        if (activeState == null) {
            State[] states = gameObject.GetComponentsInChildren<State>();
            if (states.Length == 0) {
                Debug.Log("Unit initializeStates failure, no states in children");
            }
            try {
                foreach (var state in states) {
                    state.buildState(this);
                    if (state.initialState) {
                        mantleState(state);
                    }
                }
            }
            catch (NullReferenceException) {
                Debug.Log("Unit initializeStates failure, active state missing fitness or network");
            }
            if (activeState == null)
                Debug.Log("Unit initializeStates failure, no state set to initial state");
        }
    }

    public void mantleState(State stateInput) {
        activeState = stateInput;
        activeNetwork = stateInput.network;
        activeFitness = stateInput.fitness;
    }

    // ACTIVE METHODS //
    public void resetUnit() {
        robot.reset();
        stage.reset();
        activeFitness.reset();
    }
    public void stopVelocity() {
        foreach (var obj in unitObjects) {
            obj.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }
    }

    private void Update() {
        activeFitness.updateFitness();
        activeState.checkTransition();
    }

    public void updateConfigurations() {
        robot.updateConfigurations();
        stage.updateConfigurations();
    }
    public void inscribeControllerIntoUnit(string controllerInput) {
        activeNetwork.inscribeController(controllerInput);
    }

    // GETTER METHODS //
    public string getNetwork() {
        return controller.transcribeController();
    }
}