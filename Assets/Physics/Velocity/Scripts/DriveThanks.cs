using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DriveThanks : MonoBehaviour
{
    public float speedMove = 1f;
    public float rotationSpeed = 100.0f;
    public Transform turret;
    public Transform gun;

    public GameObject shell;
    public GameObject enemy;

    private float speed = 15.0f;
    private float rotSpeed = 5.0f;
    private float moveSpeed = 1.0f;

    static float delayReset = 0.2f;
    float delay = delayReset;




    private void CreateBullet()
    {
        Instantiate(shell, gun.position, gun.rotation);
        GameObject shellFired = Instantiate(shell, turret.transform.position, turret.transform.rotation);
        shellFired.GetComponent<Rigidbody>().velocity = speed * gun.forward;

    }

    Vector3 CalculateTrajectory()
    {
        Vector3 p = enemy.transform.position - this.gameObject.transform.position;
        Vector3 v = enemy.transform.forward * enemy.GetComponent<DriveThanks>().speed;
        float s = shell.GetComponent<MoveShell>().speed;

        float a = Vector3.Dot(v, v) - s * s;
        float b = Vector3.Dot(p, v);
        float c = Vector3.Dot(p, p);
        float d = b * b - c * a;

        if (d < 0.1f)
        {
            return Vector3.zero;
        }

        float sqrt = Mathf.Sqrt(d);

        float t1 = (-b - sqrt) / c;
        float t2 = (-b + sqrt) / c;

        float time = 0;

        if (t1 < 0 && t2 < 0)
        {
            return Vector3.zero;
        }
        else if (t1 < 0)
        {
            time = t2;
        }
        else if (t2 < 0)
        {
            time = t1;
        }
        else
        {
            time = Mathf.Max(t1, t2);
        }

        return time * p + v;
    }





    void Update()
    {
    
        // Get the horizontal and vertical axis.
        // By default they are mapped to the arrow keys.
        // The value is in the range -1 to 1
        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;

        // Make it move 10 meters per second instead of 10 meters per frame...
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;

        // Move translation along the object's z-axis
        transform.Translate(0, 0, translation);
        //transform.Translate(0, 0, speed * Time.deltaTime);

        // Rotate around our y-axis
        transform.Rotate(0, rotation, 0);

        if (Input.GetKey(KeyCode.T))
        {
            turret.RotateAround(turret.position, turret.right, -2);
        }else if (Input.GetKey(KeyCode.G))
        {
            turret.RotateAround(turret.position, turret.right, 2);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            /*Vector3 aimAt = CalculateTrajectory();
            if (aimAt != Vector3.zero)
            {
                this.gameObject.transform.forward = aimAt;

            }*/
                CreateBullet();
                delay = delayReset;
        }
    }
}
