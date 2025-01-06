using System;
//using System.Numerics;
using UnityEngine;

public class PlatformFighterController : MonoBehaviour
{
    private CharacterController characterController;
    public float moveSpeed = 6f;
    public float jumpForce = 8f;
    public float gravity = -20f;

    private Vector3 velocity;
    private bool isGrounded;
    public GameObject presents;

    Animator charAnimator;
    bool died = false;
    Vector3 startPos;

    public bool playMode = false;
    public bool atEnd = false;
    public int presentCount = 0; 

    GameManager gameManager;

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        characterController = GetComponent<CharacterController>();
        charAnimator = transform.GetChild(0).GetComponent<Animator>();
        startPos = characterController.transform.position; 
    }

    void InitPresents()
    {
        for ( int i = 0; i < presents.transform.childCount; i++ )
        {
            GameObject g = presents.transform.GetChild(i).gameObject;
            g.SetActive(true); 

        }
    }

    public string GetPresentsString()
    {
        // get total number of presents vs number active
        int total = presents.transform.childCount;
        int active = 0;

        for (int i = 0; i < presents.transform.childCount; i++)
        {
            GameObject g = presents.transform.GetChild(i).gameObject;
            if (g.activeSelf)
                active++; 
        }

        presentCount = total - active; 
        string st = "Presents " + (total - active) + "/" + total;
        return st; 
    }

    public string[] GetAnimNames()
    {
        string[] names = { "" };
        AnimatorClipInfo[] clipInfo = charAnimator.GetCurrentAnimatorClipInfo(0);
        if (clipInfo == null || clipInfo.Length == 0)
            return names;

        for (int i = 0; i < clipInfo.Length; i++)
        {
            names[i] = clipInfo[i].clip.name;
        }

        return names;
    }

    public bool AnimIs(string str)
    {
        string[] names = GetAnimNames();

        foreach (var n in names)
        {
            //Debug.Log("AnimIs " + n);
            if (str == n)
                return true;
        }

        return false;
    }

    void UpdateDied()
    {
        if ( died && AnimIs("Dead"))
        {
            ResetGame();
            gameManager.GotToStart();
        }
    }

    public void ResetGame()
    {
        characterController.enabled = false;
        characterController.transform.position = startPos;
        characterController.enabled = true;
        charAnimator.Play("Idle");
        died = false;
        InitPresents();
        atEnd = false; 
    }

    void Update()
    {
        if (!playMode)
            return;

        if (atEnd)
            return; 

        UpdateDied();

        if (died)
            return; 
            
        // Check if character is grounded
        isGrounded = characterController.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small negative value to keep grounded
        }

        // Get horizontal movement input
        float horizontalInput = Input.GetAxis("Horizontal");

        // Create movement vector
        Vector3 movement = new Vector3(horizontalInput, 0, 0);

        // can only move if idle, jumping, or already running
        if (AnimIs("Idle") || AnimIs("Jump") || AnimIs("Run"))
        {
            // Apply movement
            characterController.Move(movement * moveSpeed * Time.deltaTime);
        }

        bool jump = false; 
        // Handle jumping
        if ( (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) 
            && isGrounded)
        {
            jump = true; 
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        bool[] attacks = { false, false, false, false, false };

        // attacks .. but might keep Santa just passissive.  We'll see 
        if (Input.GetKeyDown(KeyCode.K))
            attacks[0] = true;
        if (Input.GetKeyDown(KeyCode.L))
            attacks[1] = true;
        if (Input.GetKeyDown(KeyCode.I))
            attacks[2] = true;
        if (Input.GetKeyDown(KeyCode.O))
            attacks[3] = true;

        // set santa to dance 
        if (Input.GetKeyDown(KeyCode.P))
            attacks[4] = true;

        // if dancing face the camera 
        if ( AnimIs("Dance") )
        {
            transform.forward = new Vector3(0, 0, -1);
        }
        else
        // Face the direction of movement
        if (movement != Vector3.zero)
        {
            transform.forward = new Vector3(movement.x, 0, 0);
        }

        UpdateAnimation(movement, jump, attacks);
    }

    void UpdateAnimation(Vector3 moveVec, bool jump, bool[] attacks )
    {
        // check for attacks first
        if (isGrounded && AnimIs("Idle"))
        {
            /*if (attacks[0])
                charAnimator.Play("Light");
            else
            if (attacks[1])
                charAnimator.Play("Heavy");
            else
            if (attacks[2])
                charAnimator.Play("Special1");
            else
            if (attacks[3])
                charAnimator.Play("Special2");
            else
            if (attacks[4])
                charAnimator.Play("Dance");*/
        }



        // if landed stop jump anim
        if (isGrounded && AnimIs("Jump"))
        {
            charAnimator.Play("Idle");
        }
        else
        if ( jump )
        {
            charAnimator.Play("Jump");
        }
        else
        if (isGrounded && moveVec.x != 0 && AnimIs("Idle"))
        {
            // set the run anim if idle 
            charAnimator.Play("Run");
        }
        else
            if (moveVec.x == 0 && AnimIs("Run"))
            charAnimator.Play("Idle");

        
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Check if the collided object's name contains "spike"
        if (!died)
        {
            if (hit.gameObject.name.ToLower().Contains("spike"))
            {
                Debug.Log("Character hit a spike!");
                // Add your spike collision logic here
                // For example:
                // TakeDamage();
                // PlayHitEffect();
                // etc.

                charAnimator.Play("Death");
                died = true;
            }
            else
            // if present pick up and disable the object 
            if (hit.gameObject.name.ToLower().Contains("ending"))
            {
                charAnimator.Play("Dance");
                hit.gameObject.SetActive(false);
                atEnd = true; 
            }
            else
            // if present pick up and disable the object 
            if (hit.gameObject.name.ToLower().Contains("present"))
            {
                hit.gameObject.SetActive(false); 
            }
        }
    }
}
