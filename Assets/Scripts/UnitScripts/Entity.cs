using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {
    /* Entity ----- Parent class for both stage, and robot. Meant for anything which has smaller objects in it
     * which must be reset at the end of an experiment. The Entity class can be added to a disembodied object
     * with child objects, or an embodied object without child objects. */

    // PROPERTIES
    protected List<ObjectConfiguration> componentConfigurations;
    public Unit rootUnit;
    public string entityName;
    public int entityId;
    public List<GameObject> components;

    // INITIALIZATION METHODS //

    // Core Methods
    protected void buildEntity(Unit unitInput, string nameInput, int idInput) {
        rootUnit = unitInput;
        entityName = nameInput;
        entityId = idInput;
        components = new List<GameObject>();
        componentConfigurations = new List<ObjectConfiguration>();
        populateComponents();
        populateConfigurations();
    }
    public void updateConfigurations() {
        componentConfigurations.Clear();
        populateConfigurations();
    }
    // Helper Methods
    private void populateComponents() {
        /* populateComponents ----- Populates the components list with the game objects which are child of the
         * attached object, or the attached object itself if it has none. */
        if (gameObject.transform.childCount != 0) {
            Transform[] childTransforms = gameObject.GetComponentsInChildren<Transform>();
            foreach (Transform t in childTransforms) {
                components.Add(t.gameObject);
            }
        }
        else
            components.Add(gameObject);
    }
    private void populateConfigurations() {
        Rigidbody rb;
        foreach (GameObject obj in components) {
            rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
                componentConfigurations.Add(new ObjectConfiguration(obj));
        }
    }

    // ACTIVE METHODS //

    // Core Methods
    public virtual void reset() {
        foreach (ObjectConfiguration objCon in componentConfigurations) {
            objCon.resetObject();
        }
    }

    // OBJECTCONFIGURATION CLASS //
    protected class ObjectConfiguration {

        // PROPERTIES //

        // Object Properties
        public GameObject savedObject;
        Vector3 objectPosition;
        Vector3 objectRotation;

        // CONSTRUCTOR //
        public ObjectConfiguration(GameObject objectInput) {
            savedObject = objectInput;
            objectPosition = objectInput.transform.position;
            objectRotation = objectInput.transform.eulerAngles;
        }
        // ACTIVE METHODS //
        public void resetObject() {
            /* resetObject ----- resets the object to it's initial position, rotation and velocities*/
            savedObject.transform.position = objectPosition;
            savedObject.transform.eulerAngles = objectRotation;
            Rigidbody rb = savedObject.GetComponent<Rigidbody>();
            rb.velocity = new Vector3(0, 0, 0);
            rb.angularVelocity = new Vector3(0, 0, 0);
        }

        // STANDARD GETTERS //
        public Vector3 getPosition() {
            return objectPosition;
        }
        public Vector3 getRotation() {
            return objectRotation;
        }

        // STANDARD SETTERS //
        public void setPosition(Vector3 positionInput) {
            objectPosition = positionInput;
        }
    }
}
