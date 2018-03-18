using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : Entity {
    /* Stage ----- Centralized container class. Attached to a disembodied gameobject which contains all parts
     * of the stage. Can have certain rules applied to it, such as "robot cannot leave the area" or "more than
     * one robot." */

    // PROPERTIES //

    // Identifying Properties
    /* Inherited Properties
     * Unit rootUnit
     * string entityName;
     * int entityId;
     * List<GameObject> components;
     * List<ObjectConfiguration> componentConfigurations;
     */

    public Vector3 position;
    public Vector3 fixedCameraPosition;
    public int[] size;

    /* Inherited Methods of Import
    * void resetConfigurations()
    */

    public virtual void buildStage(Unit unitInput, string nameInput, int idInput) {
        base.buildEntity(unitInput, nameInput, idInput);
        if (size.Length == 0) {
            size = new int[] { 50, 50 };
            Debug.Log("No Unit Size Set, Setting to Default of 50x50");
        }
    }
    public virtual void buildStage(Unit unitInput, string nameInput, int idInput, Vector3 positionInput) {
        buildStage(unitInput, nameInput, idInput);

        position = positionInput + new Vector3(positionInput.x + (size[0] + 5) * (idInput % 3), 0, positionInput.z + (size[1] + 5) * Mathf.Floor(idInput / 3.0f));
        fixedCameraPosition = positionInput + new Vector3(-size[0] / 2, (size[0] + size[1]) / 5, -size[1] / 2);
        rootUnit.gameObject.transform.position = position;
        updateConfigurations();
    }
    void Start () {
		
	}
	void Update () {
		
	}
}
