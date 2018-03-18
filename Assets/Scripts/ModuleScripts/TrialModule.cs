using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialModule : Module {

    // Properties
    int currentTrial;
    SetupModule setupModule;
    public int trialRuntime;
    public List<string> controllerList { get; set; }
    public List<float> fitnessList { get; set; }
    Dictionary<Unit, contender> contenders;
    public int totalTrials;

    protected override void initializeComponents() {
        base.initializeComponents();
        currentTrial = 1;
        contenders = new Dictionary<Unit, contender>();
    }
    protected override void linkPartnerModules() {
        base.linkPartnerModules();
        setupModule = gameObject.GetComponent<SetupModule>();
    }

    public override void run() {
        base.run();
        if (rootDirector.time == trialRuntime) {
            resetModule();
            attemptTransition();
        }
    }

    public override void attemptTransition() {
        if (currentTrial == totalTrials + 1) {
            int tempTrials = -1;
            float tempFitness = contenders[activeUnit].fitness;
            contender tempContender = contenders[activeUnit];
            foreach (var c in contenders) {
                if (c.Value.successfulTrials >= tempTrials) {
                    if(c.Value.fitness > tempFitness) {
                        tempContender = c.Value;
                        tempTrials = c.Value.successfulTrials;
                    }
                }
            }
            transition(tempContender.unit.controller.transcribeController());
        }
    }
    public void transition(string controllerInput) {
        baseUnit.inscribeControllerIntoUnit(controllerInput);
        baseUnit.activeState.processed = true;
        rootDirector.exchangeModule = setupModule;
        deactivateModule();
    }
    
    protected override void populateModule() {
        addUnits(controllerList.Count, true);
        for (int i = 0; i < units.Count; i++) {
            units[i].inscribeControllerIntoUnit(controllerList[i]);
            contenders.Add(units[i], new contender(units[i], fitnessList[i]));
        }
    }
    public override void deactivateModule() {
        currentTrial = 1;
        controllerList.Clear();
        foreach (var c in contenders) {
            Destroy(c.Key.gameObject);
        }
        contenders.Clear();
        base.deactivateModule();
    }
    public override void resetModule() {
        rootDirector.time = 0;
        currentTrial++;
        foreach (var c in contenders) {
            if (c.Key.transitionFlag)
                c.Value.successfulTrials++;
        }
        updateDisplay();
        base.resetModule();
    }

    protected override void buildOutputs() {
        outputs = new Scrivener.OutputPackage(new List<string> { "Module Name", "Current Trial", "Planned Trials", "Active Unit", "Successes", "Runtime" }, rootDirector.bart);
        outputs.updateInfo("Module Name", moduleName);
        outputs.updateInfo("Planned Trials", totalTrials);
        outputs.updateInfo("Current Trial", currentTrial);
        outputs.updateInfo("Active Unit", activeUnit.unitName);
        outputs.updateInfo("Successes", contenders[activeUnit].successfulTrials);
    }
    public override void updateDisplay() {
        outputs.updateInfo("Current Trial", currentTrial);
        outputs.updateInfo("Active Unit", activeUnit.unitName);
        outputs.updateInfo("Successes", contenders[activeUnit].successfulTrials);
    }

    public override void saveModule() {
        base.saveModule();
    }
    public override void loadModule() {
        base.loadModule();
    }

    private class contender {
        public Unit unit;
        public float fitness;
        public int successfulTrials;

        public contender(Unit unitInput, float initialFitness) {
            unit = unitInput;
            fitness = initialFitness;
            successfulTrials = 0;
        }
    }
}

