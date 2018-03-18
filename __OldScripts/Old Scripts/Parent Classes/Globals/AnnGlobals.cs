using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnGlobals : MonoBehaviour {

    /* AnnGlobals ----- this class's entire purpose is to store the various global variables
     * that need to be called by the controller's artifical neural network, these include
     * variables responsible for the neural growth, evolution, the random seed, ect.
     */
    
    public int seed;
    public string path;
    public int maxHiddenNeurons;
    public int maxSynapses;
    public int growthCycle;
    public float synapseRandomizeValue;
    public float chanceOfSynapsePerturbation;
    public float synapsePruneThreshold;
    public float neuronPruneThreshold;
    public float chanceOfSynapsePruning;
    public float chanceOfNewSynapse;
    public float chanceOfNewNeuron;
    public float chanceOfNeuronPruning;
    public float synapseMaxPerturbation;
    public float synapseMinPerturbation;
    public float synRandom;
}
