using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    [Header("Movement Settings")]
    [SerializeField]
    private float maxRunSpeed = 3;
    [SerializeField]
    private float runSpeed = 10;
    [SerializeField]
    private float jumpSpeed = 10;
    [SerializeField]
    private float fallMultiplier = 2.5f;
    [SerializeField]
    private float lowJumpMultiplier = 3f;
    [HideInInspector]
    public bool isGrounded;

    private Rigidbody2D rigidbodyComponent;
    private Animator animator;
    private bool isTouchingWall;

    // Use this for initialization
    void Start()
    {
        rigidbodyComponent = this.GetComponent<Rigidbody2D>();
        animator = this.GetComponent<Animator>();

        isGrounded = true;
        isTouchingWall = false;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("IsGrounded", isGrounded);

        float currentRunSpeed;
        if (!Input.GetButton("Horizontal"))
        {
            currentRunSpeed = 0;
        }
        else currentRunSpeed = Input.GetAxis("Horizontal");

        animator.SetFloat("CurrentRunSpeed", Mathf.Abs(currentRunSpeed));

        ChangeFacingDirection();

    }

    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
    {
        MovePlayer();
        JumpPlayer();
        //  Debug.Log("x:" + rigidbodyComponent.velocity.x + "- y:" + rigidbodyComponent.velocity.y);

    }


    private void ChangeFacingDirection()
    {
        if (Input.GetAxis("Horizontal") < -0.01f)
        {
            this.transform.localScale = new Vector3(-Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
        }
        else if (Input.GetAxis("Horizontal") > 0.01f)
        {
            this.transform.localScale = new Vector3(Mathf.Abs(this.transform.localScale.x), this.transform.localScale.y, this.transform.localScale.z);
        }
    }

    private void MovePlayer()
    {
        //Store the current horizontal input in the float moveHorizontal.
        float moveHorizontal;

        //If button for movement not pressed, reset velocity to 0 so the player will stop moving immediatly (removing any deccelaration effect)
        //Also set velocity to 0 if player is touching the wall on air to avoid odd momentum
        if (!Input.GetButton("Horizontal") || (isTouchingWall && !isGrounded))
        {
            moveHorizontal = 0;
        }
        else
        {
            moveHorizontal = Input.GetAxis("Horizontal") * runSpeed;
        }

        Vector2 movement = new Vector2(moveHorizontal, rigidbodyComponent.velocity.y);

        rigidbodyComponent.velocity = movement;

        //Limit max speed velocity
        if (rigidbodyComponent.velocity.x > maxRunSpeed)
        {
            rigidbodyComponent.velocity = new Vector2(maxRunSpeed, rigidbodyComponent.velocity.y);
        }
        else if (rigidbodyComponent.velocity.x < -maxRunSpeed)
        {
            rigidbodyComponent.velocity = new Vector2(-maxRunSpeed, rigidbodyComponent.velocity.y);
        }
    }

    private void JumpPlayer()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rigidbodyComponent.velocity = Vector2.up * jumpSpeed;
          
        }
        //Add gravity when player is falling to have a better jump feeling
        //https://www.youtube.com/watch?v=7KiK0Aqtmzc
        if (rigidbodyComponent.velocity.y < 0)
        {
            rigidbodyComponent.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rigidbodyComponent.velocity.y > 0 && !Input.GetButton("Jump")) //Add a low jump when just pressing the jump key 
        {
            rigidbodyComponent.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            isTouchingWall = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            isTouchingWall = false;
        }
    }


}
