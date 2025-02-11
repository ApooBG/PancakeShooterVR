using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointConfigure : MonoBehaviour
{
    [SerializeField] Joint joint;
    [SerializeField] GameObject target;
    [SerializeField] bool hasJoint;
    [SerializeField] float detectionRadius = 0.2f;
    [SerializeField] List<GameObject> listOfAttachableObjects;
    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch))
        {
            JointTrigger();
        }
    }

    void JointTrigger()
    {
        if (hasJoint)
        {
            hasJoint = false;
            joint.connectedBody = null;
            while (target.transform.parent != null)
            {
                target.transform.SetParent(null);
            }
        }

        else
        {
            if (CheckForObjectNear() != null)
            {
                hasJoint = true;
                joint.connectedBody = target.GetComponent<Rigidbody>();
                target.transform.SetParent(transform);
            }
        }
    }

    GameObject CheckForObjectNear()
    {
        GameObject nearestObject = null;
        float nearestDistance = Mathf.Infinity; // Start with the largest possible value

        // Loop through each GameObject in the list
        foreach (GameObject gameObject in listOfAttachableObjects)
        {
            float distance = Vector3.Distance(transform.position, gameObject.transform.position); // Calculate the distance from current object

            // Check if this object is closer than any previously checked ones and within the detection radius
            if (distance < nearestDistance && distance < detectionRadius)
            {
                nearestObject = gameObject;  // Assign this object as the nearest one
                nearestDistance = distance;  // Update the nearest distance
            }
        }

        target = nearestObject;
        // Return the nearest object only if it's within the detection radius
        return nearestObject;
    }
}
