using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : UnitObject
{
    public GameObject player;
    public float spottingRange = 2f;
    public float jumpingPower = 1000f;
    private bool aquired = false;

    void Awake()
    {
        StartCoroutine(Jumper());
    }

    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player"); //ugly right now, player gets destroyed, Iĺl fix this in a later version
        if (player != null)
        {
            float distance = gameObject.transform.position.x - player.transform.position.x;
            //Debug.Log(distance);
            if (distance <= spottingRange && distance >= spottingRange * -1f && !aquired)
            {
                aquired = true;
                gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpingPower * 10f);
                gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * distance * jumpingPower * -1f);

                GetComponent<AudioSource>().clip = jump;
                GetComponent<AudioSource>().Play();
            }
        }
    }

    IEnumerator Jumper()
    {
        while (alive)
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * Random.Range(1, 200f));
            yield return new WaitForSeconds(Random.Range(0, 2f));
        }
    }
}