using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Flock : MonoBehaviour
{

    float speed = 0f;
    bool turning = false;
    // Start is called before the first frame update
    void Start()
    {
        
       speed = Random.Range(FlockManager.FM.minSpeed, FlockManager.FM.maxSpeed); 
    }

    void ApplyRules()
    {
        GameObject[] fishes;
        fishes = FlockManager.FM.allFish;

        Vector3 vCenter = Vector3.zero;
        Vector3 vAvoid = Vector3.zero;
        float gSpeed = 0.01f;
        float nDistance;
        int groupSize = 0;

        foreach(GameObject fish in fishes)
        {
            if(fish != this.gameObject)
            {
                nDistance = Vector3.Distance(fish.transform.position, this.transform.position);
                if(nDistance <= FlockManager.FM.neighbourDistance)
                {
                    vCenter += fish.transform.position;
                    groupSize++;

                    if(nDistance < 1f)
                    {
                        vAvoid = vAvoid + (this.transform.position - fish.transform.position);
                    }

                    Flock anotherflock = fish.GetComponent<Flock>();
                    gSpeed = gSpeed + anotherflock.speed;
                }
            }
        }
        if (groupSize > 0)
        {
            vCenter = vCenter / groupSize + (FlockManager.FM.goalPosition - this.transform.position);
            speed = gSpeed / groupSize;
            if(speed > 10)
            {

            }

            Vector3 direction = (vCenter + vAvoid) - transform.position;

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), FlockManager.FM.rotationSpeed * Time.deltaTime);
            }

        }                                                       

    }

    // Update is called once per frame
    void Update()
    {
        Bounds bound = new Bounds(FlockManager.FM.transform.position, FlockManager.FM.swimLimits * 2);

        if(!bound.Contains(transform.position))
        {
            turning = true;
        }
        else
        {
            turning = false;
        }

        if (turning)
        {
            Vector3 direction = FlockManager.FM.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), FlockManager.FM.rotationSpeed * Time.deltaTime);
        }
        else
        {
            if (Random.Range(0f, 100f) < 10)
            {
                speed = Random.Range(FlockManager.FM.minSpeed, FlockManager.FM.maxSpeed);
            }
            if (Random.Range(0f, 100f) < 50)
            {
                ApplyRules();
            }

        }

        this.transform.Translate(0, 0, speed * Time.deltaTime);
    }
}
