using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Centibot : Robot {
   
    public override void initializeRobot()
    {
        produceBody();
    }
    public override void produceBody()
    {
        float max = rootUnit.rootModule.hg.limitMax;
        float min = rootUnit.rootModule.hg.limitMin;

        int segments = 3;
        List<GameObject> segmentsList = new List<GameObject>();
        for(int i = 0; i < segments; i++)
        {
            if(segmentsList.Count == 0)
            {
                GameObject bodyHead = createSegment(rootUnit.origin + new Vector3(i, 0, 0), 0, max, min);
                GameObject.Destroy(bodyHead.GetComponent<HingeJoint>());
                segmentsList.Add(bodyHead);
            }
            else
            {
                GameObject bodyOne = createSegment(rootUnit.origin + new Vector3(i * -.6f, 0, 0), 0, max, min);
                TinkerBox.createHinge(bodyOne, segmentsList[segmentsList.Count-1], new Vector3(.6f, 0, 0), new Vector3(-.6f, 0, 0), new Vector3(0, 0, 1));
                TinkerBox.addLimitsToHinge(bodyOne, max, min);
                TinkerBox.addRelay(bodyOne, rootUnit.controller, typeof(motor), "counter");
                segmentsList.Add(bodyOne);
            }
        }
        addToParent();
        setInitialState();
    }
    
    //====== HELPER FUNCTIONS ======//
    public GameObject createSegment(Vector3 positionInput, int index, float max, float min)
    {
        // Main Body
        TinkerBox.Specs shape = new TinkerBox.Specs(new Vector3(0, .5f, 0) + positionInput, new Vector3(0, 0, 0), new Vector3(.5f, .2f, .3f));
        GameObject model = TinkerBox.createShape(PrimitiveType.Cube, shape, "centibody" + rootUnit.id.ToString(), Color.red);
        TinkerBox.addMass(model, 1);
        parts.Add(model);

        // Leg 1
        shape = new TinkerBox.Specs(new Vector3(0, .5f, -.35f) + positionInput, new Vector3(90, 0, 0), new Vector3(.1f, .2f, .1f));
        GameObject up = TinkerBox.createShape(PrimitiveType.Cylinder, shape, "Left High" + rootUnit.id.ToString(), Color.yellow);
        TinkerBox.addMass(up, 1);
        parts.Add(up);

        shape = new TinkerBox.Specs(new Vector3(0, .25f, -.55f) + positionInput, new Vector3(0, 0, 0), new Vector3(.1f, .25f, .1f));
        GameObject down = TinkerBox.createShape(PrimitiveType.Cylinder, shape, "Left Low" + rootUnit.id.ToString(), Color.yellow);
        TinkerBox.addMass(down, 1);
        parts.Add(down);

        TinkerBox.createHinge(up, model, new Vector3(0, 1, 0), new Vector3(0, 0, -.5f), new Vector3(1, 0, 0));
        TinkerBox.createHinge(down, up, new Vector3(0, 1, 0), new Vector3(0, -1, 0), new Vector3(1,0,0));
        TinkerBox.addLimitsToHinge(up, max, min);
        TinkerBox.addLimitsToHinge(down, max, min);

        TinkerBox.addRelay(up, rootUnit.controller, typeof(motor), "");
        TinkerBox.addRelay(down, rootUnit.controller, typeof(motor), "");
        TinkerBox.addRelay(down, rootUnit.controller, typeof(touchsensor), "Plane");
        down.tag = "Lower";

        // Leg 2       
        shape = new TinkerBox.Specs(new Vector3(0, .5f, .35f) + positionInput, new Vector3(90, 0, 0), new Vector3(.1f, .2f, .1f));
        GameObject up2 = TinkerBox.createShape(PrimitiveType.Cylinder, shape, "Right High" + rootUnit.id.ToString(), Color.yellow);
        TinkerBox.addMass(up2, 1);
        parts.Add(up2);

        shape = new TinkerBox.Specs(new Vector3(0, .25f, .55f) + positionInput, new Vector3(0, 0, 0), new Vector3(.1f, .25f, .1f));
        GameObject down2 = TinkerBox.createShape(PrimitiveType.Cylinder, shape, "Right Low" + rootUnit.id.ToString(), Color.yellow);
        TinkerBox.addMass(down2, 1);
        parts.Add(down2);

        TinkerBox.createHinge(up2, model, new Vector3(0, -1, 0), new Vector3(0, 0, .5f), new Vector3(1, 0, 0));
        TinkerBox.createHinge(down2, up2, new Vector3(0, 1, 0), new Vector3(0, 1, 0), new Vector3(1, 0, 0));
        TinkerBox.addLimitsToHinge(up2, max, min);
        TinkerBox.addLimitsToHinge(down2, max, min);

        TinkerBox.addRelay(up2, rootUnit.controller, typeof(motor), "negative");
        TinkerBox.addRelay(down2, rootUnit.controller, typeof(motor), "negative");
        TinkerBox.addRelay(down2, rootUnit.controller, typeof(touchsensor), "Plane");
        down.tag = "Lower";

        return model;
    }
    
    public void addToParent()
    {
        foreach(GameObject ob in parts)
        {
            ob.transform.parent = gameObject.transform;
        }
    }
}
