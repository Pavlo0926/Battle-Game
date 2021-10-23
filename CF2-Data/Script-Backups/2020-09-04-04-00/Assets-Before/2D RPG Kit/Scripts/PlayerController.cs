using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour {
    public Rigidbody2D rigidBody;
    public Animator animator;
    public float moveSpeed;
    
    public Animator Animation;

    //Make instance of this script to be able reference from other scripts!
    public static PlayerController instance;

    public string areaTransitionName;
    private Vector3 boundary1;
    private Vector3 boundary2;

    public bool canMove = true;

	// Use this for initialization
	void Awake () {

        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (instance == null)
        {
            instance = this;
        } else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        DontDestroyOnLoad(gameObject);
	}

    ///*
    // MOBILE INPUT
    // Uncomment this complete Update() function to enable mobile controls. But comment out the whole Update() function below this one.
    // Update is called once per frame
    void Update () {
        if (ControlManager.instance.mobile)
        {
            if (canMove)
            {
                rigidBody.velocity = new Vector2(Mathf.RoundToInt(CrossPlatformInputManager.GetAxis("Horizontal")), Mathf.RoundToInt(CrossPlatformInputManager.GetAxis("Vertical"))) * moveSpeed;
            }
            else
            {
                rigidBody.velocity = Vector2.zero;

            }

            Animation.SetFloat("moveX", rigidBody.velocity.x);
            Animation.SetFloat("moveY", rigidBody.velocity.y);

            if (CrossPlatformInputManager.GetAxisRaw("Horizontal") == 1 || CrossPlatformInputManager.GetAxisRaw("Horizontal") == -1 || CrossPlatformInputManager.GetAxisRaw("Vertical") == 1 || CrossPlatformInputManager.GetAxisRaw("Vertical") == -1)
            {
                if (canMove)
                {
                    Animation.SetFloat("lastMoveX", CrossPlatformInputManager.GetAxisRaw("Horizontal"));
                    Animation.SetFloat("lastMoveY", CrossPlatformInputManager.GetAxisRaw("Vertical"));
                }
            }

            transform.position = new Vector3(Mathf.Clamp(transform.position.x, boundary1.x, boundary2.x), Mathf.Clamp(transform.position.y, boundary1.y, boundary2.y), transform.position.z);
        }

        if (!ControlManager.instance.mobile)
        {
            if (canMove)
            {
                rigidBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                rigidBody.velocity = rigidBody.velocity.normalized * moveSpeed;
            }
            else
            {
                rigidBody.velocity = Vector2.zero;

            }

            Animation.SetFloat("moveX", rigidBody.velocity.x);
            Animation.SetFloat("moveY", rigidBody.velocity.y);

            if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
            {
                if (canMove)
                {
                    Animation.SetFloat("lastMoveX", Input.GetAxisRaw("Horizontal"));
                    Animation.SetFloat("lastMoveY", Input.GetAxisRaw("Vertical"));
                }
            }

            //This calculates the bounds and doesn't let the player go beyond the defined bounds
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, boundary1.x, boundary2.x), Mathf.Clamp(transform.position.y, boundary1.y, boundary2.y), transform.position.z);
        }
        
    }

    //Method to set up the bounds which the player can not cross
    public void SetBounds(Vector3 bound1, Vector3 bound2)
    {
        boundary1 = bound1 + new Vector3(.5f, 1f, 0f);
        boundary2 = bound2 + new Vector3(-.5f, -1f, 0f);
    }

}

