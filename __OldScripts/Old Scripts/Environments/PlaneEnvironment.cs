using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneEnvironment : Environment {

    /* PlaneEnvironment ----- A simple environment with a wide flat plane and
     * no other members
     */ 

    public override void initializeEnvironment()
    {
        createPlane(new Vector3(2, 1, 2));

        setInitialState();
    }
}
