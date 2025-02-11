using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] Transform controller;
    [SerializeField] TextMeshProUGUI bulletsUI;

    Weapon pistol;
    float timePassedFromLastShot;
    bool backToPosition;
    // Start is called before the first frame update
    void Start()
    {
        backToPosition = true;
        pistol = GetComponent<Weapon>();    
    }

    // Update is called once per frame
    void Update()
    {
        // Get the current pressure of the trigger
        float triggerPressure = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch);

        // Update the position of the trigger GameObject based on the trigger pressure
        pistol.Trigger.transform.position = Vector3.Lerp(pistol.TriggerStartPosition.position, pistol.TriggerEndPosition.position, triggerPressure);

        if (triggerPressure < 0.15f && !backToPosition)
            backToPosition = true;

        if (triggerPressure > 0.7f && timePassedFromLastShot > pistol.DelayBetweenShots && backToPosition)
        {
            Shooting();
        }

        timePassedFromLastShot += Time.deltaTime;
    }

    void Shooting()
    {
        timePassedFromLastShot = 0;
        backToPosition = false;
        pistol.Shoot(bulletsUI);
        StartCoroutine(RecoilEffect());
        Raycast();
    }

    void Raycast()
    {
        GameObject newBullet = Instantiate(pistol.BulletPrefab, pistol.BarrelEnd.transform.position, pistol.BarrelEnd.transform.rotation);

        Bullet bulletComponent = newBullet.GetComponent<Bullet>();
        if (bulletComponent != null)
        {
            bulletComponent.Shoot(pistol.BarrelEnd.transform.forward);
        }

        RaycastHit hit;
        // Perform a raycast from the barrel end forward
        if (Physics.Raycast(pistol.BarrelEnd.transform.position, pistol.BarrelEnd.transform.forward, out hit, 700f))
        {
            Debug.Log("Hit: " + hit.collider.gameObject.name); // Log the name of the hit object
                                                               // Determine which part of the zombie was hit
            if (hit.collider.CompareTag("Head"))
            {
                Debug.Log("Hit the head!");
                HandleHeadShot(hit);
                return;
            }

            HandleHit(hit);
        }
    }

    void BloodEffect(Vector3 position, Vector3 normal, GameObject bloodEffectPrefab)
    {
        // Instantiate the blood effect at the hit position
        GameObject bloodEffect = Instantiate(bloodEffectPrefab, position, Quaternion.LookRotation(normal));
        bloodEffect.SetActive(true);
        // Optionally, destroy the blood effect after a few seconds if it doesn't self-destroy
        Destroy(bloodEffect, 2f);
    }

    IEnumerator RecoilEffect()
    {
        // Randomly determine the amount of recoil

        // Apply recoil
        float recoil = Random.Range(pistol.MinRecoilAmount, pistol.MaxRecoilAmount + 1);
        Quaternion recoilRotation = Quaternion.Euler(recoil, 0, 0);
        float elapsedTime = 0f; 
        float duration = 0.05f; // Duration for the recoil effect

        while (elapsedTime < duration)
        {
            pistol.RecoilObject.transform.localRotation = Quaternion.Slerp(pistol.RecoilObject.transform.localRotation, recoilRotation, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Return to original rotation
        elapsedTime = 0f;
        Quaternion originalRotation = Quaternion.Euler(0, 0, 0);
        duration = 0.05f; // Duration to return to original position
        while (elapsedTime < duration)
        {
            pistol.RecoilObject.transform.localRotation = Quaternion.Slerp(pistol.RecoilObject.transform.localRotation, originalRotation, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        pistol.RecoilObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    void HandleHit(RaycastHit hit)
    {
        // Example: Destroy the hit object if it's destructible
        Debug.Log("Hit " + hit.transform.gameObject.name);

        if (hit.transform.gameObject.tag == "Enemy")
        {
            float damage = Random.Range(pistol.MinAttackDamage, pistol.MaxAttackDamage + 1);
            hit.transform.parent.gameObject.GetComponent<Zombie>().GetShot(damage);
            BloodEffect(hit.point, hit.normal, pistol.BloodEffectParticle);
        }
        // Example: Spawn bullet impact effect at the hit location
        //Instantiate(impactEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
    }

    void HandleHeadShot(RaycastHit hit)
    {
        //GameObject zombie = hit.transform.gameObject;
        GameObject zombie = GetTopParent(hit.transform.gameObject);
        Zombie zombieScript = zombie.GetComponent<Zombie>();
        zombieScript.GetShot(pistol.HeadDamage);
        BloodEffect(hit.point, hit.normal, pistol.HeadBloodEffectParticle);
        pistol.HeadshotVibration();
    }

    GameObject GetTopParent(GameObject obj)
    {
        while (obj.transform.parent != null)
        {
            obj = obj.transform.parent.gameObject;
        }
        return obj;
    }
}
