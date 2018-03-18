//#define ENABLE_DISPLAY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork : MonoBehaviour {

    // USER INPUTS //
    public bool loadNetwork;
    public string networkLoadPath;

    // NEURON AND SYNAPSE CLASSES //
    private Controller rootController;
    private bool isRootNet;
    private List<Neuron> sensorNeurons;
    private List<Neuron> effectorNeurons;
    private List<Neuron> hiddenNeurons;
    private List<Synapse> synapses;
    public string controllerLoadPath;
    public string netTag; // Must be entered as TagOne-TagTwo-TagThree due to editor restrictions
    public float subnetImportance;
    public List<Sensor> sensors;
    public List<Effector> effectors;

    // CONTROLLER CONSTRUCTION METHODS //
    // Core Methods
    public void buildNeuralNetwork(Controller controllerInput) {
        /* NeuralNet ----- Constructor for NeuralNet, taking special input in the form
            * of netTag which are then fed into build net, and to the various sensors and 
            * effectors to ensure they function as desired. */
        initializeComponents(controllerInput);
        findRelevantPartsInRobot();
        if (controllerLoadPath == "") {
            growNeuralNetwork(netTag);
        }
        else {
            loadNeuralNetwork();
        }
    }
    public void buildNeuralNetwork(Controller controllerInput, string netTagInput, bool isRootNetInput) {
        /* NeuralNet ----- Constructor for NeuralNet, taking special input in the form
            * of netTag which are then fed into build net, and to the various sensors and 
            * effectors to ensure they function as desired. */
        initializeComponents(controllerInput, netTagInput, isRootNetInput);
        findRelevantPartsInRobot();
        growNeuralNetwork(netTag);
    }
    public void buildNeuralNetwork(Controller controllerInput, string netInput) {
        initializeComponents(controllerInput, netInput.Split('|')[0], false);
        subnetImportance = -1;
        //netName = "Net " + rootController.nets.Count.ToString(); THREAD
        
        findRelevantPartsInRobot();
        growNeuralNetwork(netTag);
        inscribeController(netInput);
    }
    public void findRelevantPartsInRobot() {
        /* adaptController ----- Method to be called immediately in start. Scans through the child
            * objects of the attached gameObject for sensors and effectors and places them the 
            * appropriate List. Does not create the neural net, that function being carried
            * out by buildNeuralNet. */
        Sensor[] drossSensors = rootController.sensors;
        Effector[] drossEffectors = rootController.effectors;
        foreach (Sensor sen in drossSensors) {
            if (sen.netTag.Contains(netTag))
                sensors.Add(sen);
        }
        foreach (Effector eff in drossEffectors) {
            if (eff.netTag.Contains(netTag))
                effectors.Add(eff);
        }
    }
    public void growNeuralNetwork(string keywords) {
        /* buildNeuralNet ----- Method to be called in initializeController. Scans through the 
            * lists of sensors and effectors, creating neurons for them. Links said neurons by 
            * synapses in a manner specified by special keywords string from initializeController. */
        foreach (Sensor sen in sensors) {
            SensorNeuron sensorNeuron = new SensorNeuron(sen);
            sensorNeurons.Add(sensorNeuron);
        }
        if (rootController.hiddenNeurons) {
            for (int i = 0; i < sensors.Count; i++) {
                HiddenNeuron hiddenNeuron = new HiddenNeuron();
                hiddenNeurons.Add(hiddenNeuron);
            }
        }
        foreach (Effector eff in effectors) {
            EffectorNeuron effectorNeuron = new EffectorNeuron(eff);
            effectorNeurons.Add(effectorNeuron);
        }
        foreach (SensorNeuron senRon in sensorNeurons) {
            foreach (EffectorNeuron effRon in effectorNeurons) {
                if (senRon.checkConnectionTag(effRon)) {
                    Synapse syn = new Synapse(senRon, effRon);
                    senRon.addSynapse(syn);
                    synapses.Add(syn);
                }
            }
        }
    }
    // Helper Methods
    private void initializeComponents(Controller controllerInput, string netTagInput, bool isRootNetInput) {
        rootController = controllerInput;
        netTag = netTagInput;
        isRootNet = isRootNetInput;
        subnetImportance = -1;
        name = netTag + " net";
        sensorNeurons = new List<Neuron>();
        effectorNeurons = new List<Neuron>();
        hiddenNeurons = new List<Neuron>();
        synapses = new List<Synapse>();
        sensors = new List<Sensor>();
        effectors = new List<Effector>();
    }
    private void initializeComponents(Controller controllerInput) {
        rootController = controllerInput;
        subnetImportance = -1;
        name = netTag + " net";
        sensorNeurons = new List<Neuron>();
        effectorNeurons = new List<Neuron>();
        hiddenNeurons = new List<Neuron>();
        synapses = new List<Synapse>();
        sensors = new List<Sensor>();
        effectors = new List<Effector>();
    }

    // CONTROLLER ACTIVITY METHODS //

    // Core Methods
    public void fireNet() {
        /* fireNet ----- Sends a pulse of activity through the neural net, checking sensors for 
            * updated values and altering the robots effector neurons to follow suite. */
        foreach (SensorNeuron senRon in sensorNeurons)
            senRon.fireNeuron();
        if (hiddenNeurons.Count > 0)
            foreach (HiddenNeuron hinRon in hiddenNeurons)
                hinRon.fireNeuron();
        foreach (EffectorNeuron effRon in effectorNeurons) {
            effRon.normalize();
            effRon.fireNeuron();
        }
    }

    // CONTROLLER MUTATION METHODS //

    // Core Methods
    public void mutateNet() {
        /* mutateNet ----- Mutates the current neural net, altering it's values by a randomness 
            * factor specified. */

        // Basic Synapse Mutation
        foreach (Synapse syn in synapses) {
            float randNum = Random.Range(0, 100) / 100f;
            if (randNum <= rootController.mutationChance) {
                float mutation = Random.Range(rootController.mutationMinimum, rootController.mutationMaximum);
                syn.alterWeight(mutation);
            }
        }
        // Pruning Synpases
        // Adding Hidden Neurons
        // Pruning Hidden Neurons
    }

    // CONTROLLER TRANSCRIPTION/INSCRIPTION METHODS //
    public void saveNeuralNetwork(string path) {
        Scrivener.writeController(path, transcribeController(), "");
    }
    public void loadNeuralNetwork() {
        inscribeController(Scrivener.readController(networkLoadPath));
    }
    public string transcribeController() {
        string controllerTranscription = netTag + "|";
        List<Neuron> allNeurons = Tools.consolidateLists(new List<List<Neuron>> { sensorNeurons, effectorNeurons, hiddenNeurons });
        Dictionary<Neuron, int> neuronFile = new Dictionary<Neuron, int>();
        int neuronCap = allNeurons.Count - 1;
        int synapseCap = synapses.Count - 1;
        controllerTranscription += hiddenNeurons.Count.ToString() + ":";
        for (int i = 0; i < neuronCap; i++) {
            controllerTranscription += allNeurons[i].transcribeNeuron(i) + ":";
            neuronFile.Add(allNeurons[i], i);
        }
        controllerTranscription += allNeurons[neuronCap].transcribeNeuron(neuronCap) + "|"; // This method was chosen for best time
        neuronFile.Add(allNeurons[neuronCap], neuronCap);
        for (int i = 0; i < synapseCap; i++) {
            controllerTranscription += synapses[i].transcribeSynapse(neuronFile) + ":";
        }
        controllerTranscription += synapses[synapseCap].transcribeSynapse(neuronFile);
        return controllerTranscription;
    }
    public void inscribeController(string controllerInput) {
        string[] controllerLines = controllerInput.Split('|');
        string[] neuronLines = controllerLines[1].Split(':');
        string[] synapseLines = controllerLines[2].Split(':');
        string[] tempSynapseLine;
        hiddenNeurons.Clear(); // All hidden neurons are cleared with each inscription
        synapses.Clear();
        netTag = controllerLines[0];
        // Create new hidden neuron population to match the inscriber
        for (int i = 0; i < int.Parse(neuronLines[0]); i++) {
            hiddenNeurons.Add(new HiddenNeuron());
        }

        List<Neuron> allNeurons = Tools.consolidateLists(new List<List<Neuron>> { sensorNeurons, effectorNeurons, hiddenNeurons });

        // Clear Neuron Synapses (we need only clear the sensors and effectors as hidden was cleared)
        for (int i = 0; i < sensorNeurons.Count + effectorNeurons.Count; i++) {
            allNeurons[i].clearSynapses();
        }

        // Create new synapses based on inscription controller
        for (int i = 0; i < synapseLines.Length; i++) {
            tempSynapseLine = synapseLines[i].Split('=');
            Synapse tempSyn = new Synapse(allNeurons[int.Parse(tempSynapseLine[1])], allNeurons[int.Parse(tempSynapseLine[2])], float.Parse(tempSynapseLine[0]));
            allNeurons[int.Parse(tempSynapseLine[1])].addSynapse(tempSyn);
            synapses.Add(tempSyn);
        }
    }

    // NEURON AND SYNAPSE CLASSES //
    private class Neuron {
        /* Nueron ----- Highly straight forward class, acts as the neurons for the purpose of
         * activity, the neurons of the neural net. Meant to be a parent class for specialized
         * neurons. */
        protected float neuronValue;
        protected SensorEffector animator;
        public List<Synapse> synapses;
        public Neuron(SensorEffector animatorInput) {
            animator = animatorInput;
            synapses = new List<Synapse>();
        }
        public virtual void updateNeuron(float input) {
            /* updateNeuron ----- updates the neuron based on the attached element of the neuron.
             * Updates cumulatively. For sensorNeurons, float value is ignored in place of drawing 
             * a value from the attached sensor. Otherwise the value may be altered depending on
             * the nature of the neuron. */
#if ENABLE_DISPLAY
            if (image != null) {
                float r = 0;
                float g = 0;
                if (neuronValue > 0) {
                    g = neuronValue;
                }
                else {
                    r = neuronValue * -1;
                }
                image.displayColor = new Color(r, g, 0, .9f);
            }
#endif
        }
        public virtual void fireNeuron() {
            /* fireNeuron ----- sends the signal down the neurons synapses, updating them.
             * Clears the neurons value after it's been fired. */
        }
        public virtual string transcribeNeuron(int inputId) {
            return inputId.ToString() + "=";
        }
        public void addSynapse(Synapse synInput) {
            synapses.Add(synInput);
        }
        public void clearSynapses() {
            synapses.Clear();
        }
        public float getNeuronValue() {
            return neuronValue;
        }
        public void setNeuronValue(float input) {
            neuronValue = input;
        }
        public bool checkConnectionTag(Neuron neuronInput) {
            if (neuronInput.animator.connectionTag.Contains(animator.connectionTag))
                return true;
            return false;
        }
    }
    private class SensorNeuron : Neuron {
        public SensorNeuron(Sensor sensorInput) : base(sensorInput) {
        }
        public override void updateNeuron(float input) {
            neuronValue = animator.value;
            base.updateNeuron(0);
        }
        public override void fireNeuron() { 
            updateNeuron(0);
            foreach (Synapse syn in synapses) {
                syn.messenger();
            }
            neuronValue = 0;
        }
        public override string transcribeNeuron(int inputId) {
            return base.transcribeNeuron(inputId) + "Sensor";
        }
    }
    private class EffectorNeuron : Neuron {
        public EffectorNeuron(Effector effectorInput) : base(effectorInput) {
        }
        public override void updateNeuron(float input) {
            neuronValue += input;
            base.updateNeuron(0);
        }
        public override void fireNeuron() {
            animator.parseValue(neuronValue);
            neuronValue = 0;
        }
        public override string transcribeNeuron(int inputId) {
            return base.transcribeNeuron(inputId) + "Effector";
        }
        public void normalize() {
            if (neuronValue > 1)
                neuronValue = 1;
            else if (neuronValue < -1)
                neuronValue = -1;
        }
    }
    private class HiddenNeuron : Neuron {
        public HiddenNeuron() : base(null) {
        }
        public override void updateNeuron(float input) {
            neuronValue += input;
            base.updateNeuron(0);
        }
        public override void fireNeuron() {
            foreach (Synapse syn in synapses) {
                syn.messenger();
            }
            neuronValue = 0;
        }
        public override string transcribeNeuron(int inputId) {
            return base.transcribeNeuron(inputId) + "Hidden";
        }
    }
    private class Synapse {
        float weight;
        Neuron parentNeuron;
        Neuron childNeuron;

        public Synapse(Neuron parentInput, Neuron childInput) {
            weight = (float)Random.Range(-100, 101) / 100;
            parentNeuron = parentInput;
            childNeuron = childInput;
#if ENABLE_DISPLAY
            dispImage = new NetDisplayImage();
            if (dispImage != null) {
                float r = 0;
                float g = 0;
                if (weight > 0)
                    g = weight;
                else
                    r = weight * -1;
                dispImage.displayColor = new Color(g, r, 0, .9f);
            }
#endif
        }
        public Synapse(Neuron parentInput, Neuron childInput, float weightInput) {
            if (weightInput >= -1 && weightInput <= 1) {
                weight = weightInput;
            }
            else {
                weight = 0;
            }
            parentNeuron = parentInput;
            childNeuron = childInput;
#if ENABLE_DISPLAY
            dispImage = new NetDisplayImage();
            if (dispImage != null) {
                float r = 0;
                float g = 0;
                if (weight > 0)
                    g = weight;
                else
                    r = weight * -1;
                dispImage.displayColor = new Color(g, r, 0, .9f);
            }
#endif
        }

        public void alterWeight(float mutation) {
            if (weight + mutation >= -1 && weight + mutation <= 1) {
                weight += mutation;
            }
        }

        public void messenger() {
            float val = parentNeuron.getNeuronValue() * weight;
            paintSynapseImage(val);
            childNeuron.updateNeuron(val);
        }

        public string transcribeSynapse(Dictionary<Neuron, int> neuronFile) {
            string parentNeuronId = neuronFile[parentNeuron].ToString();
            string childNeuronId = neuronFile[childNeuron].ToString();
            return weight.ToString() + "=" + parentNeuronId + "=" + childNeuronId;
        }
        public void paintSynapseImage(float val) {

        }

        public Neuron getParent() {
            return parentNeuron;
        }
        public Neuron getChild() {
            return childNeuron;
        }
        public float getWeight() {
            return weight;
        }

        public void setWeight(float weightInput) {
            weight = weightInput;
        }
#if ENABLE_DISPLAY
        public void setSynapseImage(NetDisplayImage synapseImageInput) {
            dispImage = synapseImageInput;
        }
#endif
    }

    // NET DISPLAY METHODS //
#if ENABLE_DISPLAY
    private class NetDisplay {
        private Director director;
        public GameObject display;
        public List<NetDisplayImage> displayImages;

        public NetDisplay(ref NeuralNet netInput, Director directorInput) {
            director = directorInput;
            displayImages = new List<NetDisplayImage>();
            display = Instantiate(Resources.Load("Images\\NetBackground") as GameObject);
            display.transform.SetParent(director.canvas.transform);

            display.transform.position += new Vector3(0, 515, 0);
            List<Neuron>[] neurons = { netInput.sensorNeurons as List<Neuron>, netInput.hiddenNeurons as List<Neuron>, netInput.effectorNeurons as List<Neuron> };
            for (int i = 0; i < 3; i++) {
                int row = 0;
                for (int j = 0; j < neurons[i].Count; j++) {
                    var ron = neurons[i][j];
                    ron.image = new NetDisplayImage();
                    ron.image.image = Instantiate(Resources.Load("Images\\Neuron")) as GameObject;
                    ron.image.image.transform.SetParent(display.transform);
                    if (j % 6 == 0 && j != 0)
                        row++;
                    ron.image.image.transform.position += new Vector3(90 + (40 * (j % 6)) + (30 * row), 590 - (50 * i) - (35 * row), 0);
                    displayImages.Add(ron.image);
                }
            }
            List<Synapse> syntemp = new List<Synapse>();
            foreach (List<Neuron> ronList in neurons) {
                foreach (Neuron ron in ronList) {
                    foreach (Synapse syn in ron.synapses) {
                        syntemp.Add(syn);
                    }
                }
            }
            foreach (Synapse syn in syntemp) {
                syn.dispImage.image = Instantiate(Resources.Load("Images\\Synapse")) as GameObject;
                syn.dispImage.image.transform.SetParent(display.transform);
                displayImages.Add(syn.dispImage);

                Vector3 differenceVector = syn.getParent().image.image.transform.position - syn.getChild().image.image.transform.position;
                Image im = syn.dispImage.image.GetComponent<Image>();
                im.rectTransform.sizeDelta = new Vector2(differenceVector.magnitude, 1);
                im.rectTransform.pivot = new Vector2(0, 0.5f);
                im.rectTransform.position = syn.getChild().image.image.transform.position;
                float angle = Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg;
                im.rectTransform.rotation = Quaternion.Euler(0, 0, angle);
            }
            display.SetActive(false);
        }

        public void update() {
            if (display.activeSelf) {
                foreach (NetDisplayImage im in displayImages) {
                    im.image.GetComponent<Image>().color = im.displayColor;
                }
            }
        }

        public void unmaskDisplay(bool YesOrNo) {
            display.SetActive(YesOrNo);
        }
    }
    public class NetDisplayImage {
        public GameObject image;
        public Color displayColor;
        public NetDisplayImage() {
            displayColor = new Color(1, 1, 1);
        }
    }
#endif
}
