using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : MonoBehaviour
{
    //these will be according to level
    float mSpeed;
    float mScale;
    float mMinSpeed;
    float mMaxSpeed;
    
    //desired shark sizes
    [SerializeField]
    float mMinScale;
    [SerializeField]
    float mMaxScale;

    //Things we need to animate
    Animator mAnimator;
    [SerializeField]
    Rigidbody2D mRigidBody2D;

    // Start is called before the first frame update
    void Start()
    {
        //speeds according to level+1 or +2
        mMinSpeed = Levels.levels + 1;
        mMaxSpeed = Levels.levels + 2;
        //random speeds and scale
        mSpeed = Random.Range(mMinSpeed, mMaxSpeed);
        mScale = Random.Range(mMinScale, mMaxScale);
        transform.localScale *= mScale;
        mAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        mMinSpeed = Levels.levels + 1;
        mMaxSpeed = Levels.levels + 2;

        //transform.right is a vector 3
        Vector2 forward = new Vector2(transform.right.x, transform.right.y);

        //using Vector2.right is not relative to our rotation (object will just go right no matter what rotation)
        mRigidBody2D.MovePosition(mRigidBody2D.position + forward * Time.fixedDeltaTime *mSpeed);

        if (transform.position.x < -10 || transform.position.x > 25)
        {
            Destroy(gameObject);
        }
    }
}
