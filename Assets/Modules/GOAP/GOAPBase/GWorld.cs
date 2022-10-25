using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GWorld
{
    private static readonly GWorld instance = new GWorld();
    private static WorldStates world;

    private static Queue<GameObject> patients;
    private static Queue<GameObject> cubicles;

    static GWorld()
    {
        world = new WorldStates();
        patients = new Queue<GameObject>();
       cubicles = new Queue<GameObject>();
        GameObject[] initialCubicles = GameObject.FindGameObjectsWithTag("Cubicle");
        foreach(GameObject cubicle in initialCubicles)
        {
            cubicles.Enqueue(cubicle);
        }
        if(initialCubicles.Length > 0)
        {
            world.ModifyState("FreeCubicle", initialCubicles.Length);
        }

        Time.timeScale = 5;
    }
    private GWorld()
    {
    }

    public void AddPatient(GameObject patient)
    {
        patients.Enqueue(patient);
    }

    public GameObject RemovePatient()
    {
        if(patients.Count == 0)
        {
            return null;
        }
        return patients.Dequeue();
    }

    public void AddCubicle(GameObject cubicle)
    {
        cubicles.Enqueue(cubicle);
    }

    public GameObject RemoveCubicle()
    {
        if (cubicles.Count == 0)
        {
            return null;
        }
        return cubicles.Dequeue();
    }

    public static GWorld Instance
    {
        get { return instance; }
    }

    public WorldStates GetWorld()
    {
        return world;
    }
}
