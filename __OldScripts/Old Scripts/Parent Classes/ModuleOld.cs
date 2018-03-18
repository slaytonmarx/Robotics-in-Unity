using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Module : MonoBehaviour {

    /* Module ----- [Parent Class]
     * The module organizes experiments run from staging. It has a child class, the unit, which bundles
     * three important components of each experiment, the environment, the task, and the robot. Modules
     * also deal with meta parts of the experiment, like telling controllers to work a certain way and
     * deciding what env, task, and robot go together. The module also has a public class, the module
     * profile, this keeps track of several important members, like the seed, the staging, the origin,
     * and other details. Finally the module also contains the TinkerBox class, which is used to make
     * gameobjects in the unity simulation.
     */

    // Root
    public Staging rootStaging;

    // Globals
    public AnnGlobals ag;
    public HubGlobals hg;

    // Informational Components
    public int ModuleId;
    public System.Random moduleRandom;
    public string ModuleName;
    public int ModuleSeed;
    public Vector3 Origin;
    public int currentUnit;
    public int generation;
    
    public List<Unit> units;

    public string parentNet;
    public float parentFitness;
    
    void Awake()
    {
        // Everything that can be properly set up before hand is done so in start, so as to avoid
        // confusion or null calls
        units = new List<Unit>();
        parentFitness = -5000;
        generation = 0;
    }
    public void Update()
    {
        run();
    }

    public virtual void initializeModule(int idInput, Staging stagingInput, int seedInput, Vector3 originInput)
    {
        ModuleId = idInput;
        moduleRandom = new System.Random(seedInput);
        ModuleName = this.GetType().ToString();
        rootStaging = stagingInput;
        ModuleSeed = seedInput;
        Origin = originInput;

        ag = rootStaging.ag;
        hg = rootStaging.hg;
    }
    public virtual void run()
    {

    }
    public virtual void evolveUnits()
    {
        foreach(Unit unit in units)
        {
            if(unit.task.fitness > parentFitness)
            {
                parentFitness = unit.task.fitness;
                parentNet = unit.controller.packageNet();
            }
        }
        foreach(Unit unit in units)
        {
            unit.controller.unpackageNet(parentNet);
            unit.controller.evolveNet();
        }
        generation += 1;
    }

    public virtual Unit buildUnit(int unitId, string unitName)
    {
        // Creates the unit object
        GameObject unitHolder = new GameObject();
        unitHolder.name = unitName;
        unitHolder.transform.parent = gameObject.transform;
        Unit unit = unitHolder.AddComponent<Unit>();

        unit.initializeUnit(unitId, this, adjustOrigin(unitId));

        return unit;

        /* ====== For other modules, the system is as follows
         * Unit unit = base.buildUnit(...);
         * <ERCT>
         * Environment environment = createObject(typeof(Environment), unit.gameObject) as Environment;
         * Robot robot = createObject(typeof(Robot), unit.gameObject) as Robot;
         * Controller controller = createObject(typeof(Controller), unit.gameObject) as Controller;
         * Task task = createObject(typeof(Task), unit.gameObject) as Task;
         * 
         * unit.constructUnit(environment, controller, robot, task);
         * 
         * environment.initializeEnvironment(...);
         * robot.initializeRobot(...);
         * controller.initializeController(...);
         * task.initializeTask(...);
         */

    }

    public virtual void iterateTask()
    {

    }

    public object createObject(Type type, GameObject parent)
    {
        /* createObject ----- create object takes a type and creates a GameObject, names the gameobject after that type,
         * assigns that type as a component to the gameobject, and then returns the component.
         */
        GameObject newObject = new GameObject();
        newObject.transform.parent = parent.transform;
        newObject.name = type.ToString();

        object component = newObject.AddComponent(type);
        return component;
    }

    public virtual Vector3 adjustOrigin(int unitId)
    {
        int collumn = unitId % 3;
        int row = unitId / 3;
        Vector3 unitOrigin = Origin + new Vector3(collumn * 25 , 0, row * 25);
        return unitOrigin;
    }

    public class Unit : MonoBehaviour
    {
        // Root
        public Module rootModule;

        // Informational Components
        public int id;
        public Vector3 origin;
        public Vector3 cameraPosition;

        // Existential Components
        public Environment environment;
        public Robot robot;
        public Controller controller;
        public Task task;

        public void initializeUnit(int unitId, Module moduleInput, Vector3 originInput)
        {
            id = unitId;
            rootModule = moduleInput;
            origin = originInput;
            cameraPosition = originInput + new Vector3(-10, 5, -10);
        }

        public void constructUnit(Environment environmentInput, Controller controllerInput, Robot robotInput, Task taskInput)
        {
            environment = environmentInput;
            controller = controllerInput;
            robot = robotInput;
            task = taskInput;
        }
        public void resetUnit()
        {
            environment.resetEnvironment();
            robot.resetRobot();
            task.resetTask();
        }
    }

}
