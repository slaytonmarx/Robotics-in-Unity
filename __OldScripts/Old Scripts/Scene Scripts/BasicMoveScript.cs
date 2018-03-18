using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMoveScript : MonoBehaviour {

    /* BasicMoveScript ----- extremely simple script to test the movement and lighting in the workshop.
     * Allows an object to be minipulated with wasdqe to see how lighting works.
     */ 

    Rigidbody rb;
    public int value;
    
	void Start () {
        rb = gameObject.GetComponent<Rigidbody>();
        if(rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.mass = 100;
        rb.useGravity = false;
    }
	
	// Update is called once per frame
	void Update () {
        // Move North
        rb.velocity = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(new Vector3(value, 0, 0));
        }
        // Move West
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(new Vector3(0, 0, value));
        }
        // Move East
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(new Vector3(0, 0, -value));
        }
        // Move South
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(new Vector3(-value, 0, 0));
        }
        // Move Up
        if (Input.GetKey(KeyCode.E))
        {
            rb.AddForce(new Vector3(0, value, 0));
        }
        // Move Down
        if (Input.GetKey(KeyCode.Q))
        {
            rb.AddForce(new Vector3(0, -value, 0));
        }
    }
}
