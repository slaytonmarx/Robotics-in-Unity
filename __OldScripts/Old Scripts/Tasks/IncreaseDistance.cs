using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseDistance : Task {

    /* IncreaseDistance ----- Increase distance takes the first part from the list of
     * important parts and checks how far it's gotten in a certain direction (given
     * by the misc command)
     */

    private string direction;
    private float originX; // If there's an offset with the origin.
    private GameObject part;

    public override void initializeTask(List<GameObject> importantPartsInput, List<GameObject> importantMembersInput, string miscCommand)
    {
        base.initializeTask(importantPartsInput, importantMembersInput, miscCommand);
        direction = miscCommand;
        originX = rootUnit.origin.x;
        part = importantParts[0];
    }

    public override void getFitness()
    {
        fitness += part.transform.position.x - originX;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}
}
