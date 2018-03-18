using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Module : MonoBehaviour {
    /* Module ----- The Module is an important part of any experiment. A module is connected to the
     * Director, where it get's it's global values, and has a number of Units, each with their own
     * controller, environment, and robot. Each module has it's own time, parent controller, and
     * experiments which it runs. At the end of each generation the module finds the unit with the
     * best fitness and makes it's controller the new parent controller. */

    // Identifying Properties
    public Director rootDirector { get; protected set; }
    public Scrivener.OutputPackage outputs;
    public bool initialModule;
    public string moduleName;

    // Fixed Properties
    public List<Unit> units { get; protected set; }
    public Unit activeUnit { get; protected set; }
    public Unit baseUnit;

    // Object Properties
    protected Vector3 modulePosition { get; set; }

    public void buildModule(Director directorInput) {
        buildModule(directorInput, new Vector3(0, 0, 0));
        baseUnit.buildUnit(this, 0, modulePosition, false);
    }
    public virtual void buildModule(Director directorInput, Vector3 positionInput) {
        /* initializeModule ----- initializes the module such that all properties are initialized and units
         * are loaded. The number of units, what robot they test, and what environment they're tested in 
         * depends on the loader, but also on the module being run. */
        rootDirector = directorInput;
        modulePosition = positionInput;
        gameObject.transform.position = modulePosition;
        
        initializeComponents();
        linkPartnerModules();
    }
    protected virtual void initializeComponents() {
        Debug.Log("Module: Initializing Components");
        units = new List<Unit>();
        try {
            if (baseUnit == null) {
                baseUnit = GetComponentInChildren<Unit>();
            }
        }
        catch (MissingComponentException) {
            Debug.Log("Module initializeComponents Failure. No unit in childen");
        }
    }
    protected virtual void linkPartnerModules() {
        Debug.Log("Module: Linking Partner Modules");
    }

    protected void addUnits(int unitNumberInput, bool lockStateInput) {
        for (int i = 0; i < unitNumberInput; i++) {
            units.Add(addUnit(lockStateInput));
        }
    }
    protected Unit addUnit(GameObject testUnitInput, int idInput, Vector3 positionInput, bool lockStateInput) {
        /* makeUnit ----- Method to be called immediately following the creation of the module.
         * Creates a single instance of the chosen environment at the inputed position, complete with
         * a robot. May load many environments, depending on specifications */
        GameObject unitObject = Instantiate(testUnitInput);
        unitObject.transform.SetParent(gameObject.transform);
        Unit unit = unitObject.GetComponent<Unit>();
        unit.buildUnit(this, units.Count, positionInput, lockStateInput);
        return unit;
    }
    public Unit addUnit(bool lockStateInput) {
        GameObject unitObject = Instantiate(baseUnit.gameObject);
        unitObject.transform.SetParent(gameObject.transform);
        Unit unit = unitObject.GetComponent<Unit>();
        unit.buildUnit(this, units.Count, modulePosition, lockStateInput);
        unit.gameObject.SetActive(true);
        return unit;
    }
    public virtual Unit addUnit() {
        GameObject unitObject = Instantiate(baseUnit.gameObject);
        unitObject.transform.SetParent(gameObject.transform);
        Unit unit = unitObject.GetComponent<Unit>();
        unit.buildUnit(this, units.Count, modulePosition, false);
        unit.gameObject.SetActive(true);
        return unit;
    }

    public virtual void run() {
        updateRuntime();
    }

    public virtual void attemptTransition() {

    }
    public virtual void transition() {

    }

    protected virtual void populateModule() {
        Debug.Log("Module: Populating Module");
    }
    protected virtual void finalizeActivation() {
        Debug.Log("Module: Finalizing Module");
        activeUnit = units[0];
    }
    public virtual Module activateModule() {
        populateModule();
        finalizeActivation();
        buildOutputs();
        return this;
    }
    public virtual void deactivateModule() {
        units.Clear();
        outputs.Clear();
    }
    public virtual void resetModule() {
        /* resetModule ----- resets all environments in the module while copying the controller with the
         * best fitness into all environments and mutating it. If no controller is fitter than the parent
         * the parent will instead be copied into all robots and mutated. */
        foreach (Unit unit in units) {
            unit.resetUnit();
        }
        updateDisplay();
    }

    public virtual void saveModule() {

    }
    public virtual void loadModule() {

    }

    protected virtual void buildOutputs() {

    }
    public virtual void updateDisplay() {

    }
    public virtual void updateRuntime() {
        outputs.updateInfo("Runtime", rootDirector.time);
    }

    // GETTER METHODS //
    public virtual void changeActiveModule(int input) {
        Debug.Log("Module Changing isn't possible in this module type");
    }
    public virtual void changeActiveUnit(int input) {
        Debug.Log("Unit Changing isn't possible in this module type");
    }
    public virtual void updateModuleFocus(Module moduleFocus) {
        rootDirector.activeModule = moduleFocus;
    }
    public virtual void updateUnitFocus(int unitIdInput) {
        activeUnit = units[unitIdInput];
        updateDisplay();
    }
    public int getUnitCount() {
        return units.Count;
    }
    
    public virtual Vector3 getFixedCamPosition() {
        return activeUnit.stage.fixedCameraPosition;
    }
    public virtual Vector3 getFollowCamPosition() {
        return activeUnit.robot.focus.transform.position + new Vector3(-5, 3, -5);
    }

    // SETTER METHODS //
    public void inscribeControllerIntoUnits(string controllerInput) {
        foreach (Unit unit in units) {
            unit.inscribeControllerIntoUnit(controllerInput);
        }
    }
}