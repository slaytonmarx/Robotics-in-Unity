using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stabilize : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}
