using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string Name;
    public float MinAttackDamage;
    public float MaxAttackDamage;
    public float HeadDamage;
    public int Bullets;
    public float MinRecoilAmount;
    public float MaxRecoilAmount;
    public float ReloadTime;
    public float DelayBetweenShots;
    public GameObject BloodEffectParticle;
    public GameObject HeadBloodEffectParticle;
    public GameObject RecoilObject;
    public GameObject BulletPrefab;
    public GameObject BarrelEnd;
    public ParticleSystem ShootingParticles;
    public GameObject Trigger;
    public Transform TriggerStartPosition;
    public Transform TriggerEndPosition;
    public ControllerVibration RecoilControllerVibration;
    public ControllerVibration HeadshotControllerVibration;

    int currentBullets;
    // Start is called before the first frame update
    void Start()
    {
        RecoilControllerVibration.enabled = false;
        HeadshotControllerVibration.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot(TextMeshProUGUI bulletsUI)
    {
        ShootingParticles.Play();

        if (currentBullets > 0)
            currentBullets--;

        if (currentBullets == 0)
            currentBullets = Bullets;
        bulletsUI.text = "x" + currentBullets.ToString();
        RecoilVibration();
    }

    public void RecoilVibration()
    {
        RecoilControllerVibration.enabled = true;
        RecoilControllerVibration.RightControllerVibration();
    }

    public void HeadshotVibration()
    {
        HeadshotControllerVibration.enabled = true;
        HeadshotControllerVibration.RightControllerVibration();
    }
}
