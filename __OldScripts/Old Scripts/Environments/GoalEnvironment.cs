using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalEnvironment : Environment {

    public override void initializeEnvironment()
    {
        base.initializeEnvironment();
        createPlane(new Vector3(2, 1, 2));

        TinkerBox.Specs goalSpec = new TinkerBox.Specs(rootUnit.origin, new Vector3(0, 0, 0), new Vector3(.5f, .5f, .5f));
        addMember(TinkerBox.createShape(PrimitiveType.Cube, goalSpec, "Goal", Color.green));
        members[1].GetComponent<Collider>().material = Resources.Load<PhysicMaterial>("BasicFriction");
        members[1].tag = "Goal";

        TinkerBox.Specs ball = new TinkerBox.Specs(rootUnit.origin, new Vector3(0, 0, 0), new Vector3(.5f, .5f, .5f));
        addMember(TinkerBox.createShape(PrimitiveType.Cube, goalSpec, "Moved", Color.blue));
        TinkerBox.addMass(members[2], 1);
        members[2].GetComponent<Collider>().material = Resources.Load<PhysicMaterial>("HighFriction");
        members[2].tag = "Ball";

        setInitialState();
    }

    //====== HELPER FUNCTIONS ======//
    public void setGoalAndBall(Vector3 goalPosition, Vector3 ballPosition)
    {
        members[1].transform.position = goalPosition;
        members[2].transform.position = ballPosition;
        initialPositions[1] = goalPosition;
        initialPositions[2] = ballPosition;
    }
}
