using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitObject : MonoBehaviour
{
    public int maxHealth = 3, currentHealth = 3, damage = 0;
    public bool alive = true;

    [SerializeField]
    private GameObject healthBarPrefab = default;
    private GameObject healthBar;

    public AudioClip jump, hurt, die;

    private void Start()
    {
        healthBar = Instantiate(healthBarPrefab, new Vector3(transform.position.x - 1f, transform.position.y + 2f, transform.position.z), transform.rotation);
        healthBar.transform.parent = transform;

        UpdateHealthBar();
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject.tag == "Enemy" && collision.gameObject.GetComponent<UnitObject>() && alive)
        {
            collision.gameObject.GetComponent<UnitObject>().ApplyDamage(damage);
        }
    }
    
    /*
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == tag)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
        }
        if (tag == "Enemy" && collision.gameObject.GetComponent<UnitObject>() && alive)
        {
            Debug.Log("XXXXXXXXXXXXX");
            collision.GetComponent<UnitObject>().ApplyDamage(damage);
            CheckHealthStatus();
        }
    }
    */
    public void CheckHealthStatus()
    {
        if (alive)
            if (currentHealth <= 0)
            {
                GetComponent<AudioSource>().clip = die;
                GetComponent<AudioSource>().Play();

                alive = false;

                GetComponent<Animator>().SetTrigger("Die");

                Destroy(GetComponent<Rigidbody2D>());
                GetComponent<BoxCollider2D>().enabled = false;

                Invoke("Die", GetComponent<AudioSource>().clip.length);
            }
            else
            {
                GetComponent<AudioSource>().clip = hurt;
                GetComponent<AudioSource>().Play();
            }
    }

    public void UpdateHealthBar()
    {
        float x = 4f * currentHealth / maxHealth;
        healthBar.transform.localScale = new Vector3(x, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
    }
    public void ApplyDamage(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        GetComponent<Animator>().SetTrigger("Damage");

        UpdateHealthBar();
        CheckHealthStatus();
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        UpdateHealthBar();
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}