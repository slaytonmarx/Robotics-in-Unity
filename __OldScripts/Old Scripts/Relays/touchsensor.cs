using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class touchsensor : Sensor {

    public bool isTouching;
    private string touchTag;

    public override void initializeRelay(GameObject partInput, Controller rootInput, string miscCommand)
    {
        base.initializeRelay(partInput, rootInput, miscCommand);
        touchTag = miscCommand;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == touchTag)
        {
            isTouching = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == touchTag)
        {
            isTouching = false;
        }
    }

    public override void parseValue()
    {
        if(isTouching)
        {
            value = 1;
        }
        else
        {
            value = 0;
        }
    }

    void Update () {
        parseValue();
	}
}
