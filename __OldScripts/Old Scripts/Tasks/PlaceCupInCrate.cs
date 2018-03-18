using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceCupInCrate : Task {

    public override void getFitness()
    {
        float diff = System.Math.Abs(rootUnit.environment.members[6].transform.position.x - rootUnit.environment.members[11].transform.position.x) + System.Math.Abs(rootUnit.environment.members[6].transform.position.y - rootUnit.environment.members[11].transform.position.y) + System.Math.Abs(rootUnit.environment.members[6].transform.position.z - rootUnit.environment.members[11].transform.position.z);
        fitness -= diff;
    }
}
