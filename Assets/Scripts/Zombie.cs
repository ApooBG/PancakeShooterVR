using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Zombie : MonoBehaviour
{
    public float Health = 100f;
    public float Speed = 1f;
    public float AttackSpeed = 5f;
    public float AttackDamage = 20f;
    public float Armor = 0f;

    public MeshCollider collider;
    public SphereCollider headCollider;
    public GameObject Player;
    public Image healthBar;
    bool dead = false;

    [SerializeField] Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dead) return;

        Movement();
        UpdateUI();
    }

    public void GetShot(float damage)
    {
        if (dead)
            return;
        float calculatedDamage = damage - Armor;
        if (Armor > 0)
            Armor -= damage;

        if (Armor <= 0)
        {
            Health -= calculatedDamage;

            //add animations
        }

        if (Health <= 0)
        {
            Dead();
        }
    }

    public void Dead()
    {
        SpawnerLevel.Instance.KillZombie();
        Health = 0;
        dead = true;
        animator.SetTrigger("Dead");
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        collider.enabled = false;
        headCollider.enabled = false;
        UpdateUI();
    }

    void Movement()
    {
        transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, Speed * Time.deltaTime);
    }

    void UpdateUI()
    {
        healthBar.fillAmount = Health / 100f;
    }
}
