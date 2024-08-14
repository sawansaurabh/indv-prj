/*
Script that associates particle system of thruster to keyboard inputs 
and given force value taken from 'public' input.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceComponent : MonoBehaviour
{
    public Transform thruster;
    public KeyCode accel;
    public float force;
    
    private ParticleSystem thrusterParticleSystem;
    private Rigidbody rb;
    private ParticleSystem.EmissionModule emissionModule;

    // Start is called before the first frame update
    void Start()
    {
        thrusterParticleSystem = thruster.GetComponent<ParticleSystem>();
        rb = GetComponent<Rigidbody>();

        emissionModule = thrusterParticleSystem.emission;
        emissionModule.enabled = false;

        rb.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 localForceVector = new Vector3(force, 0.0f, 0.0f);

        if(Input.GetKey(accel))
        {
            Vector3 globalForceVector = transform.TransformDirection(localForceVector);
            emissionModule.enabled = true;
            rb.AddForce(globalForceVector, ForceMode.Force);
        }
        else
        {
            emissionModule.enabled = false;
        }
    }
}

