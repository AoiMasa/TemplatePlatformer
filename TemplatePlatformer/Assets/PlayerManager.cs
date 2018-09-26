using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    [Header("Movement Settings")]
    [SerializeField]
    private float maxRunSpeed = 3;
    [SerializeField]
    private float runSpeed = 10;
    [SerializeField]
    private float jumpSpeed = 500;

    private Rigidbody2D rigidbodyComponent;
    public bool isGrounded;
    private bool isTouchingWall;

    // Use this for initialization
    void Start () {
        //Fetch the Rigidbody component you attach from your GameObject
        rigidbodyComponent = GetComponent<Rigidbody2D>();

        isGrounded = true;
        isTouchingWall = false;
    }

    // Update is called once per frame
    void Update()
    {

    }


    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
    {
        MovePlayer();
        JumpPlayer();
      //  Debug.Log("x:" + rigidbodyComponent.velocity.x + "- y:" + rigidbodyComponent.velocity.y);
        
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
            rigidbodyComponent.AddForce(Vector2.up * jumpSpeed);
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
