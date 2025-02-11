using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PancakePhysics : MonoBehaviour
{
    public GameObject pan;
    public bool hasCloth;
    bool isKinematic = false;
    // Start is called before the first frame update
    void Start()
    {
/*        // This checks if the Rigidbody component is already attached to the object and adds one if it isn't.
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        // Adjust the Rigidbody properties for a realistic effect
        rb.mass = 0.2f; // Adjust mass as necessary
        rb.drag = 0.3f; // Adjust drag as necessary
        rb.angularDrag = 0.05f; // Adjust angular drag as necessary

        // Since it's a pancake, we don't want it to bounce too much
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;*/
    }

    // Update is called once per frame
    void Update()
    {
        if (isKinematic)
        {
            this.GetComponent<Rigidbody>().isKinematic = false;
            //if (hasCloth)
                //this.GetComponent<Cloth>().enabled = true;
            isKinematic = false;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            this.GetComponent<Rigidbody>().isKinematic = true;
            //this.GetComponent<Cloth>().enabled = false;
            Vector3 position = new Vector3(pan.transform.position.x, pan.transform.position.y + 1f, pan.transform.position.z);
            gameObject.transform.position = position;
            isKinematic = true;
        }
    }
}
