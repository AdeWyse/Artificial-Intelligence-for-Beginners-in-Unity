using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public static FlockManager FM;
    public GameObject fishPrefab;
    public int numFish = 20;
    public GameObject[] allFish;
    public Vector3 swimLimits = new Vector3(5f, 5f, 5f);
    public Vector3 goalPosition = Vector3.zero;


    [Header("Fish Settings")]
    [Range(0f, 5f)] public float minSpeed;
    [Range(0f,5f)] public float maxSpeed;
    [Range(1f, 10f)] public float neighbourDistance;
    [Range(1f, 5f)] public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        allFish = new GameObject[numFish];
        for(int i = 0; i < numFish; i++)
        {
            Vector3 position = this.transform.position + new Vector3(Random.Range(-swimLimits.x, swimLimits.x), Random.Range(-swimLimits.y, swimLimits.y), Random.Range(-swimLimits.z, swimLimits.z));
            allFish[i] = Instantiate(fishPrefab, position, Quaternion.identity);
        }

        FM = this;
        goalPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Random.RandomRange(0f, 100f) < 10)
        {
            goalPosition = this.transform.position + new Vector3(Random.Range(-swimLimits.x, swimLimits.x), Random.Range(-swimLimits.y, swimLimits.y), Random.Range(-swimLimits.z, swimLimits.z));
            goalPosition = this.transform.position + new Vector3(Random.Range(-swimLimits.x, swimLimits.x), Random.Range(-swimLimits.y, swimLimits.y), Random.Range(-swimLimits.z, swimLimits.z));
        }
    }
}
