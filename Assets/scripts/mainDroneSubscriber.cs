/*
Script for subscribing to thruster forces messages incoming from ROS2
and settting force inputs accordingly for each drone thruster.
Also associates particle system to force inputs.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.InterfacesIndvPrjPkg;

public class mainDroneSubscriber : MonoBehaviour
{
    public List<Transform> transforms;
    private float[] forces;
    private int[] thrusters;

    // Start is called before the first frame update
    void Start()
    {
        ROSConnection.GetOrCreateInstance().Subscribe<ThrusterForcesMsg>("thruster_forces_to_unity", MyCallback);

        foreach (Transform thruster in transforms)
        {
            ParticleSystem ps = thruster.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                var emission = ps.emission;
                emission.enabled = false;
                Debug.Log($"Particle system emission disabled for: {thruster.name}");
            }
            else
            {
                Debug.LogWarning($"Transform {thruster.name} does not have a ParticleSystem component.");
            }

            // Disable gravity for the parent Rigidbody
            Rigidbody parentRigidbody = thruster.GetComponentInParent<Rigidbody>();
            if (parentRigidbody != null)
            {
                parentRigidbody.useGravity = false;
            }
            else
            {
                Debug.LogWarning($"Parent of {thruster.name} does not have a Rigidbody component.");
            }
        }
    }

    void MyCallback(ThrusterForcesMsg forcesMessage)
    {
        forces = forcesMessage.forces;
        thrusters = forcesMessage.thrusters;

        // Debug log to check values
        Debug.Log("Received new thruster forces message.");
    }

    void Update()
    {
        if (forces != null && thrusters != null)
        {
            for (int i = 0; i < thrusters.Length; i++)
            {
                if (i >= transforms.Count)
                {
                    Debug.LogWarning("Thruster index out of range of transforms list.");
                    continue;
                }

                Transform thruster = transforms[i];
                ParticleSystem ps = thruster.GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    var emission = ps.emission;
                    if (thrusters[i] == 1)
                    {
                        emission.enabled = true;
                        Debug.Log($"Enabled particle emission for: {thruster.name}");
                    }
                    else
                    {
                        emission.enabled = false;
                        Debug.Log($"Disabled particle emission for: {thruster.name}");
                    }
                }
                else
                {
                    Debug.LogWarning($"Transform {thruster.name} does not have a ParticleSystem component.");
                }

                // Apply force to the parent Rigidbody
                Rigidbody parentRigidbody = thruster.GetComponentInParent<Rigidbody>();
                if (parentRigidbody != null)
                {
                    Vector3 forceVector = new Vector3(forces[i], 0.0f, 0.0f);
                    parentRigidbody.AddForce(forceVector, ForceMode.Force);
                    // Debug.Log($"Applied force {forceVector} to parent Rigidbody of: {thruster.name}");
                }
                else
                {
                    Debug.LogWarning($"Parent of {thruster.name} does not have a Rigidbody component.");
                }
            }
        }
    }
}
