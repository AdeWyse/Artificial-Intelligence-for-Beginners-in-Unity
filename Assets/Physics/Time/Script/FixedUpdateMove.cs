using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedUpdateMove : MonoBehaviour
{
    public float speed = 2;

    void FixedUpdate()
    {
        this.gameObject.transform.Translate(0, 0, speed *  Time.deltaTime);
    }
}
