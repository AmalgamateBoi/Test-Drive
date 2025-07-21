using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raycastWheel : MonoBehaviour
{
    private Rigidbody carRigidBody;
    public Transform modelWheel;

    [Header("Suspension")]
    public float springStrength = 2000f;
    public float springDampening = 2f;
    public float springRestDistance = 1f;

    [Header("Wheel")]
    public float wheelRadius = 0.26f;
    public float tireGrip = 0.2f;
    public float wheelMass = 12f;
    public bool isMotor = false;
    public bool isSteer = false;

    void Start()
    {
        modelWheel = transform.GetChild(0);
        Debug.Log(modelWheel.gameObject.name);
    }
}
