using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubGlobals : MonoBehaviour {

    /* HubGlobals ----- this class's entire purpose is to store the various global variables
     * that need to be called by the controller's RelayHub, these include variables responsible
     *  for the joint limits, velocity, cpg dividers, and a whole mess of other values.
     */

    public float velocity;
    public float force;
    public float floaterForce;
    public float limit;
    public float limitMax;
    public float limitMin;
    public float onsensorValue;
}
