using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalModule : Module {

    Vector3 goalPosition;
    Vector3 ballPosition;
    System.Random random;

    public override void initializeModule(int idInput, Staging stagingInput, int seedInput, Vector3 originInput)
    {
        base.initializeModule(idInput, stagingInput, seedInput, originInput);
        random = new System.Random(seedInput);
        for (int i = 0; i < 9; i++)
        {
            units.Add(buildUnit(i, "GoalUnit " + i.ToString()));
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

        GoalEnvironment environment = createObject(typeof(GoalEnvironment), unit.gameObject) as GoalEnvironment;
        Floater robot = createObject(typeof(Floater), unit.gameObject) as Floater;
        Controller controller = createObject(typeof(Controller), unit.gameObject) as Controller;
        MoveBallToGoal task = createObject(typeof(MoveBallToGoal), unit.gameObject) as MoveBallToGoal;

        unit.constructUnit(environment, controller, robot, task);

        environment.initializeEnvironment();
        environment.setGoalAndBall(unit.origin + new Vector3(-6, .5f, 0), unit.origin + new Vector3(-2, .5f, 1));
        robot.initializeRobot();
        controller.initializeController(unitId, ModuleSeed + unitId);

        // For the initialization of task
        List<GameObject> importantMembers = new List<GameObject>();
        importantMembers.Add(environment.members[1]);
        importantMembers.Add(environment.members[2]);

        task.initializeTask(null, importantMembers, "");

        controller.isActive = true;

        return unit;
    }

    public override void run()
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
            print("Reset");
        }
        if(generation != 0 && generation % 20 == 0)
        {
            int goalx = random.Next(0, 6);
            int goalz = random.Next(0, 6);
            goalPosition = new Vector3(goalx, .5f, goalz);

            int ballx = random.Next(0, 6);
            int ballz = random.Next(0, 6);
            ballPosition = new Vector3(ballx, .5f, ballz);

            parentFitness = -5000;

            foreach(Unit unit in units)
            {
                unit.environment.initialPositions[1] = goalPosition + unit.origin;
                unit.environment.initialPositions[2] = ballPosition + unit.origin;
            }
        }
        if(generation == 500)
        {
            units[0].controller.writeNet(parentNet);
        }
    }
}
