using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBallToGoal : Task{

    float previousMag;

    public override void initializeTask(List<GameObject> importantPartsInput, List<GameObject> importantMembersInput, string miscCommand)
    {
        base.initializeTask(importantPartsInput, importantMembersInput, miscCommand);
        previousMag = 1000;
    }

    public override void getFitness()
    {
        float magnitude = (importantMembers[0].transform.position - importantMembers[1].transform.position).magnitude;
        if(magnitude < previousMag)
        {
            fitness += magnitude;
        }
        if(magnitude > previousMag)
        {
            fitness -= magnitude;
        }
        previousMag = magnitude;
    }

}
