using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Anatomy : Entity {

    // Identifying Properties
    /* Inherited Properties
     * Unit rootUnit
     * string entityName;
     * int entityId;
     * List<GameObject> components;
     * List<ObjectConfiguration> componentConfigurations;
     */
    public GameObject focus;

    public void buildAnatomy(Unit unitInput, string nameInput, int idInput) {
        base.buildEntity(unitInput, nameInput, idInput);
        findRobotFocus();
    }
    public override void reset() {
        base.reset();
    }
    public void findRobotFocus() {
        bool focusFound = false;
        foreach (GameObject obj in components) {
            if (obj.tag == "Focus") {
                focus = obj;
                focusFound = true;
            }
        }
        if (!focusFound)
            Debug.Log("No focus found for " + entityName);
    }
}
