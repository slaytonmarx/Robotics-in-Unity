using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relay : MonoBehaviour
{
    public Controller rootNet;
    public HubGlobals hg;
    public GameObject part;
    public int id;
    public float value;
    public virtual void initializeRelay(GameObject partInput, Controller rootInput, string miscCommand)
    {
        rootNet = rootInput;
        hg = rootNet.rootUnit.rootModule.hg;
        part = partInput;
        List<Relay> relayList;
        string typeKey = Controller.getTypeAsString(this);
        if (!rootNet.relayCatalog.ContainsKey(typeKey))
        {
            relayList = new List<Relay>();
            rootNet.relayCatalog.Add(typeKey, relayList);
        }
        relayList = rootNet.relayCatalog[typeKey];
        id = relayList.Count;
        relayList.Add(this);
    }

    public virtual void parseValue()
    {
    }
    public virtual void actuate()
    {

    }

    public virtual string compose()
    {
        string composition = id.ToString() + "     " + Controller.getTypeAsString(this);
        return composition;
    }
}

public class Sensor : Relay
{
    public override string compose()
    {
        return base.compose() + "     Value: " + value.ToString();
    }
}
public class Effector : Relay
{

}

