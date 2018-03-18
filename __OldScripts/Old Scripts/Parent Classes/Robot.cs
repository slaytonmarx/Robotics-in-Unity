using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour {

    // Root
    public Module.Unit rootUnit;

    // Existential Components
    public List<GameObject> parts;
    public List<Vector3> initialPositions;
    public List<Quaternion> initialRotations;

    public void Awake()
    {
        rootUnit = gameObject.GetComponentInParent<Module.Unit>();
        parts = new List<GameObject>();
        initialPositions = new List<Vector3>();
        initialRotations = new List<Quaternion>();
    }

    public virtual void initializeRobot()
    {
        produceBody();
    }
	public virtual void produceBody()
    {
        // Must have set initial state at the end
    }

    public void setInitialState()
    {
        foreach (GameObject part in parts)
        {
            initialPositions.Add(part.transform.position);
            initialRotations.Add(part.transform.rotation);
        }
    }
    public void resetRobot()
    {
        for (int i = 0; i < parts.Count; i++)
        {
            parts[i].transform.position = initialPositions[i];
            parts[i].transform.rotation = initialRotations[i];
            if (parts[i].GetComponent<Rigidbody>() != null)
            {
                Rigidbody rb = parts[i].GetComponent<Rigidbody>();
                rb.velocity = new Vector3(0, 0, 0);
            }
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

}
