using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class motor : Effector
{
    /* motor ----- relay which connects a part's hinge joint to the relay hub
     */

    public bool isActuating;
    private float max;
    private float min;

    public float currentAngle;

    public float angleToActuate;
    public float difference;
    public float velocity;

    // Hinge Components
    private HingeJoint joint;
    private JointMotor jointMotor;

    public override void initializeRelay(GameObject part, Controller rootInput, string miscCommand)
    {
        base.initializeRelay(part, rootInput, miscCommand);
        hg = rootNet.rootUnit.rootModule.hg;

        joint = part.GetComponent<HingeJoint>();
        joint.enablePreprocessing = true;
        jointMotor = joint.motor;
        jointMotor.force = hg.force;
        joint.useMotor = true;
        angleToActuate = 0;
        jointMotor.freeSpin = true;
        max = joint.limits.max;
        min = joint.limits.min;

        if (miscCommand == "negative")
        {
            joint.axis = -joint.axis;
        }
    }
    public override void parseValue()
    {
        if (value < 0)
        {
            angleToActuate = value * min;
        }
        else
        {
            angleToActuate = value * max;
        }
    }
    public override void actuate()
    {
        if (rootNet.isActive)
        {
            parseValue();
            joint.motor = jointMotor;
            currentAngle = joint.angle;
            difference = angleToActuate - (currentAngle);
            velocity = difference * hg.velocity;
            jointMotor.targetVelocity = velocity;
        }
    }
    public override string compose()
    {
        string composition = base.compose();
        return composition + "    Part: " + part.name + "     Angle: " + angleToActuate.ToString();
    }
    
    void Update()
    {
        actuate();
    }
}
