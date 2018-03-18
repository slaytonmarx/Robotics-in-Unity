using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : Robot {

    public override void initializeRobot()
    {
        produceBody();
    }
    public override void produceBody()
    {
        TinkerBox.Specs spec = new TinkerBox.Specs(rootUnit.origin + new Vector3(0, .5f, 0), new Vector3(0, 0, 0), new Vector3(.8f, .8f, .8f));
        GameObject floater = TinkerBox.createShape(PrimitiveType.Sphere, spec, "Floater", Color.red);
        TinkerBox.addMass(floater, 5);
        floater.GetComponent<Rigidbody>().useGravity = false;
        parts.Add(floater);
        floater.transform.parent = gameObject.transform;

        pointsensor xGoal = TinkerBox.addRelay(floater, rootUnit.controller, typeof(pointsensor), "x") as pointsensor;
        xGoal.setVoyeuree(rootUnit.environment.members[1]);
        pointsensor zGoal = TinkerBox.addRelay(floater, rootUnit.controller, typeof(pointsensor), "z") as pointsensor;
        zGoal.setVoyeuree(rootUnit.environment.members[1]);
        pointsensor xBall = TinkerBox.addRelay(floater, rootUnit.controller, typeof(pointsensor), "x") as pointsensor;
        xBall.setVoyeuree(rootUnit.environment.members[2]);
        pointsensor zBall = TinkerBox.addRelay(floater, rootUnit.controller, typeof(pointsensor), "z") as pointsensor;
        zBall.setVoyeuree(rootUnit.environment.members[2]);

        TinkerBox.addRelay(floater, rootUnit.controller, typeof(jet), "+x");
        TinkerBox.addRelay(floater, rootUnit.controller, typeof(jet), "-x");
        TinkerBox.addRelay(floater, rootUnit.controller, typeof(jet), "+z");
        TinkerBox.addRelay(floater, rootUnit.controller, typeof(jet), "-z");
        setInitialState();
    }

    private void Update()
    {
        parts[0].transform.rotation = initialRotations[0];
        parts[0].GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
    }
}
