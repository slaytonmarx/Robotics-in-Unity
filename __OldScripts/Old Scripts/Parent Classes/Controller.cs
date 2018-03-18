using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

    /* Controller ----- [Parent Class]
     * The Controller class is one of the most vital to the robot, acting as the robots
     * "brain." It is composed of two parts, the ArtificialNeuralNet (ann), and the RobotAnimator (animator).
     * The ann contains a virtual neural net comprised of neurons, this is where the computing and evolution
     * of the robot takes place. The animator is the controllers relay with the rest of it's body, sort of like
     * the spinal cord of a human.
     * 
     * No matter how extensive and recursive the net is, and no matter how man subnets, the ann member of the
     * Controller is the first net to be called, and the root of all the rest.
     * 
     * * The Neurons are arranged into an array of 3 lists:
         * 
         * ----- 0 = Input Layer
         * ----- 1 = Hidden Layer
     * * * ----- 2 = Output Layer
     */

    // Root
    public Module.Unit rootUnit;

    // Informational Components
    private int id;
    public System.Random controllerRandom;
    
    private AnnGlobals ag;
    private List<Neuron>[] neurons;
    private List<Synapse> synapses;
    public bool isActive;
    public Dictionary<string, List<Relay>> relayCatalog;
    private char textBreak = '/';

    public void Awake()
    {
        isActive = false;
        rootUnit = gameObject.GetComponentInParent<Module.Unit>();
        ag = rootUnit.rootModule.ag;
        neurons = new List<Neuron>[3];
        for (int i = 0; i < 3; i++)
        {
            neurons[i] = new List<Neuron>();
        }
        synapses = new List<Synapse>();
        relayCatalog = new Dictionary<string, List<Relay>>();
    }

    public void initializeController(int idInput, int seed)
    {
        // The Hook
        id = idInput;
        controllerRandom = new System.Random(seed);
    }

    public void intitializeController()
    {
    }

    //====== ESSENTIAL FUNCTIONS ====== //

    public void evolveNet()
    {
        // evolveNet ----- fundemental to evolutionary robotics, the evolve net function
        // does what it says on the tin. There's a chance hidden neurons will be added,
        // there's a chance they'll be pruned, synapses may be added or pruned or perturbed. 

        for (int i = 0; i < ag.growthCycle; i++)
        {
            pruning(neurons, synapses);
            growth(neurons);
            perturbation(synapses);
        }
    }
    public void cloneNet(Controller netToBeChanged)
    {
        // cloneNet ----- cloneNet takes in a net and coppies the contents of the net that
        // calls it into that net.
        netToBeChanged.resetIDs();
        resetIDs();

        netToBeChanged.synapses.Clear();
        netToBeChanged.neurons[1].Clear();
        foreach (Neuron neuron in neurons[1])
        {
            //netToBeChanged.neurons[1].Add(neuron.cloneHiddenNeuron());
        }

        List<Neuron> masterList = new List<Neuron>();
        for (int i = 0; i < 3; i++)
        {
            masterList.AddRange(netToBeChanged.neurons[i]);
        }
        foreach (Synapse synapse in synapses)
        {
            netToBeChanged.synapses.Add(synapse.cloneSynapse(masterList, synapse.parent.id, synapse.child.id));
        }
    } // Must improve this one when write and read are done
    public string packageNet()
    {
        // packageNet ----- packageNet takes the neural net and writes it to a string,
        // it is very important for a number of functions and the cloning and read/write
        // process.
        string composition = "";

        composition += "NEURONS" + textBreak;

        List<Neuron> masterList = new List<Neuron>();
        for (int i = 0; i < 3; i++)
        {
            masterList.AddRange(neurons[i]);
        }
        foreach (Neuron neuron in masterList)
        {
            composition += neuron.compose() + textBreak;
        }

        composition += "END NEURONS" + textBreak;

        composition += "SYNAPSES" + textBreak;

        foreach (Synapse synapse in synapses)
        {
            composition += synapse.compose() + textBreak;
        }

        composition += "END SYNAPSES" + textBreak;

        return composition;
    }
    public void unpackageNet(string composition)
    {
        // unpackageNet ----- takes a string and unpackages it, converting the contents
        // of the one which unpacks it to it's own. I turn composition to a list instead
        // of an array because I use some recursion here and I feel it's more elegant to
        // remove the first entry of the list as we go through it. It's not more efficient
        // but it's just so cool.
        clearNet();
        List<string> compositionList = new List<string>(composition.Split(textBreak));

        // Moves us up to the neuron list
        while (compositionList[0] != "NEURONS")
        {
            compositionList.Remove(compositionList[0]);
        }
        compositionList.Remove(compositionList[0]);

        unpackNeurons(compositionList);

        while (compositionList[0] != "SYNAPSES")
        {
            compositionList.Remove(compositionList[0]);
        }
        compositionList.Remove(compositionList[0]);

        unpackSynapses(compositionList);
    }
    public void writeNet(string composition)
    {
        // writeNet ----- takes a net and writes it to the specified path.
        string path = ag.path + "net" + id.ToString() + ".txt";

        string[] packedComposition = composition.Split(textBreak);
        File.WriteAllLines(path, packedComposition);
    }
    public string readNet()
    {
        string name = "net" + id.ToString() + ".txt";
        string path = ag.path + name;
        string composition = "";
        string[] packedComposition = new string[0];
        try
        {
            packedComposition = File.ReadAllLines(path);
        }
        catch (FileNotFoundException)
        {
            print("THERE IS NO FILE CALLED: " + name + " AT LOCATION: " + path);
        }
        for (int i = 0; i < packedComposition.Length; i++)
        {
            if (!(packedComposition[i] == ""))
            {
                composition += packedComposition[i] + textBreak;
            }
        }
        return composition;
    }

    public void run()
    {
        if (isActive)
        {
            clearAllNeurons();
            foreach (Neuron neuron in neurons[0])
            {
                neuron.getValue();
            }
            for (int i = 0; i < 3; i++)
            {
                foreach (Neuron neuron in neurons[i])
                {
                    foreach (Synapse synapse in neuron.connectedSynapses)
                    {
                        if (synapse.parent == neuron)
                        {
                            synapse.child.value += neuron.value * synapse.weight;
                        }
                    }
                }
            }
            foreach (Neuron neuron in neurons[2])
            {
                neuron.setValue();
            }
        }
    }

    //====== EVOLVE NET HELPER FUNCTIONS ====== //
    public void addNeuron(Relay relayInput)
    {
        // addNeuron ----- adds a neuron to the appropriate neuron sublist.
        int newNeuronID = neurons[0].Count + neurons[1].Count + neurons[2].Count;
        Neuron neuron = new Neuron(newNeuronID, relayInput);
        assignNeuronToList(neuron, relayInput);
    }
    private void addSynapse(Neuron parentInput, Neuron childInput)
    {
        synapses.Add(new Synapse(synapses.Count, parentInput, childInput));
    }
    public void populateSynapses()
    {
        // populateSynapses ----- creates a synapse between every input neuron and output neuron in
        // the net. Connections between hidden neurons are made later.
        foreach (Neuron inputNeuron in neurons[0])
        {
            foreach (Neuron outputNeuron in neurons[2])
            {
                synapses.Add(new Synapse(synapses.Count, inputNeuron, outputNeuron));
            }
        }
    }
    public void randomizeSynapses()
    {
        foreach(Synapse synapse in synapses)
        {
            int cc = controllerRandom.Next(0, 2);

            synapse.weight = controllerRandom.Next(0, (int)(ag.synapseRandomizeValue * 100));
            synapse.weight /= 100f;

            if (cc == 0)
            {
                synapse.weight *= -1;
            }
        }
    }
    private void pruning(List<Neuron>[] pruningNeurons, List<Synapse> pruningSynapses)
    {
        // PRUNING NEURONS //
        List<Neuron> nToPrune = new List<Neuron>();
        List<Synapse> sToPrune = new List<Synapse>();
        foreach (Neuron neuron in pruningNeurons[1])
        {
            bool tryPrune = true;
            foreach (Synapse synapse in neuron.connectedSynapses)
            {
                if (synapse.weight > ag.neuronPruneThreshold && synapse.weight < -ag.neuronPruneThreshold)
                {
                    tryPrune = false;
                }
            }
            if (tryPrune)
            {
                float cc = controllerRandom.Next(0, 100) / 100f;
                if (cc < ag.chanceOfNeuronPruning)
                {
                    nToPrune.Add(neuron);
                }
            }
        }
        // PRUNING SYNAPSES //
        foreach (Synapse synapse in pruningSynapses)
        {
            bool tryPrune = false;
            if (synapse.weight < ag.synapsePruneThreshold && synapse.weight > -ag.synapsePruneThreshold)
            {
                tryPrune = true;
            }
            if (tryPrune)
            {
                float cc = controllerRandom.Next(0, 100) / 100f;
                if (cc < ag.chanceOfSynapsePruning)
                {
                    sToPrune.Add(synapse);
                }
            }
        }
        foreach (Neuron i in nToPrune)
        {
            pruneNeuron(i);
        }
        foreach (Synapse i in sToPrune)
        {
            pruneSynapse(i);
        }
        resetIDs();
    }
    private void growth(List<Neuron>[] growingNeurons)
    {
        // GROW A NEW NEURON
        float cc = controllerRandom.Next(0, 100) / 100f;
        if (growingNeurons[1].Count < ag.maxHiddenNeurons && cc < ag.chanceOfNewNeuron)
        {
            addNeuron(null);
            int parentID = controllerRandom.Next(0, growingNeurons[0].Count);
            addSynapse(growingNeurons[0][parentID], growingNeurons[1][neurons[1].Count - 1]);
        }
        // GROW A NEW SYNAPSE
        List<Neuron> eligibleNeurons = new List<Neuron>();
        eligibleNeurons.AddRange(growingNeurons[0]);
        eligibleNeurons.AddRange(growingNeurons[1]);
        List<Neuron> connectingNeurons = findNeuronsToConnect(eligibleNeurons);
        if (connectingNeurons != null)
        {
            addSynapse(connectingNeurons[0], connectingNeurons[1]);
        }
    }
    private void perturbation(List<Synapse> perturbedSynapses)
    {
        foreach (Synapse synapse in perturbedSynapses)
        {
            float cc = controllerRandom.Next(0, 100) / 100f;
            if (cc < ag.chanceOfSynapsePerturbation)
            {
                cc = controllerRandom.Next((int)(ag.synapseMinPerturbation * 100), (int)(ag.synapseMaxPerturbation * 100 + 1)) / 100f;
                int t = controllerRandom.Next(0, 2);
                if (t == 0)
                {
                    synapse.weight += cc;
                }
                else
                {
                    synapse.weight -= cc;
                }
                if (synapse.weight > 1)
                {
                    synapse.weight -= synapse.weight - 1;
                }
                if (synapse.weight < -1)
                {
                    synapse.weight -= synapse.weight + 1;
                }
            }
        }
    }
    private List<Neuron> findNeuronsToConnect(List<Neuron> parentNeurons)
    {
        // findParentNeuron -----uses recursion to find a suitable child neuron to
        // make a synapse with.
        if (parentNeurons.Count == 0)
        {
            // base case if search is unsuccessful
            return null;
        }

        List<Neuron> connectingNeurons = new List<Neuron>();

        Neuron chosenNeuron = parentNeurons[controllerRandom.Next(0, parentNeurons.Count - 1)];
        if (neurons[0].Contains(chosenNeuron))
        {
            connectingNeurons = findChildForInput(parentNeurons, chosenNeuron);
        }
        if (neurons[1].Contains(chosenNeuron))
        {
            connectingNeurons = findChildForHidden(parentNeurons, chosenNeuron);
        }
        return connectingNeurons;
    }
    private List<Neuron> findChildForInput(List<Neuron> parentNeurons, Neuron chosenNeuron)
    {
        // findChildForInput ----- part two of the sweet, sweet recursion we've got
        // going on with the findParentNeuron

        List<Neuron> connectingNeurons = new List<Neuron>();

        Neuron childNeuron;
        int cc = controllerRandom.Next(1, neurons[1].Count + neurons[2].Count + 1);
        if (cc > neurons[1].Count) // OUTPUT CASE
        {
            childNeuron = neurons[2][cc - neurons[1].Count - 1];
        }
        else
        {
            childNeuron = neurons[1][cc - 1];
        }
        bool contains = false;
        foreach (Synapse synapse in chosenNeuron.connectedSynapses)
        {
            if (synapse.child == childNeuron)
            {
                contains = true;
            }
        }
        if (contains)
        {
            parentNeurons.Remove(chosenNeuron);
            connectingNeurons = findNeuronsToConnect(parentNeurons);
        }
        else
        {
            connectingNeurons.Add(chosenNeuron);
            connectingNeurons.Add(childNeuron);
        }
        return connectingNeurons;
    }
    private List<Neuron> findChildForHidden(List<Neuron> parentNeurons, Neuron chosenNeuron)
    {
        // findChildForInput ----- part two of the sweet, sweet recursion we've got
        // going on with the findParentNeuron
        if (parentNeurons.Count == 0)
        {
            // base case if search is unsuccessful
            return null;
        }
        List<Neuron> connectingNeurons = new List<Neuron>();

        int cc = controllerRandom.Next(1, neurons[2].Count + 1);
        Neuron childNeuron = neurons[2][cc - 1];
        bool contains = false;
        foreach (Synapse synapse in chosenNeuron.connectedSynapses)
        {
            if (synapse.child == childNeuron)
            {
                contains = true;
            }
        }
        if (contains)
        {
            parentNeurons.Remove(chosenNeuron);
            connectingNeurons = findNeuronsToConnect(parentNeurons);
        }
        else
        {
            connectingNeurons.Add(chosenNeuron);
            connectingNeurons.Add(childNeuron);
        }
        return connectingNeurons;
    }
    private void pruneNeuron(Neuron neuronToBePruned)
    {
        // pruneNeuron ----- says on the tin
        List<Synapse> cullList = new List<Synapse>();
        foreach (Synapse synapse in synapses)
        {
            if (synapse.parent == neuronToBePruned)
            {
                synapse.child.connectedSynapses.Remove(synapse);
                cullList.Add(synapse);
            }
            else if (synapse.child == neuronToBePruned)
            {
                synapse.parent.connectedSynapses.Remove(synapse);
                cullList.Add(synapse);
            }
        }
        foreach (Synapse i in cullList)
        {
            synapses.Remove(i);
        }
        for (int i = 0; i < 3; i++)
        {
            neurons[i].Contains(neuronToBePruned);
            neurons[i].Remove(neuronToBePruned);
        }
    }
    private void pruneSynapse(Synapse synapseToBePruned)
    {
        // pruneSynapse ----- says on the tin
        synapseToBePruned.parent.connectedSynapses.Remove(synapseToBePruned);
        synapseToBePruned.child.connectedSynapses.Remove(synapseToBePruned);
        synapses.Remove(synapseToBePruned);
    }

    //====== UNPACKAGE NET HELPER FUNCTIONS ======//
    public void unpackNeurons(List<string> list)
    {
        if (list[0] == "END NEURONS")
        {
            return;
        }
        string[] neuronArray = list[0].Split(',');
        Relay neuronRelay = null;
        if (neuronArray[1] != "null")
        {
            neuronRelay = relayCatalog[neuronArray[1]][Int32.Parse(neuronArray[2])];
        }
        Neuron neuron = new Neuron(Int32.Parse(neuronArray[0]), neuronRelay);
        assignNeuronToList(neuron, neuronRelay);
        list.Remove(list[0]);
        unpackNeurons(list);
    }
    public void unpackSynapses(List<string> list)
    {
        if (list[0] == "END SYNAPSES")
        {
            return;
        }
        string[] synapseArray = list[0].Split(',');
        List<Neuron> masterList = new List<Neuron>();
        masterList.AddRange(neurons[0]); masterList.AddRange(neurons[2]); masterList.AddRange(neurons[1]);
        Synapse synapse = new Synapse(Int32.Parse(synapseArray[0]), masterList[Int32.Parse(synapseArray[1])], masterList[Int32.Parse(synapseArray[2])]);
        synapse.weight = float.Parse(synapseArray[3]);
        synapses.Add(synapse);

        list.Remove(list[0]);
        unpackSynapses(list);
    }

    //====== RUN HELPER FUNCTIONS ======//
    public void clearAllNeurons()
    {
        List<Neuron> masterList = new List<Neuron>();
        for (int i = 0; i < 3; i++)
        {
            masterList.AddRange(neurons[i]);
        }
        foreach(Neuron neuron in masterList)
        {
            neuron.clear();
        }
    }
    public void createNewNet()
    {
        foreach (KeyValuePair<string, List<Relay>> pair in relayCatalog)
        {
            foreach (Relay rel in pair.Value)
            {
                addNeuron(rel);
            }
        }
        populateSynapses();
        randomizeSynapses();
    }

    //====== ARTIFICIAL NEURAL NET CHILD CLASSES ======//
    private class Neuron
    {
        /* Neuron ----- The neuron has six important members. It's id, name, connectedSyns, relay, input, and output.
         * Neurons can further be broken into three subtypes:
         * 
         * Sensors Neurons: These neurons take in data from their associated Sensor Relay, make sense of
         * it, and transmit it back out.
         * 
         * Hidden Neurons: These neurons don't interact with the outside world at all, however they add
         * to the complexity of the net.
         * 
         * Effector Neurons: These neurons take in data from their synapses, make sense of it, and
         * translate it into effects on the world.
         */
        public int id;
        public string name;
        public Relay relay;
        public List<Synapse> connectedSynapses;
        public float value;

        public Neuron(int idInput, Relay relayInput)
        {
            id = idInput;
            connectedSynapses = new List<Synapse>();
            relay = relayInput;
            name = assignName(id, relay);
            value = 0;
        }

        //====== RUNNING FUNCTIONS ======//        }
        public void setValue()
        {
            if (relay != null)
            {
                relay.value = value;
            }
        }

        public void getValue()
        {
            if (relay != null)
            {
                value = relay.value;
            }
        }
        //====== INPUT OUTPUT FUNCTIONS ======//
        public string compose()
        {
            // compose ----- takes the important information about a neuron, composes it as a string, and returns that string to the caller.
            // FORMAT IS: id, neurontype, relay id, relayType
            string composition = "";
            try
            {
                composition += id.ToString() + "," + getTypeAsString(relay) + "," + relay.id.ToString();
            }
            catch (NullReferenceException)
            {
                composition += id.ToString() + ",null,0\n";
            }
            return composition;
        }

        //====== HELPER FUNCTIONS ======//
        private string assignName(int num, Relay input)
        {
            if (input == null)
            {
                return "HiddenNueron" + num.ToString();
            }
            else
            {
                return Controller.getTypeAsString(relay) + "Neuron" + id.ToString();
            }
        }
        public void clear()
        {
            // clear ----- sets the input and output of the neuron to 0
            value = 0;
        }
    }
    private class Synapse
    {
        /* Synapse ----- Far simpler than the neuron, the synapse has four important members, it's id,
         * it's parentNeuron, it's childNeuron, and it's weight. The weight can never be above 1 or 
         * below -1.
         */

        public int id;
        public Neuron parent;
        public Neuron child;
        public float weight;

        public Synapse(int idInput, Neuron parentInput, Neuron childInput)
        {
            id = idInput;
            parent = parentInput;
            child = childInput;
            parent.connectedSynapses.Add(this);
            child.connectedSynapses.Add(this);
            weight = 0;
        }

        //====== INPUT OUTPUT FUNCTIONS ======//
        public string compose()
        {
            return id.ToString() + "," + parent.id.ToString() + "," + child.id.ToString() + "," + weight.ToString();
        }

        public Synapse cloneSynapse(List<Neuron> list, int pID, int cID)
        {
            Synapse s = new Synapse(id, list[pID], list[cID]);
            s.weight = weight;
            return s;
        }
    }

    //====== ERROR CHECKING AND DISPLAY FUNCTIONS ======//
    public List<string> neuronsToList()
    {
        // neuronsToList ----- Translates the neuron contents of an ANN to a list of strings
        // so as to display important data to the unity editor.
        List<string> list = new List<string>();
        for (int i = 0; i < 3; i++)
        {
            foreach (Neuron n in neurons[i])
            {
                string[] line = n.compose().Split(',');
                list.Add("Id: " + line[0] + "  Name: " + n.name + "  Relay Type: " + line[1] + "  Value: " + n.value.ToString());
            }
        }
        return list;
    }
    public List<string> synapsesToList()
    {
        List<string> list = new List<string>();
        foreach (Synapse s in synapses)
        {
            string[] line = s.compose().Split(',');
            list.Add("Id: " + line[0] + "  Parent: " + line[1] + "  Child " + line[2] + "  Weight: " + line[3]);
        }
        return list;
    }
    public List<string> relayToList()
    {
        List<string> composition = new List<string>();
        foreach (KeyValuePair<string, List<Relay>> entry in relayCatalog)
        {
            composition.Add(entry.Key);
            foreach (Relay relay in entry.Value)
            {
                composition.Add(relay.compose());
            }
        }
        return composition;
    }
    public void resetIDs()
    {
        int idCounter = 0;
        foreach (Neuron neuron in neurons[0])
        {
            neuron.id = idCounter;
            idCounter += 1;
        }
        foreach (Neuron neuron in neurons[2])
        {
            neuron.id = idCounter;
            idCounter += 1;
        }
        foreach (Neuron neuron in neurons[1])
        {
            neuron.id = idCounter;
            idCounter += 1;
        }

        idCounter = 0;
        foreach (Synapse synapse in synapses)
        {
            synapse.id = idCounter;
            idCounter += 1;
        }
    }
    public void debugAction()
    {
        evolveNet();
    }
    private void clearNet()
    {
        for (int i = 0; i < 3; i++)
        {
            neurons[i].Clear();
        }
        synapses.Clear();
    }

    private void assignNeuronToList(Neuron neuron, Relay relayInput)
    {
        if (relayInput is Sensor)
        {
            neurons[0].Add(neuron);
        }
        else if (relayInput is Effector)
        {
            neurons[2].Add(neuron);
        }
        else
        {
            neurons[1].Add(neuron);
        }
    }
    

    //====== NONE ESSENTIAL MEMBERS ======//
    public List<string> neuronList;
    public List<string> synapseList;
    public List<string> relayList;

    public void populateDisplay()
    {
        neuronList = neuronsToList();
        synapseList = synapsesToList();
        relayList = relayToList();
    }

    //====== GENERAL HELPER FUNCTION ======//
    public static string getTypeAsString(object ob)
    {
        string[] obType = ob.GetType().ToString().Split('+');
        return obType[obType.Length - 1];
    }
    
    public void Update()
    {
        populateDisplay();
        // FLAGGED
        if (Input.GetKeyDown(KeyCode.T))
        {
            debugAction();
        }
        // FLAGGED
    }
}
