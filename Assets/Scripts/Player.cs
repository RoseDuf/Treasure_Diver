using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    //Speed and Force Variables
    [SerializeField]
    float mWalkSpeed;
    [SerializeField]
    float mSwimSpeed;
    [SerializeField]
    float mSwimForce;
    [SerializeField]
    float downSpeed;

    //Nitro Cylinder variables
    [SerializeField]
    float nitroTimeLimit;
    float timesUp;
    
    //Weight variables
    float weight=0;
    float weight_modifier=0;

    //GroundCheck Variables
    [SerializeField]
    LayerMask mWhatIsGround;
    float kGroundCheckRadius = 0.1f;
    Vector2 mFacingDirection;
    
    // Invincibility timer
    float kInvincibilityDuration;
    float mInvincibleTimer;
    bool mInvincible;
    // Damage effects
    float kDamagePushForce = 1f;

    // Booleans 
    bool mSwimming;
    bool mMoving;
    bool mGrounded;
    bool mFalling;
    bool mHurt;
    bool inWater;
    public static bool nitroCylinderActivate;
    public static bool nitroCylinderObtained;
    bool timerActivated;

    //Lives
    public static int extraLives;

    // Reference to audio sources
    AudioSource mCoinSound;
    AudioSource mJetPackSound;
    AudioSource mTakeDamageSound;
    AudioSource mEnemySound;
    AudioSource mNitroSound;

    // References to other components (can be from other game objects!)
    Animator mAnimator;
    Rigidbody2D mRigidBody2D;
    List<GroundCheck> mGroundCheckList;
    
    void Start()
    {
        // Get references to other components and game objects
        mAnimator = GetComponent<Animator>();
        mRigidBody2D = GetComponent<Rigidbody2D>();

        // Obtain ground check components and store in list
        mGroundCheckList = new List<GroundCheck>();
        GroundCheck[] groundChecksArray = transform.GetComponentsInChildren<GroundCheck>();
        foreach (GroundCheck g in groundChecksArray)
        {
            mGroundCheckList.Add(g);
        }

        //initialization
        timerActivated = false;
        nitroCylinderObtained = false;
        extraLives = 2;

        // Get audio references
        AudioSource[] audioSources = GetComponents<AudioSource>();
        mCoinSound = audioSources[0];
        mJetPackSound = audioSources[1];
        mTakeDamageSound = audioSources[2];
        mEnemySound = audioSources[3];
        mNitroSound = audioSources[4];
    }

    void Update()
    {
        //increase weight the more gold you gather
        weight = Mathf.Clamp(Score.amountOfGold * 0.1f, 0f, 2f);
        weight_modifier = (1f + weight);
        
        CheckGrounded();
        MoveCharacter();
        Animate();
        CheckIfHurt();
        ResetOxygens();
        NitroCylinder();

        ////output to log the position change
        //Debug.Log(transform.position);
    }

    //When invincible, trigger mHurt animation
    //invincibility stops when you touch the ground
    void CheckIfHurt()
    {
        if (mInvincible)
        {
            mHurt = true;
            if (mGrounded)
            {
                mInvincible = false;
                mHurt = false;
            }
        }
    }

    //If you lost all oxygen but you still have extra lives left, reset oxygens to 2.
    void ResetOxygens()
    {
        if (LifeCounter.oxygens == 0 && mGrounded)
        {
            if (extraLives > 0)
            {
                LifeCounter.oxygens = 2;
            }
            Die();
        }
    }

    //Nitro Cylinder functions will follow
    //this is the overall control of nitroCylinders
    void NitroCylinder()
    {
        //Input to activate Nitro for 3 seconds
        if (Input.GetButtonDown("NitroBoost") && nitroCylinderObtained)
        {
            mNitroSound.Play(); //play sound
            nitroCylinderActivate = true; //nitro cylinder is activated
            NitroObtained.nitros = 0; //you no longer have a nitro cylinder in stock
            if (!timerActivated) //Timer only activates when you have any obtained
            {
                NitroActivationTime();
                timerActivated = true;
            }
        }
        if (timerActivated)
        {
            NitroTimer();
        }  
    }

    //just updates the timeInterval that will cause a nitro to disapear
    void NitroActivationTime()
    {
        timesUp = Time.time + nitroTimeLimit;
    }

    //Counter
    void NitroTimer()
    {
        if (timesUp <= Time.time) //Time.time is the number of seconds elapsed since the start of the game
        {
            nitroCylinderActivate = false;
            nitroCylinderObtained = false;
            timerActivated = false;
        }
    }

    //IsGrounded function that performs the logic and returns a boolean - true if the player is on the ground, false otherwise.
    private void CheckGrounded()
    {
        foreach (GroundCheck g in mGroundCheckList)
        {
            if (g.CheckGrounded(kGroundCheckRadius, mWhatIsGround, gameObject))
            {
                mGrounded = true;
                return;
            }
        }
        mGrounded = false;
    }
    
    //Controller for player movement
    private void MoveCharacter()
    {
        ////Determine movement speed
        float swimForce;
        float moveSpeed;

        //if nitro is activated, your speed and force is increased by 2
        if (nitroCylinderActivate)
        {
            swimForce = mSwimForce+2;
            moveSpeed = mGrounded ? (mWalkSpeed+2) : (mSwimSpeed+2);
        }

        //If you're invincible/hurt, you cant move
        if (mInvincible)
        {
            moveSpeed = 0;
            swimForce = 0;
        }
        else //you can move
        {
            swimForce = mSwimForce;
            moveSpeed = mGrounded ? mWalkSpeed : mSwimSpeed;
        }

        //Check if the player wants Player to swim (see input manager)
        //and set the value of "mSwimming" accordingly
        if (inWater) //check if in water, can swim if outside of water.
        {
            //Added this functionality to make game more playable.
            //If you press down it will stop propulsion force upwards and let you go down faster.
            if (Input.GetButton("Down") && !mInvincible)
            {
                transform.Translate(-Vector2.up * downSpeed * Time.deltaTime);
                mRigidBody2D.angularVelocity = 0;
                mRigidBody2D.velocity = Vector2.zero;
            }

            //Swimming functionality
            //LikeBalloon fight, add force upwards, need to keep pressing space bar.
            if (mGrounded && Input.GetButtonDown("Swim") && mRigidBody2D.velocity.y == 0)
            {
                mRigidBody2D.AddForce(Vector2.up * (swimForce / weight_modifier), ForceMode2D.Impulse);
                mJetPackSound.Play(); //bloop
            }
            if ((mGrounded != true) && Input.GetButtonDown("Swim") && mRigidBody2D.velocity.y != 0)
            {
                mRigidBody2D.AddForce(Vector2.up * (swimForce / weight_modifier), ForceMode2D.Impulse);
                mJetPackSound.Play(); //bloop
            }
        }

        //check for movement
        if (Input.GetButton("Left") && !mInvincible)
        {
            //translate the game object
            transform.Translate(-Vector2.right * moveSpeed * Time.deltaTime);
            FaceDirection(-Vector2.right);
            mMoving = true;
        }
        else if (Input.GetButton("Right") && !mInvincible)
        {
            //translate the game object
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            FaceDirection(Vector2.right);
            mMoving = true;
        }
        else
        {
            mMoving = false;
        }
    }

    //Animation controller
    void Animate()
    {
        if (!mGrounded)
        {
            mSwimming = true;
        }
        else
        {
            mSwimming = false;
        }
        if (mRigidBody2D.velocity.y < -2.0f)
        {
            mFalling = true;
        }
        else
        {
            mFalling = false;
        }

        mAnimator.SetBool("mSwimming", mSwimming);
        mAnimator.SetBool("mMoving", mMoving);
        mAnimator.SetBool("mFalling", mFalling);
        mAnimator.SetBool("mHurt", mHurt);
    }
    
    //The following 2 methods are for face direction
    //just makes sure the character's forward vector is right of left
    public Vector2 GetFacingDirection()
    {
        return mFacingDirection;
    }

    private void FaceDirection(Vector2 direction)
    {
        mFacingDirection = direction;
        if (direction == Vector2.left)
        {
            Vector3 newScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            transform.localScale = newScale;
        }
        else
        {
            Vector3 newScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            transform.localScale = newScale;
        }
    }

    //Simple collider to see if player is in or above the water
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "AboveWater")
        {
            inWater = false;
        }
        else if (collision.tag == "Water")
        {
            inWater = true;
        }
    }

    //All of the Triggers
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "shark" && !mInvincible)
        {
            mEnemySound.Play();
            LifeCounter.oxygens -= 1;
            if (LifeCounter.oxygens >= 0)
            {
                TakeDamage();
            }
        }
        if (collision.tag == "Octopus" && !mInvincible && !Octopus.mSpawning)
        {
            mEnemySound.Play();
            LifeCounter.oxygens -= 1;
            if (LifeCounter.oxygens >= 0)
            {
                TakeDamage();
            }
        }
        if (collision.CompareTag("Gold"))
        {
            Destroy(collision.gameObject);
            Score.amountOfGold += 1;
            mCoinSound.Play();
        }
        if (collision.CompareTag("Gold2"))
        {
            Destroy(collision.gameObject);
            Score.amountOfGold += 2;
            mCoinSound.Play();
        }
        if (collision.CompareTag("Gold3"))
        {
            Destroy(collision.gameObject);
            Score.amountOfGold += 10;
            mCoinSound.Play();
        }
        if (collision.tag == "Edge1")
        {
            transform.position = new Vector2(-4.5f, transform.position.y);
        }
        if (collision.tag == "Edge2")
        {
            transform.position = new Vector2(19f, transform.position.y);
        }
        if (collision.tag == "Nitro")
        {
            NitroObtained.nitros = 1;
            Destroy(collision.gameObject);
            nitroCylinderObtained = true;
        }

    }

    //If you lose all oxygen, but still have extra lives, just spawn on boat
    //else you go to game over screen
    public void Die()
    {
        Score.amountOfGold = 0; //lose the gold you've gathered
        transform.position = new Vector2(7.8f, 5.6f);
        extraLives -= 1;
        if (extraLives < 0)
        {
            Destroy(gameObject);
            SceneManager.LoadScene("GameOver");
        }
    }

    //simple force that applies when and enemy touches you. Alse sets invincibility to true
    public void TakeDamage()
    {
        if (!mInvincible)
        {
            mTakeDamageSound.Play();
            Vector2 forceDirection = new Vector2(-mFacingDirection.x, 1.0f) * kDamagePushForce;
            mRigidBody2D.velocity = Vector2.zero;
            mRigidBody2D.AddForce(forceDirection, ForceMode2D.Impulse);
            mInvincible = true;
        }
    }
}
