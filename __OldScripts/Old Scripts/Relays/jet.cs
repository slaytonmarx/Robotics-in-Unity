using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jet : Effector {

    string direction;
    float jetForce;

    public override void initializeRelay(GameObject part, Controller rootInput, string miscCommand)
    {
        base.initializeRelay(part, rootInput, miscCommand);
        hg = rootNet.rootUnit.rootModule.hg;

        direction = miscCommand;
    }
    public override void parseValue()
    {
        jetForce = value * hg.floaterForce;
    }
    public override void actuate()
    {
        if (rootNet.isActive)
        {
            parseValue();
            Vector3 throttle = new Vector3(0,0,0);
            if(direction == "x")
            {
                throttle = new Vector3(jetForce, 0, 0);
            }
            if (direction == "y")
            {
                throttle = new Vector3(0, jetForce, 0);
            }
            if (direction == "z")
            {
                throttle = new Vector3(0, 0, -jetForce);
            }
            gameObject.GetComponent<Rigidbody>().AddRelativeForce(throttle);
        }
    }
    public override string compose()
    {
        string composition = base.compose();
        return composition + "    Part: " + part.name + "     Power: " + jetForce.ToString();
    }

    void Update()
    {
        actuate();
    }
}
