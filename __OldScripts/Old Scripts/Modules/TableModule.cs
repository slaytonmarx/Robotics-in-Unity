using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableModule : Module {

    public override void initializeModule(int idInput, Staging stagingInput, int seedInput, Vector3 originInput)
    {
        base.initializeModule(idInput, stagingInput, seedInput, originInput);
        for(int i = 0; i < 9; i++)
        {
            units.Add(buildUnit(i, "TableTest" + i.ToString()));
        }
        foreach (Unit unit in units)
        {
            unit.controller.createNewNet();
            unit.controller.randomizeSynapses();
        }
    }
    public override Unit buildUnit(int unitId, string unitName)
    {
        Unit unit = base.buildUnit(unitId, unitName);
        

        TableEnvironment environment = createObject(typeof(TableEnvironment), unit.gameObject) as TableEnvironment;
        Telebot robot = createObject(typeof(Telebot), unit.gameObject) as Telebot;
        Controller controller = createObject(typeof(Controller), unit.gameObject) as Controller;
        PlaceCupInCrate task = createObject(typeof(PlaceCupInCrate), unit.gameObject) as PlaceCupInCrate;
        
        unit.constructUnit(environment, controller, robot, task);
  
        environment.initializeEnvironment();
        robot.initializeRobot();
        controller.initializeController(unitId, ag.seed);

        // Task Essentials
        List<GameObject> importantParts = new List<GameObject>();
        importantParts.Add(robot.parts[0]);

        List<GameObject> importantMembers = new List<GameObject>();
        importantMembers.Add(environment.members[6]); // Crate Bottom
        importantMembers.Add(environment.members[11]); // Cup
        importantMembers.Add(environment.members[7]); // The crate sides
        importantMembers.Add(environment.members[8]); // The crate sides
        importantMembers.Add(environment.members[9]); // The crate sides
        importantMembers.Add(environment.members[10]); // The crate sides

        task.initializeTask(importantParts, importantMembers, "");

        controller.isActive = true;

        return unit;
    }

    public override void iterateTask()
    {
        Vector3 cupPlacementVector = TableEnvironment.getCupPosition(moduleRandom); 
        foreach(Unit unit in units)
        {
            TableEnvironment te = unit.environment as TableEnvironment;
            te.placeCup(cupPlacementVector);
            te.initialPositions[11] = te.members[11].transform.position;
        }
    }

    private void Update()
    {
        if (rootStaging.time % rootStaging.gg.timeTillActuate == 0)
        {
            foreach (Unit unit in units)
            {
                unit.controller.run();
            }
        }
        if (rootStaging.currentModule == ModuleId)
        {
            rootStaging.FitnessText.text = "Fitness: " + (units[currentUnit].task.fitness / rootStaging.time).ToString();
            rootStaging.ParentFitnessText.text = "ParentFitness: " + (parentFitness / rootStaging.gg.generationTime).ToString();
            rootStaging.GenerationText.text = "Generation: " + generation.ToString();
        }
        if (rootStaging.time % rootStaging.gg.generationTime == 0)
        {
            evolveUnits();
            foreach (Unit unit in units)
            {
                unit.resetUnit();
            }
            rootStaging.time = 0;

            if (generation != 0 && generation % 50 == 0)
            {
                iterateTask();
            }
        }

        if (generation == 5000)
        {
            units[0].controller.writeNet(parentNet);
        }


        if (Input.GetKeyDown(KeyCode.M))
        {
            iterateTask();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            foreach(Unit unit in units)
            {
                unit.resetUnit();
            }
        }
    }
}
