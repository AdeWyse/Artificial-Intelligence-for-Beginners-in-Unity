using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnner : MonoBehaviour
{
    public GameObject patientPrefab;
    public int numPatient;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnPatient", 0f, Random.Range(10f,20f));
    }

    void SpawnPatient()
    {
        Instantiate(patientPrefab, this.gameObject.transform.position, Quaternion.identity);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
