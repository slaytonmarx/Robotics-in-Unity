using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onsensor : Sensor {

    private void Update()
    {
        value = hg.onsensorValue;
    }
}
