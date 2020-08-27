using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : UnitObject
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float spottingRange = 2f;
    [SerializeField]
    private float movementPower = 1000f;
    [SerializeField]
    private float jumpPower = 1000f;
    [SerializeField]
    private bool aquired = false;
    [SerializeField]
    private bool JumpBehaviour = false;
    [SerializeField]
    private bool ChaseBehaviour = false;

    [SerializeField]
    public float rotationSpeed = 10f;
    [SerializeField]
    public float speed = 1f;

    void Awake()
    {
        if (JumpBehaviour)
            StartCoroutine(Jumper());
    }

    void LateUpdate()
    {
        if (JumpBehaviour && grounded) JumpTowardsPlayer();
        if (ChaseBehaviour) ChasePlayer();

        grounded = false;

    }

    void ChasePlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);


            if (spottingRange > distance && gameObject.GetComponent<Rigidbody2D>())
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
                if (player.transform.position.x < gameObject.transform.position.x)
                {
                    gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(Time.deltaTime * movementPower * -1f, 0f);
                    Flip(1);
                }
                else
                {
                    gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(Time.deltaTime * movementPower * 1f, 0f);
                    Flip(-1);
                }
                anim.SetBool("isWalking", true);

                if (!GetComponent<AudioSource>().isPlaying)
                {
                    GetComponent<AudioSource>().clip = jump;
                    GetComponent<AudioSource>().Play();
                }
            }
            else
                anim.SetBool("isWalking", false);

        }
    }

    void JumpTowardsPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player"); //ugly right now, player gets destroyed, Iĺl fix this in a later version
        if (player != null && gameObject.GetComponent<Rigidbody2D>())
        {
            float distance = gameObject.transform.position.x - player.transform.position.x;
            //Debug.Log(distance);
            if (distance <= spottingRange)
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
                gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * Time.deltaTime * jumpPower);
                if (player.transform.position.x < gameObject.transform.position.x)
                {
                    gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * Time.deltaTime * movementPower * -1f);
                    Flip(1);
                }
                else
                {
                    gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * Time.deltaTime * movementPower);
                    Flip(-1);
                }

                if (!GetComponent<AudioSource>().isPlaying)
                {
                    GetComponent<AudioSource>().clip = jump;
                    GetComponent<AudioSource>().Play();
                }
            }
        }
    }

    IEnumerator Jumper()
    {
        while (alive && JumpBehaviour)
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * Random.Range(1, 200f));
            yield return new WaitForSeconds(Random.Range(1, 2f));
        }
    }
    void Flip(int value)
    {
        Vector3 theScale = transform.localScale;
        theScale.x = value;
        transform.localScale = theScale;
    }

}