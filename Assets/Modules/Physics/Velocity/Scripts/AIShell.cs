using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShell : MonoBehaviour {

    Rigidbody rb;

    public GameObject explosion;
    

    void OnCollisionEnter(Collision col) {

        if (col.gameObject.tag == "tank" || col.gameObject.tag == "ground") {
            GameObject exp = Instantiate(explosion, this.transform.position, Quaternion.identity);
            Destroy(exp, 0.5f);
            Destroy(this.gameObject);
        }
    }

    void Start() {

        rb = this.gameObject.GetComponent<Rigidbody>();
    }

    void Update() {


        this.gameObject.transform.forward = rb.velocity;
    }
}
