using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositiveCollision : MonoBehaviour {

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Goal")
        {
            gameObject.transform.parent.gameObject.GetComponent<TableEnvironment>().rootUnit.task.fitness += 500;
        }
    }
}
