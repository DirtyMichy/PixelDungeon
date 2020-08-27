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

    [SerializeField]
    private bool showHealthbar = false;

    [SerializeField]
    private bool hasCanvasHealthbar = false;

    public bool grounded = false;

    public AudioClip jump, hurt, die;

    [SerializeField]
    private GameObject healthDamageBar;

    public Transform[] groundCheck;

    public Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();

        healthDamageBar = GameObject.Find("PlayerDamageHealthBar");
        if (showHealthbar)
        {
            if (hasCanvasHealthbar)
            {
                healthBar = healthBarPrefab;
            }
            else
            {
                healthDamageBar = Instantiate(healthBarPrefab, new Vector3(transform.position.x - 1f, transform.position.y + 2f, transform.position.z), transform.rotation);
                healthDamageBar.transform.parent = transform;

                healthDamageBar.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f);

                healthBar = Instantiate(healthBarPrefab, new Vector3(transform.position.x - 1f, transform.position.y + 2f, transform.position.z), transform.rotation);
                healthBar.transform.parent = transform;
            }

            UpdateHealthBar();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject.tag != collision.gameObject.tag && collision.gameObject.GetComponent<UnitObject>() && alive)
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

    private void Update()
    {
        for (int i = 0; i < groundCheck.Length; i++)
        {
            if (Physics2D.Linecast(transform.position, groundCheck[i].position, 1 << LayerMask.NameToLayer("Ground")))
                grounded = true;
        }

        if (showHealthbar && healthDamageBar != null)
            healthDamageBar.transform.localScale = new Vector3(Mathf.Lerp(healthDamageBar.transform.localScale.x, healthBar.transform.localScale.x, Time.time * 0.01f), healthDamageBar.transform.localScale.y, healthDamageBar.transform.localScale.z);

    }
    public void ApplyDamage(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        GetComponent<Animator>().SetTrigger("Damage");

        if (showHealthbar)
            UpdateHealthBar();

        CheckHealthStatus();
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        if (showHealthbar)
            UpdateHealthBar();
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}