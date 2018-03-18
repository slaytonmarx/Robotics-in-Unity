using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ControlMovement : MonoBehaviour {

    Rigidbody rb;
    public int speed;

	// Use this for initialization
	void Start () {
        try {
            rb = gameObject.GetComponent<Rigidbody>();
        }
        catch (MissingComponentException) {
            Debug.Log("Start ControlMovement Failure. No rigidbody component.");
        }
	}
	
	// Update is called once per frame
	void Update () {
        rb.velocity = new Vector3(0, 0, 0);
        rb.angularVelocity = Vector3.zero;
        if (Input.GetKey("i")) {
            rb.AddForce(new Vector3(speed, 0, speed));
        }
        if (Input.GetKey("j")) {
            rb.AddForce(new Vector3(-speed, 0, speed));
        }
        if (Input.GetKey("k")) {
            rb.AddForce(new Vector3(-speed, 0, -speed));
        }
        if (Input.GetKey("l")) {
            rb.AddForce(new Vector3(speed, 0, -speed));
        }
        if (Input.GetKey("u")) {
            rb.AddTorque(new Vector3(0, -speed/ 2, 0));
        }
        if (Input.GetKey("o")) {
            rb.AddTorque(new Vector3(0, speed / 2, 0));
        }
    }
}
