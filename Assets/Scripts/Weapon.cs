using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage = 1;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player" && collision.GetComponent<UnitObject>())
        {
            Debug.Log(gameObject.name + " deals damage to: " + collision.gameObject.name);
            collision.GetComponent<UnitObject>().ApplyDamage(damage);
        }
        else
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
        }

        Debug.Log(collision.gameObject);
    }

    public void Attack()
    {
        Invoke("EnableDamage", 0.2f);
    }

    void EnableDamage()
    {
        GetComponent<AudioSource>().Play();
        GetComponent<CircleCollider2D>().enabled = true;
        Invoke("DisableDamage", 0.1f);
    }

    void DisableDamage()
    {
        GetComponent<CircleCollider2D>().enabled = false;
    }
}