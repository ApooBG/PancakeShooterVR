using Meta.WitAi;
using Oculus.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CookingPancake : MonoBehaviour
{
    [SerializeField] PancakeMaterialChange backsidePancake;
    [SerializeField] PancakeMaterialChange frontSidePancake;
    [SerializeField] GameObject triggerPlate;
    [SerializeField] GameObject pancakePrefab;
    [SerializeField] GameObject pan;
    [SerializeField] TextMeshProUGUI console;

    bool cooking = true;
    bool outsidePan = false;
    public float upsideDownThreshold = 160f;
    bool upsiteDown;
    float stoppedCooking = 0f;
    float outsideOfPan = 0f;

    // Start is called before the first frame update
    void Start()
    {
        upsiteDown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (outsidePan)
        {
            outsideOfPan += Time.deltaTime;
        }

        if (outsideOfPan > 0.2f)
        {
            cooking = false;
        }

        CheckPancakeOrientation();
        if (cooking == true)
        {
            stoppedCooking = 0;
            if (upsiteDown) frontSidePancake.CookPancake();
            else backsidePancake.CookPancake();
        }

        else
        {
            stoppedCooking += Time.deltaTime;
            CheckIfDropped();
        }

    }

    void CheckIfDropped()
    {
        if (stoppedCooking > 3f)
        {
            console.text = "Pancake dropped";
            SpawnerLevel.Instance.stats.PancakesDropped++;
            StartCoroutine(SpawnNewPancake(0.1f));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Pan")
        {
            console.text = "Pancake catched";
            cooking = true;
            outsidePan = false;
            outsideOfPan = 0;
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == triggerPlate && outsideOfPan > 0.5f)
        {
            if (backsidePancake.IsEdible())
                SpawnerLevel.Instance.stats.PancakesMade++;
            StartCoroutine(SpawnNewPancake(3f));
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Pan")
        {
            console.text = "Pancake lost";
            outsidePan = true;
        }
    }

    IEnumerator SpawnNewPancake(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        Vector3 position = new Vector3(pan.transform.position.x, pan.transform.position.y + 0.3f, pan.transform.position.z);
        GameObject gm = Instantiate(pancakePrefab, position, Quaternion.identity);
        gm.SetActive(true);
        this.gameObject.DestroySafely();
    }

    void CheckPancakeOrientation()
    {
        // Calculate the angle between the pancake's up vector and the world's up vector
        float angle = Vector3.Angle(transform.up, Vector3.up);

        // If the angle is greater than the threshold, the pancake is upside down
        if (angle > upsideDownThreshold)
        {
            upsiteDown = false;
        }
        else
        {
            upsiteDown = true;
        }
    }
}
