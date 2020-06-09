using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Octopus : MonoBehaviour
{
    //OCTO PREFAB

    //Variables to move object
    [SerializeField]
    float mSpeed;
    [SerializeField]
    Rigidbody2D mRigidBody2D;

    //Face direction variables
    Vector2 mFacingDirection;
    Vector2 forward;

    //Timer Variables
    int numberOfPaces = 0;
    bool stillpacing;
    public static int objectsDestroyed;

    //Animator variables
    public static bool mSpawning;
    Animator mAnimator;

    [SerializeField]
    float spawnDelay;
    float nextTimeToSpawn;

    private void Start()
    {
        mAnimator = GetComponent<Animator>();
        nextTimeToSpawn = OctopusSpawner.nextTimeToSpawn + spawnDelay;
        mSpawning = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        forward = new Vector2(transform.right.x, transform.right.y);

        if (nextTimeToSpawn <= Time.time) //Time.time is the number of seconds elapsed since the start of the game
        {
            mSpawning = false;
            Move();
        }
        else
        {
            mSpawning = true;
        }

        mAnimator.SetBool("mSpawning", mSpawning);
    }

    void Move()
    {
        //using Vector2.right is not relative to our rotation (object will just go right no matter what rotation)

        mRigidBody2D.MovePosition(mRigidBody2D.position + forward * Time.fixedDeltaTime * mSpeed);

        //make sure octo only paces through screen 2 times, then disapears off screen
        if (numberOfPaces < 2)
        {
            stillpacing = true;
        }
        else
        {
            stillpacing = false;
        }
        //Destroy octo off-screen
        if ((transform.position.x < -10 || transform.position.x > 25) && stillpacing == false)
        {
            Destroy(gameObject);
            objectsDestroyed += 1;
        }
    }

    //Triggers for Octopus (Edges of screen)
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Edge1" && stillpacing == true)
        {
            //transform.right is a vector 3
            mSpeed *= -1;
            if (gameObject.transform.rotation.eulerAngles.y == 180) //this is just to check if the octopus was rotated 180 (because of its spawn point)
            {
                FaceDirection(-Vector2.right); //if the octopus WAS rotated 180, just have it facing the right direction
            }
            else
            {
                FaceDirection(Vector2.right);
            }
            numberOfPaces += 1;
        }
        if ( col.tag == "Edge2" && stillpacing == true)
        {
            mSpeed *= -1;
            if (gameObject.transform.rotation.eulerAngles.y == 180)
            {
                FaceDirection(Vector2.right);
            }
            else
            {
                FaceDirection(-Vector2.right);
            }
            numberOfPaces += 1;
        }
    }

    //Same as player Facing Direction methods
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
}
