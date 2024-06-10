/*
script for testing subscription of custom ROS2 messages on Unity side.
This script subscribes to custom message and just prints it.
*/

using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.InterfacesIndvPrjPkg;

public class ThrusterSubscriber : MonoBehaviour
{
    public GameObject drone;

    void Start()
    {
        ROSConnection.GetOrCreateInstance().Subscribe<ThrusterForcesMsg>("thruster_forces_to_unity", printMsg);
    }

    void printMsg(ThrusterForcesMsg forcesMessage)
    {
        Debug.Log(forcesMessage);
    }
}