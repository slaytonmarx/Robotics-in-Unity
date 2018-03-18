using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionSensorPrototype : MonoBehaviour {

    public GameObject affectionObject;
    public float degreeDifference;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        degreeDifference = findDifferenceInDegrees();
	}
    private float findDifferenceInDegrees() {
        Vector3 vObj = gameObject.transform.forward;
        Vector3 vAff = (affectionObject.transform.position - gameObject.transform.position);// - gameObject.transform.position).normalized;
        float angle = Vector3.Angle(vAff, vObj);
        Vector3 cross = Vector3.Cross(vAff, vObj);
        if (cross.y > 0)
            return -angle;
        return angle;
    }
}
