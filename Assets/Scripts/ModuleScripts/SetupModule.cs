using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupModule : Module {

    // Properties
    public GrowModule growModule;

    protected override void linkPartnerModules() {
        base.linkPartnerModules();
        growModule = gameObject.GetComponent<GrowModule>();
    }

    public override void attemptTransition() {
        baseUnit.gameObject.SetActive(false);
        transition();
    }
    public override void transition() {
        rootDirector.exchangeModule = growModule;
        deactivateModule();
    }

    public override void run() {
        base.run();
        if (!baseUnit.activeState.processed) {
            attemptTransition();
        }
        if (baseUnit.transitionFlag) {
            outputs.updateInfo("Active State", activeUnit.activeState.name);
            outputs.updateInfo("Active Network", activeUnit.activeState.network.name);
        }
    }


    protected override void buildOutputs() {
        outputs = new Scrivener.OutputPackage(new List<string> { "Module Name", "Active State", "Active Network", "Runtime" }, rootDirector.bart);
        outputs.updateInfo("Module Name", moduleName);
        outputs.updateInfo("Active State", activeUnit.activeState.name);
        outputs.updateInfo("Active Network", activeUnit.activeState.network.name);
    }
    public override void updateDisplay() {
        outputs.updateInfo("Active State", activeUnit.activeState.name);
        outputs.updateInfo("Active Network", activeUnit.activeState.network.name);
    }

    protected override void populateModule() {
        baseUnit.gameObject.SetActive(true);
    }
    protected override void finalizeActivation() {
        activeUnit = baseUnit;
    }
    public override Module activateModule() {
        if (!baseUnit.activeState.processed) {
            transition();
            return null;
        }
        base.activateModule();
        return this;
    }
    public override void deactivateModule() {
        base.deactivateModule();
    }
    public override void resetModule() {
        rootDirector.time = 0;
        //currentTrial++;
        //foreach (var c in contenders) {
        //    if (c.unit.transitionFlag != "")
        //        c.successfulTrials++;
        //}
        //base.resetModule();
    }

    public override void saveModule() {
        base.saveModule();
    }
    public override void loadModule() {
        base.loadModule();
    }

    //public override void run() {
    //    attemptTransition();
    //}
    //public override void buildPhase() {
    //    // The baseModule should already have an updated net from grow phase or loading
    //    baseModule.SetActive(true);
    //}
    //public override void resetPhase() {

    //}
    //public override void attemptTransition() {
    //    Debug.Log("Still in Setup");
    //    if (module.unitFocus.transitionFlag != "")
    //        transition();
    //}
    //public override void transition() {
    //    unmakePhase();
    //    rootDirector.tempPhase = new GrowPhase(rootDirector, baseModule, rootDirector.numberOfModules);
    //}
    //public override void unmakePhase() {
    //    baseModule.SetActive(false);
    //}
}
