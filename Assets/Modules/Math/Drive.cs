using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

// A very simplistic car driving on the x-z plane.

public class Drive : MonoBehaviour
{
    public float speed = 10.0f;
    public float rotationSpeed = 100.0f;
    public GameObject fuel;
    public Boolean autoPilot = false;
    public float speedMoveAuto = 5f;
    public float speedRotateAuto = 0.5f;

    void Start()
    {

    }

    void AutoPilot()
    {
        CalculateAngle();
        this.transform.position += this.gameObject.transform.up * speedMoveAuto * Time.deltaTime;
    }
    float CalculateDistance()
    {
        float distance = 0;
        Vector3 obj2Pos = fuel.transform.position;
        Vector3 obj1Pos = this.gameObject.transform.position;

        Vector3 tankToFuel = obj2Pos - obj1Pos;

        // distance = Mathf.Sqrt(Mathf.Pow((obj2Pos.x - obj1Pos.x),2) + Mathf.Pow((obj2Pos.y - obj1Pos.y),2)); PITAGORAS

        // distance = Vector3.Distance(obj2Pos, obj1Pos); Distancia em 3D, p tirar uma direção é só colocar 0  na direção dos dois  Vector3 q vai n parametro

        distance = tankToFuel.magnitude; //sqrMagnitude is faster than magnitude. Use to compare distances, but you need to use the square falue to compare

        return distance;
    }

    void CalculateAngle()
    {
        float angle = 0;
        Vector3 obj2Pos = fuel.transform.position;
        Vector3 obj1Pos = this.gameObject.transform.position;

        Vector3 tankToFuelDistance = obj2Pos - obj1Pos;

        Vector3 facingDirection = this.gameObject.transform.up; // use the direction the object is facing

        //Dot product
        float dot =  (facingDirection.x * tankToFuelDistance.x) + (facingDirection.y * tankToFuelDistance.y);
        angle = Mathf.Acos(dot / (facingDirection.magnitude * tankToFuelDistance.magnitude));

        int clockwise = 1;

        if(Cross(facingDirection, tankToFuelDistance).z < 0)
        {
            clockwise = -1;
        } 

        if(angle * Mathf.Rad2Deg > 10)
        {
            this.gameObject.transform.Rotate(0, 0, angle * Mathf.Rad2Deg * clockwise * speedRotateAuto);

        }

    }

    Vector3 Cross(Vector3 v, Vector3 w)
    {
        float xMult = v.y * w.z - v.z * w.y;
        float yMult = v.x * w.z - v.z * w.x;
        float zMult = v.x * w.y - v.y * w.x;

        return new Vector3 (xMult, yMult, zMult);
    }

    void LateUpdate()
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
        transform.Translate(0, translation, 0);

        // Rotate around our y-axis
        transform.Rotate(0, 0, - rotation);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CalculateAngle();
            CalculateDistance();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            autoPilot = true;
        }

        if(CalculateDistance() < 3f)
        {
            autoPilot = false;
        }

        if (autoPilot)
        {
            AutoPilot();
        }
        

    }
}