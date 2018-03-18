using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pointsensor : Sensor {

    string direction;
    public GameObject voyeuree;

    public override void initializeRelay(GameObject partInput, Controller rootInput, string miscCommand)
    {
        base.initializeRelay(partInput, rootInput, miscCommand);
        direction = miscCommand;
    }

    public void setVoyeuree(GameObject voy)
    {
        voyeuree = voy;
    }

    // Update is called once per frame
    void Update () {
        value = 0;
        if ((voyeuree.transform.position - part.transform.position).magnitude < 20)
        {
            if (direction == "x")
            {
                value = (voyeuree.transform.position.x - part.transform.position.x);
            }
            if (direction == "z")
            {
                value = (voyeuree.transform.position.z - part.transform.position.z);
            }
        }
    }
}
