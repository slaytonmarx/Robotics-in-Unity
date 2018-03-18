using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* A set of tools which meant for ease of coding and to shorten blocks.
 */ 

public class Tools : MonoBehaviour {
    static public GameObject makeObject(string nameInput) {
        GameObject tulpa = new GameObject();
        tulpa.name = nameInput;
        return tulpa;
    }
    static public GameObject makeObject(string nameInput, GameObject parentInput) {
        GameObject tulpa = makeObject(nameInput);
        linkParent(tulpa, parentInput);
        return tulpa;
    }
    static public GameObject makeObject(string nameInput, Vector3 positionInput) {
        GameObject tulpa = makeObject(nameInput);
        setPosition(tulpa, positionInput);
        return tulpa;
    }
    static public GameObject makeObject(string nameInput, GameObject parentInput, Vector3 positionInput) {
        GameObject tulpa = makeObject(nameInput, parentInput);
        setPosition(tulpa, positionInput);
        return tulpa;
    }
    static public GameObject makePrototype(string nameInput, string typeInput) {
        GameObject tulpa = Instantiate(Resources.Load("Prototypes\\" + typeInput + "\\" + nameInput, typeof(GameObject))) as GameObject;
        return tulpa;
    }
    static public GameObject makePrototype(string nameInput, string typeInput, GameObject parentInput) {
        GameObject tulpa = makePrototype(nameInput, typeInput);
        linkParent(tulpa, parentInput);
        return tulpa;
    }
    static public GameObject makePrototype(string nameInput, string typeInput, Vector3 positionInput) {
        GameObject tulpa = makePrototype(nameInput, typeInput);
        setPosition(tulpa, positionInput);
        return tulpa;
    }
    static public GameObject makePrototype(string nameInput, string typeInput, GameObject parentInput, Vector3 positionInput) {
        GameObject tulpa = makePrototype(nameInput, typeInput, parentInput);
        setPosition(tulpa, positionInput);
        return tulpa;
    }
    static public Vector3 makeVector(float x, float y, float z) {
        return new Vector3(x, y, z);
    }
    static public void linkParent(GameObject childInput, GameObject parentInput) {
        childInput.transform.SetParent(parentInput.transform, false);
    }
    static public void setPosition(GameObject objectInput, Vector3 positionInput) {
        objectInput.transform.position = positionInput;
    }

    static public void setText(Text textInput, int intInput) {
        textInput.text = intInput.ToString();
    }
    static public void setText(Text textInput, float floatInput) {
        textInput.text = floatInput.ToString();
    }
    static public void setText(Text textInput, string stringInput) {
        textInput.text = stringInput;
    }
    static public List<T> consolidateLists<T>(List<List<T>> listsArgument) {
        List<T> consolidatedList = new List<T>();
        foreach(List<T> list in listsArgument) {
            consolidatedList.AddRange(list);
        }
        return consolidatedList;
    }
    static public float[] splitTextToValues(string keyString) {
        string[] keys = keyString.Split(' ');
        float[] values = new float[keys.Length];
        for(int i = 0; i < keys.Length; i++) {
            values[i] = float.Parse(keys[i]);
        }
        return values;
    }
}
