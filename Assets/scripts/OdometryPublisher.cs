using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using RosMessageTypes.Nav;
using RosMessageTypes.Std;
using RosMessageTypes.Geometry;
using RosMessageTypes.BuiltinInterfaces;

public class OdometryPublisher : MonoBehaviour
{
    ROSConnection ros;
    public string topicName = "/odom";
    public float publishMessageFrequency = 0.5f;
    private float timeElapsed;
    private OdometryMsg odomMessage;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Rigidbody rb;
    private float velocityElapsed;
    private Vector3 previousPosition;
    private Vector3 velocity;

    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<OdometryMsg>(topicName);

        odomMessage = new OdometryMsg();

        odomMessage.header = new HeaderMsg();
        odomMessage.header.stamp = new TimeMsg();

        odomMessage.pose = new PoseWithCovarianceMsg();
        odomMessage.pose.pose = new PoseMsg();
        odomMessage.pose.pose.position = new PointMsg();
        odomMessage.pose.pose.orientation = new QuaternionMsg();

        odomMessage.twist = new TwistWithCovarianceMsg();
        odomMessage.twist.twist = new TwistMsg();
        odomMessage.twist.twist.linear = new Vector3Msg();
        odomMessage.twist.twist.angular = new Vector3Msg();

        initialPosition = transform.position;
        initialRotation = transform.rotation;

        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component missing from the GameObject.");
        }

        timeElapsed = 0;
    }

    void Update() 
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= publishMessageFrequency && rb != null)
        {
            Vector3 velocity = rb.velocity;
            Vector3 angularVelocity = rb.angularVelocity;

            // Debug.Log($"Velocity: {velocity}");
            // Debug.Log($"Angular Velocity: {angularVelocity}");
            odomMessage.header.stamp = new TimeMsg
            {
                sec = (int)(uint)Time.time,
                nanosec = (uint)((Time.time - (int)Time.time) * 1e9)
            };

            odomMessage.header.frame_id = "odom";
            odomMessage.child_frame_id = "base_link";

            Vector3<FLU> relativePosition = (transform.position - initialPosition).To<FLU>();
            odomMessage.pose.pose.position = new PointMsg
            {
                x = relativePosition.x,
                y = relativePosition.y,
                z = relativePosition.z
            };

            // Calculate relative orientation
            Quaternion<FLU> relativeOrientation = (Quaternion.Inverse(initialRotation) * transform.rotation).To<FLU>();
            odomMessage.pose.pose.orientation = new QuaternionMsg
            {
                x = relativeOrientation.x,
                y = relativeOrientation.y,
                z = relativeOrientation.z,
                w = relativeOrientation.w
            };
        
            Vector3 currentPosition = transform.position;
            Vector3 displacement = currentPosition - previousPosition;
            velocity = displacement / publishMessageFrequency;

            Vector3<FLU> f_velocity = velocity.To<FLU>();
            odomMessage.twist.twist.linear = new Vector3Msg 
            {
                x = f_velocity.x,
                y = f_velocity.y,
                z = f_velocity.z
            };
            previousPosition = currentPosition;
            
            // Vector3<FLU> angularVelocity = rb.angularVelocity.To<FLU>();
            // odomMessage.twist.twist.angular = new Vector3Msg
            // {
            //     x = angularVelocity.x,
            //     y = angularVelocity.y,
            //     z = angularVelocity.z
            // };

            ros.Publish(topicName, odomMessage);

            timeElapsed = 0;
        }
    }
}
