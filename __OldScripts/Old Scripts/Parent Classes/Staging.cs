using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Staging : MonoBehaviour {

    public Text CommonText;
    public Text GenerationText;
    public Text TimeText;
    public Text FitnessText;
    public Text ParentFitnessText;
    public GameObject globalsObject;
    public Camera stageCamera;

    public int time;
    public int currentModule;

    public AnnGlobals ag;
    public HubGlobals hg;
    public GeneralGlobals gg;

    public List<Module> modules;

	// Use this for initialization
	void Start () {
        initializeGlobals();
        time = 0;
        currentModule = 0;
        modules = new List<Module>();

        // FLAGGED
        modules.Add(createModule(0, typeof(TableModule), ag.seed, new Vector3(0, 0, 0)));
        // FLAGGED
    }
    public void Update()
    {
        time += 1;
        cameraInput();
        updateOutputs();
        // FLAGGED
        if (Input.GetKeyDown(KeyCode.Y))
        {

        }
        // FLAGGED
    }

    public void cameraInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Module curMod = modules[currentModule];
            if (curMod.currentUnit != 0)
            {
                curMod.currentUnit -= 1;
                stageCamera.transform.position = curMod.units[curMod.currentUnit].cameraPosition;
                stageCamera.transform.eulerAngles = new Vector3(15,45,0);
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Module curMod = modules[currentModule];
            if (curMod.currentUnit != curMod.units.Count - 1)
            {
                curMod.currentUnit += 1;
                stageCamera.transform.position = curMod.units[curMod.currentUnit].cameraPosition;
                stageCamera.transform.eulerAngles = new Vector3(15, 45, 0);
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentModule != 0)
            {
                currentModule -= 1;
                Module curMod = modules[currentModule];
                stageCamera.transform.position = curMod.units[curMod.currentUnit].cameraPosition;
                stageCamera.transform.eulerAngles = new Vector3(15, 45, 0);
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(currentModule != modules.Count - 1)
            {
                currentModule += 1;
                Module curMod = modules[currentModule];
                stageCamera.transform.position = curMod.units[curMod.currentUnit].cameraPosition;
                stageCamera.transform.eulerAngles = new Vector3(15, 45, 0);
            }
        }
    }
    public void updateOutputs()
    {
        TimeText.text = "Time: " + time.ToString();
    }

    public void initializeGlobals()
    {
        try
        {
            ag = globalsObject.GetComponent<AnnGlobals>();
            hg = globalsObject.GetComponent<HubGlobals>();
            gg = globalsObject.GetComponent<GeneralGlobals>();
        }
        catch (UnassignedReferenceException)
        {
            print("Global Object Empty");
        }
    }

    public Module createModule(int id, Type moduleType, int seedInput, Vector3 moduleOrigin)
    {
        GameObject moduleHolder = new GameObject();
        moduleHolder.name = moduleType.ToString() + id.ToString();
        Module mod = moduleHolder.AddComponent(moduleType) as Module;
        mod.initializeModule(id, this, seedInput, moduleOrigin);
        return mod;
    }
}
