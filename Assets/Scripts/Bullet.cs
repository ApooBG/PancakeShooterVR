using Meta.WitAi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f; // Speed at which the bullet will move
    private Vector3 moveDirection;

    public void Shoot(Vector3 direction)
    {
        gameObject.SetActive(true); // Activate the bullet, useful if using object pooling
        moveDirection = direction.normalized; // Set the direction normalized
    }

    void Update()
    {
        // Move the bullet in the set direction each frame
        transform.position += moveDirection * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        this.gameObject.DestroySafely();
    }
}
