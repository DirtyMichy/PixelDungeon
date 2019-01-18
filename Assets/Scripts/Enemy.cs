using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour 
{
    public bool killable = true;
	private Animator anim;
    public GameObject player;
    public float spottingRange = 2f;
    public float jumpingPower = 1000f;
    private bool aquired = false;

    AudioSource[] sounds;

    void Awake()
    {
		anim = GetComponent<Animator> ();
        StartCoroutine(Jumper());
        sounds = GetComponents<AudioSource>();
    }

    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player"); //ugly right now, player gets destroyed, Iĺl fix this in a later version
        float distance = gameObject.transform.position.x - player.transform.position.x;
        Debug.Log(distance);
        if (distance <= spottingRange && distance >= spottingRange * -1f && !aquired)            
        {
            aquired = true;
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpingPower*10f);
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * distance * jumpingPower *-1f);
            sounds[1].Play();
        }  
    }

    IEnumerator Jumper()
    {
        while (true)
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * Random.Range(1, 200f));
            yield return new WaitForSeconds(Random.Range(0, 2f));
        }
    }

    void OnTriggerStay2D(Collider2D c)
    {
            if(c.tag == "Player" && killable)
                if(c.GetComponent<PlayerController>().damaging)
                    StartCoroutine(Die());
    }

    public IEnumerator Die()
    {
        killable = false;
		gameObject.tag = "Untagged";

		Destroy(GetComponent<BoxCollider2D>());
		Destroy(GetComponent<BoxCollider2D>());

        AudioSource[] sounds = GetComponents<AudioSource>();
        sounds[0].Play(); 

		anim.SetTrigger ("Die");

        yield return new WaitForSeconds(2f);
        
        Destroy(gameObject);
    }
}