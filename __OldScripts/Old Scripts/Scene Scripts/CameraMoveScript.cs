using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveScript : MonoBehaviour {

    /* CameraMoveScript ----- Simple script to allow the camera to be moved during a given scene.
     */ 
    
    public float value = .1f;

	void Update ()
    {
        // Move North
        if (Input.GetKey(KeyCode.Keypad8))
        {
            gameObject.transform.position = gameObject.transform.position + new Vector3(value, 0, 0);
        }

        // Move West
        if (Input.GetKey(KeyCode.Keypad4))
        {
            gameObject.transform.position = gameObject.transform.position + new Vector3(0, 0, value);
        }

        // Move East
        if (Input.GetKey(KeyCode.Keypad6))
        {
            gameObject.transform.position = gameObject.transform.position + new Vector3(0, 0, -value);
        }

        // Move South
        if (Input.GetKey(KeyCode.Keypad2))
        {
            gameObject.transform.position = gameObject.transform.position + new Vector3(-value, 0, 0);
        }

        // Move Up
        if (Input.GetKey(KeyCode.Keypad9))
        {
            gameObject.transform.position = gameObject.transform.position + new Vector3(0, value, 0);
        }

        // Move Down
        if (Input.GetKey(KeyCode.Keypad7))
        {
            gameObject.transform.position = gameObject.transform.position + new Vector3(0, -value, 0);
        }

        // Rotate Up
        if (Input.GetKey(KeyCode.Keypad3))
        {
            gameObject.transform.eulerAngles = gameObject.transform.eulerAngles + new Vector3(value * 8, 0, 0);
        }

        // Rotate Down
        if (Input.GetKey(KeyCode.Keypad1))
        {
            gameObject.transform.eulerAngles = gameObject.transform.eulerAngles + new Vector3(-value * 8, 0, 0);
        }

    }
}
