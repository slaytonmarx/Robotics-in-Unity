using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour {

    /* Task ----- [Parent Class]
     * The task class is important for evolution, it contains vital information about which
     * robot is doing well and which robot is doing poorly.
     */

    public Module.Unit rootUnit;

    public List<GameObject> importantParts;
    public List<GameObject> importantMembers;

    public float fitness;

    // Use this for initialization
    void Awake()
    {
        rootUnit = gameObject.GetComponentInParent<Module.Unit>();
        fitness = 0;
    }

    // Update is called once per frame
    void Update()
    {
        getFitness();
    }

    public virtual void initializeTask(List<GameObject> importantPartsInput, List<GameObject> importantMembersInput, string miscCommand)
    {
        /* initializeTask ----- initializeTask takes in the parameters important to the
         * task, these include:
         * 
         * ImportantParts which are from the robot and ImportantMembers which are from the environment
         * There is also a misc modifier which can be used to give special commands to the task
         */
        importantParts = importantPartsInput;
        importantMembers = importantMembersInput;
    }
	public virtual void getFitness()
    {
        /* getFitness ----- possibly the most important function of the task class, getFitness sets
         * the fitness of the task to a given value, depending on the task at hand.
         */
        fitness = 0;
    }
    public void resetTask()
    {
        fitness = 0;
    }
}
