using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class carHandler : MonoBehaviour
{
    private Rigidbody carRigidBody;
    public raycastWheel[] wheels; //This is where you reference the wheels from. Make sure your wheel objects go here.
    private RaycastHit wheelHit;
    public float maxSpeed = 3000f;
    public float accelerationSpeed = 10f;
    public AnimationCurve powerCurve;
    float motorInput = 0f;
    float steerInput = 0f;

    void Start()
    {
        carRigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Keyboard.current.wKey.isPressed) //Temp. accelerate input
        {
            motorInput = 1f;
        }
        else if (Keyboard.current.sKey.isPressed) //Temp. deccelerate input
        {
            motorInput = -1f;
        }
        else
        {
            motorInput = 0f;
        }
        if (Keyboard.current.dKey.isPressed) //Temp. steer input
        {
            steerInput = 1f;
        }
        else if (Keyboard.current.aKey.isPressed) //Temp. steer input
        {
            steerInput = -1f;
        }
        else
        {
            steerInput = 0f;
        }
    }
    void FixedUpdate()
    {
        foreach (raycastWheel i in wheels)
        {
            Suspension(i);
            Acceleration(i);
        }
    }

//uses the values from raycastWheel[] to set variables related to suspension
    void Suspension(raycastWheel thisWheel)
    {
        if (Physics.Raycast(thisWheel.transform.position, -thisWheel.transform.up, out wheelHit, thisWheel.springRestDistance))
        {
            Vector3 springDirection = thisWheel.transform.up; //suspension is relative to the cars orientation
            Vector3 wheelWorldVelocity = carRigidBody.GetPointVelocity(thisWheel.transform.position); //velocity of this wheel in the world
            float suspensionOffset = thisWheel.springRestDistance - wheelHit.distance; //calculate the difference in the current suspension to the resting position
            float suspensionVelocity = Vector3.Dot(springDirection, wheelWorldVelocity);
            float suspensionForce = (suspensionOffset * thisWheel.springStrength) - (suspensionVelocity * thisWheel.springDampening);

            thisWheel.modelWheel.position = wheelHit.point + Vector3.up * thisWheel.wheelRadius;

            carRigidBody.AddForceAtPosition(springDirection * suspensionForce, thisWheel.transform.position);
            Debug.DrawLine(thisWheel.transform.position, wheelHit.point, Color.red);
        }
    }

//uses the values from raycastWheel[] to set variables related to acceleration
    void Acceleration(raycastWheel thisWheel)
    {
        if (Physics.Raycast(thisWheel.transform.position, -thisWheel.transform.up, out wheelHit, thisWheel.springRestDistance) && thisWheel.isMotor && motorInput != 0f)
        {
            Vector3 accelerationDirection = thisWheel.transform.forward;
            float wheelVelocity = Vector3.Dot(carRigidBody.velocity, accelerationDirection);

            //thisWheel.modelWheel.rotation = wheelHit.point + Vector3.right * thisWheel.wheelRadius;

            if (wheelVelocity > maxSpeed)
            {
                return;
            }
            Vector3 wheelContact = wheelHit.point + Vector3.up * thisWheel.wheelRadius;
            Vector3 wheelForceVector = accelerationDirection * accelerationSpeed * motorInput;
            Vector3 wheelForcePosition = wheelContact - transform.position;

            carRigidBody.AddForceAtPosition(wheelForceVector, wheelForcePosition);
        }
    }
}
