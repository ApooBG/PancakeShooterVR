using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatulaToPancake : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    private FixedJoint joint;
    private bool isLifted = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spatula") && !isLifted)
        {
            // Create and configure the joint
            joint = gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = other.GetComponent<Rigidbody>();
            isLifted = true;
        }
    }

    void Update()
    {
        if (isLifted)
        {
            // Check if the spatula is tilted enough to release the pancake
            Vector3 spatulaUp = joint.connectedBody.transform.up;
            if (Vector3.Dot(spatulaUp, Vector3.up) < 0.5f) // Adjust the threshold as needed
            {
                Destroy(joint);
                isLifted = false;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Spatula") && isLifted)
        {
            Destroy(joint);
            isLifted = false;
        }
    }
}
