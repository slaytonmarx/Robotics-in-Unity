using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestModule : Module {

    /* TestModule ----- Just trying to figure some stuff out right now.
     * 
     */

    public override void initializeModule(int idInput, Staging stagingInput, int seedInput, Vector3 originInput)
    {
        base.initializeModule(idInput, stagingInput, seedInput, originInput);
        for(int i = 0; i < 15; i++)
        {
            units.Add(buildUnit(i, "CentiUnit " + i.ToString()));
        }
        foreach(Unit unit in units)
        {
            unit.controller.createNewNet();
            unit.controller.randomizeSynapses();
            print(unit.controller.relayCatalog.Count.ToString());
        }
    }

    public override Unit buildUnit(int unitId, string unitName)
    {
        Unit unit = base.buildUnit(unitId, unitName);

        PlaneEnvironment environment = createObject(typeof(PlaneEnvironment), unit.gameObject) as PlaneEnvironment;
        Centibot robot = createObject(typeof(Centibot), unit.gameObject) as Centibot;
        Controller controller = createObject(typeof(Controller), unit.gameObject) as Controller;
        IncreaseDistance task = createObject(typeof(IncreaseDistance), unit.gameObject) as IncreaseDistance;
        
        unit.constructUnit(environment, controller, robot, task);

        environment.initializeEnvironment();
        robot.initializeRobot();
        controller.initializeController(unitId, ModuleSeed + unitId);

        // For the initialization of task
        List<GameObject> importantParts = new List<GameObject>();
        importantParts.Add(robot.parts[0]);

        task.initializeTask(importantParts, null, "x+");

        controller.isActive = true;

        return unit;
    }

    public override void run()
    {
        if(rootStaging.time % rootStaging.gg.timeTillActuate == 0)
        {
            foreach(Unit unit in units)
            {
                unit.controller.run();
            }
        }
        if(rootStaging.currentModule == ModuleId)
        {
            rootStaging.FitnessText.text = "Fitness: " + (units[currentUnit].task.fitness/rootStaging.time).ToString();
            rootStaging.ParentFitnessText.text = "ParentFitness: " + (parentFitness / rootStaging.gg.generationTime).ToString();
            rootStaging.GenerationText.text = "Generation: " + generation.ToString();
        }
        if(rootStaging.time % rootStaging.gg.generationTime == 0)
        {
            evolveUnits();
            foreach(Unit unit in units)
            {
                unit.resetUnit();
            }
            rootStaging.time = 0;
            
            print("Reset");
        }
    }
}
