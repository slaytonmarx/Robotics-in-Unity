using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Scrivener : MonoBehaviour {
    /* Scrivener ----- The Scrivener is the center for input/output for the workshop. It is tasked with saving
     * controllers, loading controllers, saving experiments, loading experiments, changing text in the engine,
     * and other relevant functions. */ 

    // PROPERTIES //
    // Identifying Properties
    private Director rootDirector;
    public string projectDirectory;
    public string experimentParameters;
    public Text experimentOutput;
    // Output Package
    OutputPackage output { get; set; }
    // Options Properties
    public ButtonDisplay buttonRoot;
    public List<GameObject> buttons;
    public GameObject optionsRoot;
    public GameObject nameplateRoot;
    public GameObject outputRoot;
    // Camera Internals
    private int cameraMode;
    public Camera camera;

    // INITIALIZATION METHODS //

    // Core Methods
    public void buildScrivener() {
        rootDirector = gameObject.GetComponent<Director>();
        projectDirectory = findProjectDataDirectory();
        initializeButtonRoot();
        readExperimentParameters();
        Debug.Log("Scrivener Successfully Initialized");
    }
    // Helper Methods
    private void readExperimentParameters() {
        /* readExperimentParameters ----- Reads the parameters file included with an executable version of the
         * program if one exists. If one does not, does not change the parameters inputted by user in editor. */
        string dir = Directory.GetCurrentDirectory() + "\\parameters.txt";
        if (File.Exists(dir)) {
            experimentParameters = File.ReadAllText(dir).Trim('\n', ' ');
            Debug.Log("Log changed to " + experimentParameters);
        }
    }
    private string findProjectDataDirectory() {
        /* findProjectDataDirectory ----- Searches the current directory and the directory of the immediate parent
         * for the ProjectData folder. Necessary for running headless experiments, to keep the data together. */
        string dir = Directory.GetCurrentDirectory();
        if (Directory.Exists(dir + "\\ProjectData\\"))
            return dir + "\\ProjectData\\";
        else {
            dir = Directory.GetParent(dir).ToString();
            if (Directory.Exists(dir + "\\ProjectData\\"))
                return dir + "\\ProjectData\\";
            }
        return dir;
    }

    // EXPERIMENT INPUT/OUTPUT METHODS //

    // Core Methods
    
    // Helper Methods
    private void updateProjectInfo() {
        // updates the project info, should be called when a new experiment is saved
    } // THREAD
    public string[] chooseExperimentNameAndId() {
        string[] infoLines = File.ReadAllLines(projectDirectory + "ProjectInformation.txt");
        string id = infoLines[1].Split(':')[1];
        string[] nameAtoms = File.ReadAllLines(projectDirectory + "NameList.txt");
        return new string[] {chooseRandomName(nameAtoms), id};
    }
    private string chooseRandomName(string[] names) {
        List<string> firstNames = new List<string>();
        List<string> lastNames = new List<string>();
        int tempCounter = 0;
        bool tempSwitch = true;
        do {
            firstNames.Add(names[tempCounter]);
            tempCounter++;
            if (names[tempCounter] == "=")
                tempSwitch = false;
        }
        while (tempSwitch);
        for (int i = tempCounter + 1; i < names.Length; i++)
            lastNames.Add(names[i]);
        return firstNames[Random.Range(0, firstNames.Count)] + " " + lastNames[Random.Range(0, lastNames.Count)];
    }

    // CONTROLLER INPUT/OUTPUT METHODS //

    // Core Methods
    public static void writeController(string pathInput, string controllerInput, string testComment) {
        string controllerTranscription = controllerInput;
        string[] controllerLines = controllerTranscription.Split('|');
        string[] neuronLines = controllerLines[1].Split(':');
        string[] synapseLines = controllerLines[2].Split(':');
        File.WriteAllText(pathInput + "Info.txt", controllerLines[0] + "\n" + testComment);
        File.WriteAllLines(pathInput + "Neurons.txt", neuronLines);
        File.WriteAllLines(pathInput + "Synapses.txt", synapseLines);
    }
    public static string readController(string pathInput) {
        string[] infoLines = File.ReadAllLines(pathInput + "\\Info.txt");
        string[] neuronLines = File.ReadAllLines(pathInput + "\\Neurons.txt");
        string[] synapseLines = File.ReadAllLines(pathInput + "\\Synapses.txt");
        string encodedController = infoLines[0] + "|" + neuronLines[0] + ":";
        for (int i = 0; i < neuronLines.Length - 1; i++) {
            encodedController += neuronLines[i] + ":";
        }
        encodedController += neuronLines[neuronLines.Length - 1] + "|";
        for (int i = 0; i < synapseLines.Length - 1; i++) {
            encodedController += synapseLines[i] + ":";
        }
        encodedController += synapseLines[synapseLines.Length - 1];
        return encodedController;
    }
    public static string readBehavior(string pathInput) {
        // BIG THREAD
        return "Yes";
    }

    private void Update() {
        updateFixedCamera();
        readInput();
    }

    // UPDATE METHODS //
    private void updateFixedCamera() {
        if (cameraMode == 0)
            camera.transform.position = rootDirector.activeModule.getFixedCamPosition();
    }
    private void readInput() {
        if (Input.GetKeyUp("up")) {
            rootDirector.activeModule.changeActiveModule(0);
            updateFixedCamera();
        }
        if (Input.GetKeyUp("down")) {
            rootDirector.activeModule.changeActiveModule(1);
            updateFixedCamera();
        }
        if (Input.GetKeyUp("right")) {
            rootDirector.activeModule.changeActiveUnit(1);
            updateFixedCamera();
        }
        if (Input.GetKeyUp("left")) {
            rootDirector.activeModule.changeActiveUnit(0);
            updateFixedCamera();
        }
        float val = .5f;
        switch (cameraMode) {
            case 1:
                camera.transform.position = rootDirector.activeModule.getFollowCamPosition();
                break;
            case 2:
                if (Input.GetKey("w")) {
                    camera.transform.position = camera.transform.position + new Vector3(val, 0, val);
                }
                if (Input.GetKey("s")) {
                    camera.transform.position = camera.transform.position + new Vector3(-val, 0, -val);
                }
                if (Input.GetKey("d")) {
                    camera.transform.position = camera.transform.position + new Vector3(val, 0, -val);
                }
                if (Input.GetKey("a")) {
                    camera.transform.position = camera.transform.position + new Vector3(-val, 0, val);
                }
                if (Input.GetKey("q")) {
                    camera.transform.position = camera.transform.position + new Vector3(0, -val, 0);
                }
                if (Input.GetKey("e")) {
                    camera.transform.position = camera.transform.position + new Vector3(0, val, 0);
                }
                break;
            default:
                break;
        }
    }

    // BUTTON INTERACTION METHODS //

    // Menu Buttons
    public void initializeButtonRoot() {
        Button[] temp = optionsRoot.GetComponentsInChildren<Button>();
        buttons = new List<GameObject>();
        foreach (var button in temp) {
            buttons.Add(button.gameObject);
        }
        buttonRoot = new ButtonDisplay(buttons[0], null);
    }
    public void clickEventOptionsMenu() {
        if (buttonRoot.open) {
            buttonRoot.closeList();
        }
        else {
            buttonRoot.addList(new List<ButtonDisplay> { new ButtonDisplay(buttons[1], null), new ButtonDisplay(buttons[5], null) });
        }
        redrawOptions();
    }
    public void clickEventSaveMenu() {
        ButtonDisplay crawler = buttonRoot.branchButtons[0];
        if (crawler.open == true) {
            crawler.closeList();
        }
        else {
            crawler.addList(new List<ButtonDisplay> { new ButtonDisplay(buttons[2], null), new ButtonDisplay(buttons[3], null), new ButtonDisplay(buttons[4], null) });
        }
        redrawOptions();
    }
    public void clickEventCameraMenu() {
        ButtonDisplay crawler = buttonRoot.branchButtons[1];
        if (crawler.open == true) {
            crawler.closeList();
        }
        else {
            crawler.addList(new List<ButtonDisplay> { new ButtonDisplay(buttons[6], null), new ButtonDisplay(buttons[7], null), new ButtonDisplay(buttons[8], null) });
        }
        redrawOptions();
    }
    public void redrawOptions() {
        int len = buttonRoot.traverseOptions(0);
        optionsRoot.GetComponent<RectTransform>().sizeDelta = new Vector2(700, 70 + 65 * len);
    }

    // Action Buttons
    public void clickEventSaveAscendent() {
        // THREAD
        Debug.Log("Ascendent Robot Saved");
    }
    public void clickEventSaveParent() {
        // THREAD
        Debug.Log("Parent Robot Saved");
    }
    public void clickEventSaveCurrentUnit() {
        // THREAD
        Debug.Log("Current Unit Robot Saved");
    }
    public void clickEventCameraFixed() {
        cameraMode = 0;
        Debug.Log("Camera Set to Fixed Position");
    }
    public void clickEventCameraFollow() {
        cameraMode = 1;
        Debug.Log("Camera Set to Follow");
    }
    public void clickEventCameraFree() {
        cameraMode = 2;
        Debug.Log("Camera Set to Free Cam. Control position with wsad for 2d movement, and q and e for 3d.");
    }

    public class ButtonDisplay {
        /* ButtonDisplay ----- The menus function as a simple tree, with each menu having branches
         * of other buttons, which eventually terminate in button leaves. Traverse follows a simple
         * breadth first search whereas close turns a node into a branch. */ 
        public GameObject buttonObject;
        public RectTransform buttonTransform;
        public List<ButtonDisplay> branchButtons;
        public bool open;
        public ButtonDisplay(GameObject buttonObjectInput, List<ButtonDisplay> branchButtonsInput) {
            buttonObject = buttonObjectInput;
            branchButtons = branchButtonsInput;
            buttonTransform = buttonObject.GetComponent<RectTransform>();
        }
        public void closeList() {
            if (branchButtons == null)
                return;
            foreach (var branch in branchButtons) {
                branch.closeList();
                branch.buttonTransform.transform.localPosition = new Vector3(branch.buttonTransform.transform.localPosition.x, 200, 0);
            }
            branchButtons.Clear();
            open = false;
        }
        public void addList(List<ButtonDisplay> buttonsInput) {
            branchButtons = buttonsInput;
            open = true;
        }
        public int traverseOptions(int i) {
            buttonTransform.transform.localPosition = new Vector3(buttonTransform.transform.localPosition.x, -65 * i, 0);

            if (branchButtons == null)
                return i;
            
            foreach (var branch in branchButtons) {
                i = branch.traverseOptions(i + 1);
            }

            return i;
        }
    }
    public class OutputPackage {
        /* Unity doesn't support tuples, my god, but, they're so fundimental. Ok, we're good,
         * just gotta waste memory all over the place. I mean not really but, c'est la vie. 
         * I'll just make a dictionary and eat my sorrow. */
        public Dictionary<string, Text> outputs { get; private set; }
        Scrivener rootScrivener;
        GameObject nameplateRoot;
        GameObject outputsRoot;
        public OutputPackage(List<string> stringInputs, Scrivener scrivenerInput) {
            outputs = new Dictionary<string, Text>();
            rootScrivener = scrivenerInput;
            nameplateRoot = rootScrivener.nameplateRoot;
            outputsRoot = rootScrivener.outputRoot;

            for (int i = 0; i < stringInputs.Count; i++) {
                GameObject nameplate = createTextObject(stringInputs[i] + " NamePlate", i, nameplateRoot, true);
                createText(stringInputs[i], nameplate, true);
                GameObject output = createTextObject(stringInputs[i] + " Output", i, outputsRoot, false);
                outputs.Add(stringInputs[i], createText(stringInputs[i], output, false));
            }
            resizePanels(stringInputs.Count);
        }
        private Text createText(string textInput, GameObject obj, bool rightAlign) {
            Text text = obj.AddComponent<Text>();
            text.text = textInput;

            Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
            text.font = ArialFont;
            text.material = ArialFont.material;
            text.color = new Color(.2f, .2f, .2f);

            if (rightAlign)
                text.alignment = TextAnchor.MiddleRight;

            return text;
        }
        private GameObject createTextObject(string objName, int objPosition, GameObject objParent, bool rightAlign) {
            GameObject obj = new GameObject(objName);
            obj.transform.SetParent(objParent.transform);
            RectTransform rt = obj.AddComponent<RectTransform>();
            rt.anchorMin = new Vector2(1, 1);
            rt.anchorMax = new Vector2(1, 1);
            rt.pivot = new Vector2(1, 1);
            Vector3 position = Vector3.zero;
            Vector2 sizeDelta = Vector2.zero;
            if (rightAlign) {
                position = new Vector3(-3, -16 * objPosition, 0);
                sizeDelta = new Vector2(145, 16);
            }
            else {
                position = new Vector3(3, -16 * objPosition, 0);
                sizeDelta = new Vector2(150, 16);
            }
            rt.anchoredPosition = position;
            rt.sizeDelta = sizeDelta;
            return obj;
        }
        private void resizePanels(int size) {
            foreach (var root in new List<GameObject> { nameplateRoot, outputsRoot }) {
                RectTransform rt = root.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(rt.sizeDelta.x, 5 + 16 * size);
            }
        }
        public void updateInfo(string key, string newInfo) {
            outputs[key].text = newInfo;
        }
        public void updateInfo(string key, int newInfo) {
            outputs[key].text = newInfo.ToString();
        }
        public void updateInfo(string key, float newInfo) {
            outputs[key].text = newInfo.ToString();
        }
        public void Clear() {
            Text[] rtNameplate = nameplateRoot.GetComponentsInChildren<Text>();
            Text[] rtOutput = outputsRoot.GetComponentsInChildren<Text>();
            foreach (var obj in rtNameplate) {
                Destroy(obj.gameObject);
            }
            foreach (var obj in rtOutput) {
                Destroy(obj.gameObject);
            }
        }
    }
}
