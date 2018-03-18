using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableEnvironment : Environment {

    /* TableEnvironment ----- The table environment is the environment for a sub selection
     * of robots and tasks which seeks to move an object (a cylindrical "cup") into a box
     * placed somewhere in the area. The box can be an set of dimensions one could choose
     * and the cup can be different places on the table.
     */

    Vector3 tableOriginGlobal;

    List<GameObject> tableObjects;
    List<GameObject> crateObjects;
    GameObject cup;

    public override void initializeEnvironment()
    {
        tableObjects = new List<GameObject>();
        crateObjects = new List<GameObject>();

        // Create the plane everything takes place on
        createPlane(new Vector3(2, 1, 2));

        // Creates the table, cup, and crate, setting their positions as well
        tableOriginGlobal = new Vector3(-1.5f, 0, 0) + rootUnit.origin;
        Vector3 crateOrigin = new Vector3(2, 0, 0) + rootUnit.origin;
        Vector3 crateDimensions = new Vector3(1, .25f, 1);
        createTable(tableOriginGlobal);
        createCrate(crateOrigin, crateDimensions);
        createCup(tableOriginGlobal);
        
        setInitialState();

        /* NOTE:
         * cup index is 11
         * the bottom's id is 6
         * and the sides are 7, 8, 9, and 10
         */
    }

    //====== HELPER FUNCTIONS ======//
    public void createTable(Vector3 tableOrigin)
    {
        // Creates the table at the origin specified

        // Create Table Top
        TinkerBox.Specs topSpec = new TinkerBox.Specs(new Vector3(0, 1, 0) + tableOrigin, new Vector3(0, 0, 0), new Vector3(1, .1f, 2));
        GameObject tableTop = addMember(TinkerBox.createShape(PrimitiveType.Cube, topSpec, "tableTop", Color.white));
        tableObjects.Add(tableTop);
        tableTop.GetComponent<Renderer>().material = Resources.Load<Material>("DarkWood");

        // Create the legs
        TinkerBox.Specs oneSpec = new TinkerBox.Specs(new Vector3(.4f, .5f, .94f) + tableOrigin, new Vector3(0, 0, 0), new Vector3(.1f, 1, .1f));
        GameObject legOne = addMember(TinkerBox.createShape(PrimitiveType.Cube, oneSpec, "legOne", Color.white));
        tableObjects.Add(legOne);
        legOne.GetComponent<Renderer>().material = Resources.Load<Material>("LightWood");
        
        TinkerBox.Specs twoSpec = new TinkerBox.Specs(new Vector3(.4f, .5f, -.94f) + tableOrigin, new Vector3(0, 0, 0), new Vector3(.1f, 1, .1f));
        GameObject legTwo = addMember(TinkerBox.createShape(PrimitiveType.Cube, twoSpec, "legTwo", Color.white));
        tableObjects.Add(legTwo);
        legTwo.GetComponent<Renderer>().material = Resources.Load<Material>("LightWood");
        
        TinkerBox.Specs threeSpec = new TinkerBox.Specs(new Vector3(-.4f, .5f, .94f) + tableOrigin, new Vector3(0, 0, 0), new Vector3(.1f, 1, .1f));
        GameObject legThree = addMember(TinkerBox.createShape(PrimitiveType.Cube, threeSpec, "legThree", Color.white));
        tableObjects.Add(legThree);
        legThree.GetComponent<Renderer>().material = Resources.Load<Material>("LightWood");
        
        TinkerBox.Specs fourSpec = new TinkerBox.Specs(new Vector3(-.4f, .5f, -.94f) + tableOrigin, new Vector3(0, 0, 0), new Vector3(.1f, 1, .1f));
        GameObject legFour = addMember(TinkerBox.createShape(PrimitiveType.Cube, fourSpec, "legFour", Color.white));
        tableObjects.Add(legFour);
        legFour.GetComponent<Renderer>().material = Resources.Load<Material>("LightWood");
    }
    public void createCup(Vector3 tableOrigin)
    {
        // Creates the cup
        TinkerBox.Specs cupSpec = new TinkerBox.Specs(new Vector3(0, 1, 0) + tableOrigin, new Vector3(0, 0, 0), new Vector3(.1f, .1f, .1f));
        cup = addMember(TinkerBox.createShape(PrimitiveType.Cylinder, cupSpec, "cup", Color.blue));
        TinkerBox.addMass(cup, 1);
        cup.tag = "Ball";
        cup.AddComponent<PositiveCollision>();
        cup.transform.position = new Vector3(0, 1.15f, 0) + tableOrigin;
        Rigidbody rb = cup.GetComponent<Rigidbody>();
        //rb.isKinematic = true;
        rb.useGravity = false;
    }
    public void createCrate(Vector3 crateOrigin, Vector3 crateDimensions)
    {
        // Creates the crate at the origin specified in the dimensions specified

        // Creates crate bottom
        TinkerBox.Specs bottomSpec = new TinkerBox.Specs(new Vector3(0, .05f, 0) + crateOrigin, new Vector3(0, 0, 0), new Vector3(crateDimensions.x-.05f, .1f, crateDimensions.z - .05f));
        GameObject crateBottom = addMember(TinkerBox.createShape(PrimitiveType.Cube, bottomSpec, "crateBottom", Color.blue));
        crateObjects.Add(crateBottom);
        crateBottom.GetComponent<Renderer>().material = Resources.Load<Material>("DarkWood");
        crateBottom.tag = "Goal";

        // Creates the sides
        TinkerBox.Specs oneSpec = new TinkerBox.Specs(new Vector3(-(crateDimensions.x/2f), .25f, 0) + crateOrigin, new Vector3(0, 0, 0), new Vector3(.05f, .5f, crateDimensions.z + .05f));
        GameObject sideOne = addMember(TinkerBox.createShape(PrimitiveType.Cube, oneSpec, "sideOne", Color.white));
        crateObjects.Add(sideOne);
        sideOne.GetComponent<Renderer>().material = Resources.Load<Material>("LightWood");
        sideOne.tag = "NotGoal";

        TinkerBox.Specs twoSpec = new TinkerBox.Specs(new Vector3((crateDimensions.x / 2f), .25f, 0) + crateOrigin, new Vector3(0, 0, 0), new Vector3(.05f, .5f, crateDimensions.z + .05f));
        GameObject sideTwo = addMember(TinkerBox.createShape(PrimitiveType.Cube, twoSpec, "sideTwo", Color.white));
        crateObjects.Add(sideTwo);
        sideTwo.GetComponent<Renderer>().material = Resources.Load<Material>("LightWood");
        sideTwo.tag = "NotGoal";

        TinkerBox.Specs threeSpec = new TinkerBox.Specs(new Vector3(0, .25f, -(crateDimensions.z / 2f)) + crateOrigin, new Vector3(0, 0, 0), new Vector3(crateDimensions.x -.05f, .5f, .05f));
        GameObject sideThree = addMember(TinkerBox.createShape(PrimitiveType.Cube, threeSpec, "sideThree", Color.white));
        crateObjects.Add(sideThree);
        sideThree.GetComponent<Renderer>().material = Resources.Load<Material>("LightWood");
        sideThree.tag = "NotGoal";

        TinkerBox.Specs fourSpec = new TinkerBox.Specs(new Vector3(0, .25f, (crateDimensions.z / 2f)) + crateOrigin, new Vector3(0, 0, 0), new Vector3(crateDimensions.x - .05f, .5f, .05f));
        GameObject sideFour = addMember(TinkerBox.createShape(PrimitiveType.Cube, fourSpec, "sideFour", Color.white));
        crateObjects.Add(sideFour);
        sideFour.GetComponent<Renderer>().material = Resources.Load<Material>("LightWood");
        sideFour.tag = "NotGoal";
    }
    public void placeCup(Vector3 cupPlace)
    {
        cup.transform.position = cupPlace + tableOriginGlobal;
    }
    
    public static Vector3 getCupPosition(System.Random cupRandom)
    {
        float cupXPosition = cupRandom.Next(-45, 45) / 100f;
        float cupZPosition = cupRandom.Next(-95, 95) / 100f;
        return new Vector3(cupXPosition, 1.15f, cupZPosition);
    }

    private void Update()
    {
        cup.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
    }
}
