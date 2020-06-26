using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class PlayerController : UnitObject
{
    public bool facingRight = true;
    public bool isJumping = true;

    public float moveForce = 365f;
    public float maxSpeed = 5f;
    public float jumpForce = 1000f;
    public Transform[] groundCheck;

    public Camera cam;
    public int playerID = 0;

    public bool isLeader = false;
    public bool grounded = false;
    private Animator anim;
    private Rigidbody2D rb2d;

    GamepadInput.GamePad.Index[] gamePadIndex;

    public GameObject weapon;

    public void SetPlayerID(int i)
    {
        playerID = i;
        if (i == 0)
            cam = Camera.main;
    }

    void Awake()
    {
        cam = Camera.main;
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();

        gamePadIndex = new GamepadInput.GamePad.Index[4];
        gamePadIndex[0] = GamePad.Index.One;
        gamePadIndex[1] = GamePad.Index.Two;
        gamePadIndex[2] = GamePad.Index.Three;
        gamePadIndex[3] = GamePad.Index.Four;
    }

    void Update()
    {
        for (int i = 0; i < groundCheck.Length; i++)
        {
            if (Physics2D.Linecast(transform.position, groundCheck[i].position, 1 << LayerMask.NameToLayer("Ground")))
                grounded = true;
        }

        //Attack
        if ((Input.GetKeyDown(KeyCode.X) && playerID == 0) || GamePad.GetButton(GamePad.Button.X, gamePadIndex[playerID]))
        {
            if (alive)
            {
                anim.SetTrigger("isAttacking");
                weapon.GetComponent<Weapon>().Attack();
            }
        }

        if ((((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && playerID == 0) || GamePad.GetButton(GamePad.Button.A, gamePadIndex[playerID])) && alive && grounded)
        {
            Debug.Log(rb2d.velocity.y);
            isJumping = true;

            GetComponent<AudioSource>().clip = jump;
            GetComponent<AudioSource>().Play();
        }

        grounded = false;
    }
    /*
    void Attack()
    {
         Instantiate(weapon, new Vector3(transform.position.x - 1f, transform.position.y + 2f, transform.position.z), transform.rotation);        
    }
    */
    void FixedUpdate()
    {
        if (alive)
        {
            Vector2 directionCurrent = GamePad.GetAxis(GamePad.Axis.LeftStick, gamePadIndex[playerID]);

            if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && playerID == 0)
                directionCurrent.x = -1f;
            if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && playerID == 0)
                directionCurrent.x = 1f;

            //Debug.Log(directionCurrent.x);
            if (directionCurrent.x * rb2d.velocity.x < maxSpeed)
                rb2d.AddForce(Vector2.right * directionCurrent.x * moveForce);

            if (Mathf.Abs(rb2d.velocity.x) > maxSpeed)
                rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);

            if (directionCurrent.x > 0 && !facingRight)
                Flip();
            else if (directionCurrent.x < 0 && facingRight)
                Flip();

            if (isJumping)
            {
                //			anim.SetTrigger("Jump");
                rb2d.AddForce(new Vector2(0f, jumpForce * GetComponent<Rigidbody2D>().mass));
                rb2d.velocity = new Vector2(0f, 0f);
                
                isJumping = false;
            }

            if (directionCurrent.x != 0)
            {
                anim.SetBool("isWalking", true);
            }
            else
            {
                anim.SetBool("isWalking", false);
            }
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}