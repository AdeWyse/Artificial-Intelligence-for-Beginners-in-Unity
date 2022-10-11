using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LateUpdateMove : MonoBehaviour
{
    public float speed = 2;

    void LateUpdate()
    {
        this.gameObject.transform.Translate(0, 0, speed * Time.deltaTime);
    }
}
