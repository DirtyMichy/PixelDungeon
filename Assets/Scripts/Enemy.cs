using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour 
{
    public float rangeY = 1f;
    public float speed = 4f;
    public float delay = 1f;
    public GameObject web;
    private Transform origin;
    public bool killable = true;
	private Animator anim;

    void Awake()
    {
		anim = GetComponent<Animator> ();

		/*
        if(web)
        {
            origin = transform;
            web.gameObject.transform.parent = null;
            iTween.MoveAdd(gameObject, iTween.Hash("y", rangeY, "easeType", "easeInOutExpo", "loopType", "pingPong", "time", speed, "delay", delay));
            iTween.ScaleAdd(web, iTween.Hash("y", 45*Math.Abs(rangeY), "easeType", "easeInOutExpo", "loopType", "pingPong", "time", speed, "delay", delay));
        }
*/
		iTween.MoveAdd(gameObject, iTween.Hash("y", rangeY, "easeType", "easeInOutExpo", "loopType", "pingPong", "time", speed, "delay", delay));
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

        //iTween.Stop(web);
        AudioSource[] sounds = GetComponents<AudioSource>();
        sounds[0].Play(); 
		/*
        gameObject.AddComponent<Rigidbody2D>();
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 1000f);
*/
		anim.SetTrigger ("Die");

        yield return new WaitForSeconds(2f);
        
        Destroy(gameObject);
    }
}
