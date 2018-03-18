using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TinkerBox : MonoBehaviour {
    /* TINKERBOX ----- the tinker box is a static class which I use to easily create
         * my robots.
         */
    public static GameObject createShape(PrimitiveType type, Specs shape, string name, Color color)
    {
        // createShape ----- on the tin
        GameObject ob = GameObject.CreatePrimitive(type);
        ob.name = name;
        ob.transform.position = shape.position;
        ob.transform.eulerAngles = shape.rotation;
        ob.transform.localScale = shape.scale;
        ob.GetComponent<Renderer>().material.color = color;
        return ob;
    }
    public static void addMass(GameObject ob, float mass)
    {
        Rigidbody rb = ob.AddComponent<Rigidbody>();
        rb.mass = mass;
        Collider col = ob.GetComponent<Collider>();
        col.material = Resources.Load<PhysicMaterial>("BasicFriction");
    }
    public static void createHinge(GameObject parentObject, GameObject childObject, Vector3 anchorParent, Vector3 anchorChild, Vector3 axis)
    {
        // createHinge ----- on the tin
        HingeJoint hinge = parentObject.AddComponent<HingeJoint>();
        hinge.connectedBody = childObject.GetComponent<Rigidbody>();
        hinge.autoConfigureConnectedAnchor = false;
        hinge.anchor = anchorParent;
        hinge.connectedAnchor = anchorChild;
        hinge.axis = axis;

        Specs shape = new Specs(new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(.09f, .09f, .09f));
        GameObject jointOb = createShape(PrimitiveType.Sphere, shape, "JointMarker" + parentObject.name + childObject.name, Color.blue);
        jointOb.transform.parent = parentObject.transform;
        jointOb.transform.localPosition = new Vector3(0, 0, 0) + anchorParent;
        jointOb.GetComponent<Collider>().isTrigger = true;
        GameObject.Destroy(jointOb.GetComponent<Rigidbody>());
    }
    public static void addLimitsToHinge(GameObject ob, float max, float min)
    {
        HingeJoint hinge = ob.GetComponent<HingeJoint>();
        JointLimits lim = new JointLimits();
        lim.max = max;
        lim.min = min;
        lim.bounceMinVelocity = 0;
        lim.bounciness = 0;
        hinge.limits = lim;
        hinge.useLimits = true;
    }
    
    public static Relay addRelay(GameObject part, Controller controllerInput, System.Type type, string miscCommand)
    {
        Relay relay = part.AddComponent(type) as Relay;
        if(relay == null)
        {
            print("Relay is null, there is no relay of type: " + type.ToString());
        }
        relay.initializeRelay(part, controllerInput, miscCommand);
        return relay;
    }

    public class Specs
    {
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;
        public Specs(Vector3 positionInput, Vector3 rotationInput, Vector3 scaleInput)
        {
            position = positionInput;
            rotation = rotationInput;
            scale = scaleInput;
        }
    }
}
