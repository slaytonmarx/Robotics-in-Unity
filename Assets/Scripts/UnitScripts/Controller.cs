using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine;

/* CONTROLLER
 * The controller contains the robots neural net and communicates with the director. It runs the net,
 * taking in information from the robots sensors and transfering that information to the robot's effectors.
 */ 

public class Controller : MonoBehaviour {

    // PROPERTIES //
    public bool showcaseMode;
    public float mutationChance;
    public float mutationMaximum;
    public float mutationMinimum;
    public bool hiddenNeurons;
    public int maximumHiddenNeurons;
    public bool synapsePruning;
    public float synapsePruningChance;
    public bool neuronPruning;
    public float neuronPruningChance;
    public float effectorMultiplier;

    // External Values
    public Unit rootUnit;
    public Anatomy anatomy;
    // Important Properties
    public Sensor[] sensors;
    public Effector[] effectors;
    // Neural Nets
    private NeuralNetwork[] networks;
    
    public void buildController(Unit unitInput) {
        /* initializeController ----- Initializes the controller, taking special input in the form
         * of keyword strings which are then fed into build net, and to the various sensors and 
         * effectors to ensure they function as desired. */
        rootUnit = unitInput;
        anatomy = rootUnit.robot;
        sensors = anatomy.gameObject.GetComponentsInChildren<Sensor>();
        effectors = anatomy.gameObject.GetComponentsInChildren<Effector>();
        foreach (Sensor sen in sensors) {
            sen.buildSensor(this);
        }
        foreach (Effector eff in effectors) {
            eff.buildEffector(this);
        }
        initializeNeuralNets();
    }
    // Helper Methods
    public void initializeNeuralNets() {
        networks = gameObject.GetComponentsInChildren<NeuralNetwork>();
        foreach (var net in networks) {
            net.buildNeuralNetwork(this);
        }
    }
    public void mutateNet() {
        rootUnit.activeNetwork.mutateNet();
    }
    public string transcribeController() {
        return rootUnit.activeNetwork.transcribeController();
    }
    public int loadNet(string netLoadName) {
        string neuralNet = Scrivener.readController("ProjectData\\" + netLoadName);
        //nets.Add(new NeuralNetwork(this, neuralNet));
        return networks.Length - 1; // The index of the new net
    }
    public string getActiveNetName() {
        return rootUnit.activeNetwork.name;
    }
    public float getActiveNetImportance() {
        return 1.1f;// THREAD activeNet.subnetImportance;
    }
    public void setNetImportance(int netIndex, float importanceInput) {
        //nets[netIndex].subnetImportance = importanceInput;
    }
    void Update() {
        rootUnit.activeNetwork.fireNet();
    }
    public void initializeDisplay() {
        //netDisplay = new NetDisplay(ref topNet, rootRobot.rootUnit.rootModule.rootDirector);
    }
    public void resetDisplay() {
        //Destroy(netDisplay.display);
        //netDisplay = new NetDisplay(ref topNet, rootRobot.rootUnit.rootModule.rootDirector);
    }
    public void showDisplay(bool mask) {
        //netDisplay.unmaskDisplay(mask);
    }
    public void updateDisplay() {
        //netDisplay.update();
    }
}
