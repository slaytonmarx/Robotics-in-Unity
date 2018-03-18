//#define DISPLAY_MODE

/* Why I used preprocessor directives here, rather than branching off into multiple director
 * subclasses like a sane person. While I feel uncomfortable using preprocessor directives in
 * object oriented programming most of the time (as if you need to specialize behavior, why
 * not just make another class and code it there), in the case of DISPLAY_MODE I felt ppds were
 * the best option. Displaying the neural net as it fires is a surprisingly cumbersome action
 * in terms of processing power, but a director that's displaying neurons firing and one that
 * isn't really aren't different in terms of the important things they're doing, at least, not
 * enough to justify splitting them up into two classes. Thus, the preprocessor directive,
 * DISPLAY_MODE. I would have liked to make this accessible to the unity client, but that would
 * involve using a bool, which would mean a check for everytime the display needs to be called,
 * which is too many times.
 */

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Director : MonoBehaviour {
    // USER OPTIONS //
    public string experimentName;
    public int experimentId;
    public bool showNetScreen;

    // Load an Experiment
    public bool loadExperiment;
    public string experimentLoadName;
    // Experiment Relevant Inputs
    public Module baseModule;
    public Module activeModule { get; set; }
    public Module exchangeModule { get; set; }

    public Scrivener bart;
    public int time;
    
    // INITIALIZATION METHODS //
    private void Start() {
        buildDirector();
    }
    private void buildDirector() {
    /* buildDirector ----- Sets up the necessary components and begins a large scale
     * experiment. Runs attemptTransitionToSetupPhase first. */
        initializeComponents();
        Tools.setText(bart.experimentOutput, experimentName);
    }
    
    // buildDirector Helper Methods
    private void initializeComponents() {
        /* assignGlobalValues ----- assigns the components which should be attached to the director
         * GameObject in the editor to their corresponding members fields. */
        bart = gameObject.GetComponent<Scrivener>();
        bart.buildScrivener();
        // Experiment Name and Id.
        string[] nameAndId = bart.chooseExperimentNameAndId();
        experimentName = nameAndId[0];
        experimentId = int.Parse(nameAndId[1]);
        // Find initial module
        try {
            Module[] modules = GetComponentsInChildren<Module>();
            foreach (var module in modules) {
                module.buildModule(this);
                if (module.initialModule)
                    baseModule = module;
            }
            activeModule = baseModule.activateModule();
        }
        catch (NullReferenceException) {
            Debug.Log("Director initializeComponents failure, no module is initial");
        }
    }

    // ACTIVE METHODS //
    private void Update() {
        if (Time.timeScale != 0)
            time++;
        if (exchangeModule != null) {
            baseModule = exchangeModule;
            activeModule = baseModule.activateModule();
            exchangeModule = null;
        }
        baseModule.run();
    }

    // MODULE CONTROLS
    public void changeActiveModule(int input) {
        baseModule.changeActiveModule(input);
    }
}
