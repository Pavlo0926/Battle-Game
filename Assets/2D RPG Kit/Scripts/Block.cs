using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Lets you choose in which direction the block can move
public enum BlockDirections
{
    left,
    right,
    up,
    down,
    left_right,
    up_down,
    left_right_up_down
}

//Lets you decide if a targeted object shall be hidden or shown after moving the block
public enum TargetAction
{
    show,
    hide
}

//Adds a BoxCollider2D component automatically to the game object
[RequireComponent(typeof(BoxCollider2D))] 
public class Block : MonoBehaviour
{
    private BoxCollider2D box;
    private Rigidbody2D body;
    private bool wasMoved = false;//After block is moved, prevents moving again
    private bool isBeingInteractedWith = false; //flag to check the player is interacting
    private bool startMoving = false; //flag to begin moving the block
    private float currentTime; //used to check how long the user has been pushing
    private Vector3 startingLocation; //Where the block is located
    private Vector3 toLocation = Vector3.zero; //where the block will be moved too
    private float playerMoveSpeed;

    [Header("Settings")]
    public bool isMoveable = true;
    
    public BlockDirections moveDirection;
    
    public float waitTime = 0.75f;
    
    public float moveSpeed = 0.75f;

    //[Header("TARGET")]
    //[Tooltip("What GameObject should be shown or hidden?")]
    //public GameObject targetObject;
    //public TargetAction visibility;

    private void Awake()
    {
        box = GetComponent<BoxCollider2D>();

        if (box == null)
        {
            Debug.LogAssertion("ERROR - BoxCollider2D is required on object at: " + transform.position);
            HighlightObject();
        }

        body = gameObject.AddComponent<Rigidbody2D>();
        body.constraints = RigidbodyConstraints2D.FreezeAll;
        body.bodyType = RigidbodyType2D.Kinematic;

        startingLocation = transform.position;
        currentTime = waitTime;
    }

    private void Start()
    {
        playerMoveSpeed = PlayerController.instance.moveSpeed;
    }

    private void Update()
    {
        if (startMoving)
        {
            //PlayerController.instance.canMove = false;
            PlayerController.instance.moveSpeed = 1;
            transform.position = Vector3.MoveTowards(transform.position, toLocation, moveSpeed * Time.deltaTime);

            if (transform.position.Equals(toLocation))
            {
                startingLocation = transform.position;
                startMoving = false;

                PlayerController.instance.moveSpeed = playerMoveSpeed;
            }
        }
    }

    private void HighlightObject()
    {
        SpriteRenderer r = GetComponent<SpriteRenderer>();
        r.color = Color.red;
    }

    private void MoveLeft(Collision2D collision)
    {

        if (collision.relativeVelocity.x < 0)
        {
            //Pushing left
            currentTime -= Time.deltaTime;

            if (currentTime < 0)
            {
                isBeingInteractedWith = false;
                body.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                toLocation = new Vector2(startingLocation.x - .1f, startingLocation.y);
                startMoving = true;
            }
        }
    }

    private void MoveRight(Collision2D collision)
    {
        if (collision.relativeVelocity.x > 0)
        {
            //Pushing right
            currentTime -= Time.deltaTime;

            if (currentTime < 0)
            {
                isBeingInteractedWith = false;
                body.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                toLocation = new Vector2(startingLocation.x + .1f, startingLocation.y);
                startMoving = true;
            }
        }
    }

    private void MoveUp(Collision2D collision)
    {
        if (collision.relativeVelocity.y > 0)
        {
            //Pushing up
            currentTime -= Time.deltaTime;

            if (currentTime < 0)
            {
                isBeingInteractedWith = false;
                body.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                toLocation = new Vector2(startingLocation.x, startingLocation.y + .1f);
                startMoving = true;
            }
        }
    }

    private void MoveDown(Collision2D collision)
    {
        if (collision.relativeVelocity.y < 0)
        {
            //Pushing Down
            currentTime -= Time.deltaTime;

            if (currentTime < 0)
            {
                isBeingInteractedWith = false;
                body.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                toLocation = new Vector2(startingLocation.x, startingLocation.y - .1f);
                startMoving = true;
            }
        }
    }

    private void MoveLeftRight(Collision2D collision)
    {

        if (collision.relativeVelocity.x < 0)
        {
            //Pushing left
            currentTime -= Time.deltaTime;

            if (currentTime < 0)
            {
                isBeingInteractedWith = false;
                body.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                toLocation = new Vector2(startingLocation.x - .1f, startingLocation.y);
                startMoving = true;
            }

        } else if (collision.relativeVelocity.x > 0)
        {
            //Pushing right
            currentTime -= Time.deltaTime;

            if (currentTime < 0)
            {
                isBeingInteractedWith = false;
                body.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                toLocation = new Vector2(startingLocation.x + .1f, startingLocation.y);
                startMoving = true;
            }

        }

    }

    private void MoveUpDown(Collision2D collision)
    {
        if (collision.relativeVelocity.y > 0)
        {
            //Pushing up
            currentTime -= Time.deltaTime;

            if (currentTime < 0)
            {
                isBeingInteractedWith = false;
                body.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                toLocation = new Vector2(startingLocation.x, startingLocation.y + .1f);
                startMoving = true;
            }

        } else if (collision.relativeVelocity.y < 0)
        {
            //Pushing Down
            currentTime -= Time.deltaTime;

            if (currentTime < 0)
            {
                isBeingInteractedWith = false;
                body.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                toLocation = new Vector2(startingLocation.x, startingLocation.y - .1f);
                startMoving = true;
            }
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //check if the player, see if the block is in a state to be moved and is not previously moved and isn't currenly moving.
        if (collision.gameObject.CompareTag("Player") && isMoveable & !wasMoved && !startMoving)
        {
            isBeingInteractedWith = true;
            currentTime = waitTime;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {

        //if the player starts pushing then stops, but is still touching the block, reset time
        if (collision.relativeVelocity == Vector2.zero)
        {
            currentTime = waitTime;
        }

        //Checks if object is the player and if so, is the player interacting with block
        if (collision.gameObject.CompareTag("Player") && isBeingInteractedWith)
        {

            switch (moveDirection)
            {
                case BlockDirections.left:
                    MoveLeft(collision);
                    break;
                case BlockDirections.right:
                    MoveRight(collision);
                    break;
                case BlockDirections.up:
                    MoveUp(collision);
                    break;
                case BlockDirections.down:
                    MoveDown(collision);
                    break;
                case BlockDirections.left_right:
                    MoveLeftRight(collision);
                    break;
                case BlockDirections.up_down:
                    MoveUpDown(collision);
                    break;
                case BlockDirections.left_right_up_down:
                    MoveLeftRight(collision);
                    MoveUpDown(collision);
                    break;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isBeingInteractedWith)
        {
            isBeingInteractedWith = false;
        }
    }
}
