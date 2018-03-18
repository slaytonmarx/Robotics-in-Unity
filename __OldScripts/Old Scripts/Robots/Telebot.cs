using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telebot : Robot {

    public override void initializeRobot()
    {
        base.initializeRobot();
    }

    public override void produceBody()
    {
        TinkerBox.Specs teleSpec = new TinkerBox.Specs(new Vector3(0, 2, 0) + rootUnit.origin, new Vector3(0, 0, 0), new Vector3(.5f, .5f, .5f));
        GameObject telebot = TinkerBox.createShape(PrimitiveType.Capsule, teleSpec, "Telebot", Color.red);
        parts.Add(telebot);
        telebot.GetComponent<Renderer>().material = Resources.Load<Material>("Metal");
        telebot.transform.parent = gameObject.transform;

        TinkerBox.Specs r1spec = new TinkerBox.Specs(new Vector3(0, 1.75f, 0) + rootUnit.origin, new Vector3(0, 0, 0), new Vector3(.55f, .01f, .55f));
        GameObject r1 = TinkerBox.createShape(PrimitiveType.Cylinder, r1spec, "RingOne", Color.red);
        parts.Add(r1);
        r1.GetComponent<Renderer>().material = Resources.Load<Material>("Gold");
        r1.transform.parent = gameObject.transform;

        TinkerBox.Specs r2spec = new TinkerBox.Specs(new Vector3(0, 2f, 0) + rootUnit.origin, new Vector3(0, 0, 0), new Vector3(.55f, .01f, .55f));
        GameObject r2 = TinkerBox.createShape(PrimitiveType.Cylinder, r2spec, "RingTwo", Color.red);
        parts.Add(r2);
        r2.GetComponent<Renderer>().material = Resources.Load<Material>("Gold");
        r2.transform.parent = gameObject.transform;

        TinkerBox.Specs r3spec = new TinkerBox.Specs(new Vector3(0, 2.25f, 0) + rootUnit.origin, new Vector3(0, 0, 0), new Vector3(.55f, .01f, .55f));
        GameObject r3 = TinkerBox.createShape(PrimitiveType.Cylinder, r3spec, "RingThree", Color.red);
        parts.Add(r1);
        r3.GetComponent<Renderer>().material = Resources.Load<Material>("Gold");
        r3.transform.parent = gameObject.transform;

        pointsensor xCup = TinkerBox.addRelay(rootUnit.environment.members[11], rootUnit.controller, typeof(pointsensor), "x") as pointsensor;
        xCup.setVoyeuree(rootUnit.environment.members[6]);
        pointsensor zCup = TinkerBox.addRelay(rootUnit.environment.members[11], rootUnit.controller, typeof(pointsensor), "z") as pointsensor;
        zCup.setVoyeuree(rootUnit.environment.members[6]);

        TinkerBox.addRelay(rootUnit.environment.members[11], rootUnit.controller, typeof(jet), "x");
        TinkerBox.addRelay(rootUnit.environment.members[11], rootUnit.controller, typeof(jet), "y");
        TinkerBox.addRelay(rootUnit.environment.members[11], rootUnit.controller, typeof(jet), "z");

        setInitialState();
    }
}
