using System.Collections;

using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour {

    /* Environment ------ [Parent Class]
     * The environment is a pretty simple class, it's mostly meant to organize the environment
     * and make it easy to work with
     */

    // Root
    public Module.Unit rootUnit;

    // Existential Components
    public List<GameObject> members;
    public List<Vector3> initialPositions;
    public List<Quaternion> initialRotations;

    private void Awake()
    {
        rootUnit = gameObject.GetComponentInParent<Module.Unit>();
        members = new List<GameObject>();
        initialPositions = new List<Vector3>();
        initialRotations = new List<Quaternion>();
    }

    public virtual void initializeEnvironment()
    {
        /* constructEnvironment ----- function to construct the members of the environment in question
         */ 

        /* ====== For other environments, the system is as follows
         * 
         * <create various members>
         * 
         * setInitialPositions();
         */ 
    }
    public GameObject addMember(GameObject member)
    {
        members.Add(member);
        member.transform.parent = gameObject.transform;
        return member;
    }

    public void setInitialState()
    {
        foreach(GameObject member in members)
        {
            initialPositions.Add(member.transform.position);
            initialRotations.Add(member.transform.rotation);
        }
    }
    public void resetEnvironment()
    {
        for(int i = 0; i < members.Count; i++)
        {
            members[i].transform.position = initialPositions[i];
            members[i].transform.rotation = initialRotations[i];
            if(members[i].GetComponent<Rigidbody>() != null)
            {
                Rigidbody rb = members[i].GetComponent<Rigidbody>();
                rb.velocity = new Vector3(0, 0, 0);
                rb.angularVelocity = new Vector3(0, 0, 0);
            }
        }
    }
    public void clearEnvironment()
    {
        // clearEnvironment ----- on the tin
        foreach (GameObject member in members)
        {
            GameObject.Destroy(member);
        }
        members.Clear();
    }

    public void createPlane(Vector3 planeSize)
    {
        // extremely simple create function to make the plane most environments include
        TinkerBox.Specs spec = new TinkerBox.Specs(rootUnit.origin, new Vector3(0, 0, 0), planeSize);
        GameObject plane = addMember(TinkerBox.createShape(PrimitiveType.Plane, spec, "Plane", Color.white));
        TinkerBox.addMass(plane, 1);
        Rigidbody rb = plane.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
        plane.GetComponent<Renderer>().material = Resources.Load<Material>("CheckerPattern");
        plane.GetComponent<Collider>().material = Resources.Load<PhysicMaterial>("BasicFriction");
        plane.tag = "Plane";
    }
}
