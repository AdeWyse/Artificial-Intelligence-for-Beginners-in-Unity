using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondsUpdate : MonoBehaviour
{
    float timeStartOffSet = 0;
    bool goStartTime = false;
    public float speed = 2;
    void Update()
    {
        if (!goStartTime)
        {
            timeStartOffSet = Time.realtimeSinceStartup;
            goStartTime = true;
        }
        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, speed * (Time.realtimeSinceStartup - timeStartOffSet));
    }
}
