using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowModule : Module {

    protected TrialModule trialModule;
    protected List<GardenModule> gardens;
    protected GardenModule activeGarden;
    public List<string> successfulControllers;
    public List<float> controllerFitnesses;
    public int experimentRuntime;
    public int totalGenerations;
    public int totalModules;
    public int totalUnits;
    // Rapid Evolution
    public bool rapidEvolution;
    public int REInterval;
    public int RECycles;

    public override void buildModule(Director directorInput, Vector3 positionInput) {
        /* initializeModule ----- initializes the module such that all properties are initialized and units
         * are loaded. The number of units, what robot they test, and what environment they're tested in 
         * depends on the loader, but also on the module being run. */
        base.buildModule(directorInput, positionInput);
    }
    protected override void initializeComponents() {
        base.initializeComponents();
        successfulControllers = new List<string>();
        gardens = new List<GardenModule>();
    }
    protected override void linkPartnerModules() {
        base.linkPartnerModules();
        trialModule = gameObject.GetComponent<TrialModule>();
    }

    protected GardenModule addGarden() {
        GameObject moduleObject = Tools.makeObject("Garden " + gardens.Count.ToString(), gameObject);
        moduleObject.transform.SetParent(gameObject.transform);
        GardenModule mod = moduleObject.AddComponent<GardenModule>();
        mod.buildModule(rootDirector, this, gardens.Count, new Vector3(0, -30 * gardens.Count, 0), totalGenerations, totalUnits);
        mod.activateModule();
        return mod;
    }
    
    public override void run() {
        base.run();
        if (rootDirector.time == experimentRuntime) {
            foreach (Module garden in gardens) {
                garden.resetModule();
            }
            attemptTransition();
        }
    }

    public override void attemptTransition() {
        int earlyOutSum = 0;
        foreach (var garden in gardens) {
            earlyOutSum += garden.units.Count;
        }
        if (gardens[0].currentGeneration == totalGenerations + 1 || earlyOutSum == 0) {
            // If any are successful then successfulControllers will be
            // greater than zero, thus we can transition to phase 2.
            foreach (var garden in gardens) {
                successfulControllers.AddRange(garden.successfulControllers);
                controllerFitnesses.AddRange(garden.controllerFitnesses);
            }
            if (successfulControllers.Count != 0) {
                transition(successfulControllers, controllerFitnesses);
            }
            else {
                resetModule();
            }
        }
    }
    public void transition(List<string> controllersInput, List<float> fitnessInput) {
        if (trialModule == null)
            saveModule();
        else {
            trialModule.controllerList = new List<string>();
            trialModule.fitnessList = new List<float>();
            trialModule.controllerList.AddRange(controllersInput);
            trialModule.fitnessList.AddRange(fitnessInput);
            rootDirector.exchangeModule = trialModule;
            deactivateModule();
        }
    }

    protected override void populateModule() {
        base.populateModule();
        for (int i = 0; i < totalModules; i++) {
            gardens.Add(addGarden());
        }
        baseUnit.gameObject.SetActive(false);
    }
    protected override void finalizeActivation() {
        activeUnit = gardens[0].units[0];
    }
    public override Module activateModule() {
        base.activateModule();
        activeGarden = gardens[0];
        updateDisplay();
        return this;
    }
    public override void deactivateModule() {
        foreach (var garden in gardens) {
            Destroy(garden.gameObject);
        }
        gardens.Clear();
        successfulControllers.Clear();
        base.deactivateModule();
    }
    public override void resetModule() {
        deactivateModule();
        activateModule();
    }

    protected override void buildOutputs() {
        outputs = new Scrivener.OutputPackage(new List<string> { "Module Name", "Current Generation", "Planned Generations", "Active Garden", "Parent Fitness", "Active Unit", "Unit Fitness", "Runtime" }, rootDirector.bart);
        outputs.updateInfo("Module Name", moduleName);
        outputs.updateInfo("Planned Generations", totalGenerations);
        outputs.updateInfo("Current Generation", 1);
    }
    public override void updateDisplay() {
        outputs.updateInfo("Current Generation", activeGarden.currentGeneration);
        outputs.updateInfo("Active Garden", activeGarden.moduleName);
        outputs.updateInfo("Parent Fitness", activeGarden.parentFitness);
        outputs.updateInfo("Active Unit", activeGarden.activeUnit.unitName);
    }
    public override void updateRuntime() {
        base.updateRuntime();
        outputs.updateInfo("Unit Fitness", activeGarden.activeUnit.activeFitness.fitness);
    }

    public override void saveModule() {
        base.saveModule();
    }
    public override void loadModule() {
        base.loadModule();
    }

    public override void changeActiveModule(int input) {
        int temp = activeGarden.gardenId;
        switch (input) {
            case 0:
                temp = activeGarden.gardenId;
                if (temp != 0) {
                    updateModuleFocus(gardens[temp - 1]);
                }
                break;
            case 1:
                if (temp != gardens.Count - 1) {
                    updateModuleFocus(gardens[temp + 1]);
                }
                break;
            default:
                break;
        }
        updateDisplay();
    }
    public override void changeActiveUnit(int input) {
        int temp;
        switch (input) {
            case 0:
                temp = activeUnit.unitId;
                if (temp != 0) {
                    activeGarden.updateUnitFocus(temp - 1);
                }
                break;
            case 1:
                temp = activeUnit.unitId;
                if (temp != activeGarden.units.Count - 1) {
                    activeGarden.updateUnitFocus(temp + 1);
                }
                break;
            default:
                break;
        }
    }
    public override void updateModuleFocus(Module moduleFocus) {
        activeGarden = moduleFocus as GardenModule;
    }

    public override Vector3 getFixedCamPosition() {
        return activeGarden.activeUnit.stage.fixedCameraPosition;
    }
    public override Vector3 getFollowCamPosition() {
        return activeGarden.activeUnit.robot.focus.transform.position + new Vector3(-5, 3, -5);
    }

    protected class GardenModule : Module {
        public GrowModule rootModule;
        public int gardenId;
        public List<string> successfulControllers;
        public List<float> controllerFitnesses;
        public int currentGeneration;
        public int totalGenerations;
        public float parentFitness;
        public string parentController;

        public void buildModule(Director directorInput, GrowModule moduleInput, int idInput, Vector3 positionInput, int generationsTotalInput, int unitsTotalInput) {
            rootModule = moduleInput;
            baseUnit = rootModule.baseUnit;
            totalGenerations = generationsTotalInput;
            gardenId = idInput;
            currentGeneration = 1;
            moduleName = gameObject.name;
            successfulControllers = new List<string>();
            controllerFitnesses = new List<float>();
            base.buildModule(directorInput, positionInput);
        }
        protected override void populateModule() {
            addUnits(rootModule.totalUnits, true);
        }
        protected override void finalizeActivation() {
            activeUnit = units[0];
            parentFitness = -10000000;
            parentController = activeUnit.controller.transcribeController();
        }
        public void evolveModule() {
            int bestEnvId = -1;
            float bestFitness = parentFitness;
            List<Unit> toRemove = new List<Unit>();
            for (int i = 0; i < units.Count; i++) {
                if (units[i].activeFitness.fitness > bestFitness) {
                    bestEnvId = i;
                    bestFitness = units[i].activeFitness.fitness;
                }
            }
            if (bestEnvId != -1) { // Best Controller was not parent, replace parent with best controller
                parentFitness = bestFitness;
                parentController = units[bestEnvId].controller.transcribeController();
            }
            foreach (var unit in units) {
                if (!unit.transitionFlag) {
                    unit.inscribeControllerIntoUnit(parentController);
                    if (rootModule.rapidEvolution) {
                        if (rootModule.REInterval % currentGeneration == 0) {
                            for (int i = 0; i < rootModule.RECycles; i++) {
                                unit.controller.mutateNet();
                            }
                        }
                    }
                    else
                        unit.controller.mutateNet();
                }
                else {
                    successfulControllers.Add(unit.controller.transcribeController());
                    controllerFitnesses.Add(unit.activeFitness.fitness);
                    toRemove.Add(unit);
                }
                unit.resetUnit();
            }
            foreach (var unit in toRemove) {
                units.Remove(unit);
                unit.gameObject.SetActive(false);
            }
            currentGeneration++;
            }
        public override void resetModule() {
            rootDirector.time = 0;
            evolveModule();
            rootModule.updateDisplay();
            base.resetModule();
        }
    }
}
