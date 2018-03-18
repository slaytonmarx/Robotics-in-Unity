using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPositionStage : Stage {

    // PROPERTIES //

    protected int jumps;
    protected ObjectConfiguration goalConfiguration;
    public bool randomNewPosition;
    public int generationsToChange;
    public int degreesOfChange;
    public float magnitude;
    public override void buildStage(Unit unitInput, string nameInput, int idInput) {
        base.buildStage(unitInput, nameInput, idInput);
        foreach (ObjectConfiguration objCon in componentConfigurations) {
            if (objCon.savedObject.tag == "Goal") {
                goalConfiguration = objCon;
            }
        }
        if (goalConfiguration == null) {
            Debug.Log("No goal object found for random position stage");
        }
        Vector3 positionTemp = goalConfiguration.savedObject.transform.position - gameObject.transform.position;
        magnitude = Mathf.Sqrt(Mathf.Pow(positionTemp.x, 2) + Mathf.Pow(positionTemp.z, 2));
        jumps = 1;
    }
    public void reset(int val) {
        if (val % generationsToChange == 0) {
            float angle = degreesOfChange * Mathf.PI / 180 * jumps;
            if (randomNewPosition)
                angle = Random.Range(0, 2 * Mathf.PI);
            goalConfiguration.setPosition(new Vector3(Mathf.Cos(angle) * magnitude, goalConfiguration.getPosition().y - gameObject.transform.position.y, Mathf.Sin(angle) * magnitude) + gameObject.transform.position);
            jumps++;
        }
        base.reset();
    }
}
