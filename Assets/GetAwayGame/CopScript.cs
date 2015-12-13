using UnityEngine;
using System.Collections;

public class CopScript : MonoBehaviour
{
    float mBobXOffset = 0.12f;
    float mBobYOffset = 0.05f;
    Vector3 mOriginalPosition;

    public bool mIsCar2;

    private float mTimeSeed;

    private float mAngle = 0;
    float mTurnAngleSpeed = 65.0f;
    private float mStopTurnAngle = 0.0f;

    private float mStopForwardTime = 0.0f;
    private float mStopForwardTimeMax = 0.5f;

    private float mBackawayTimer = 0.0f;
    private float mBackawayReturnTime = 1.2f;
    private float mBackawayDirection = 1.0f;
    private float mBackawayPosition = 0.0f;
    private float mBackawaySpeed = 0.8f;
    private float mBackawayDistance = 10.0f;

    private float mTime = 0.0f;

    void RotateToAngle(float angle)
    {
        float dist = mAngle - angle;
        float speed = mTurnAngleSpeed;
        float turnDist = speed * Time.deltaTime;
        if (Mathf.Abs(dist) < turnDist)
        {
            mAngle = angle;
            return;
        }

        float direction = dist < 0 ? 1 : -1;
        mAngle += direction * turnDist;
    }

    // Use this for initialization
    void Start()
    {
        mOriginalPosition = this.transform.position;
        mTimeSeed = (mOriginalPosition.x * 0.464f + mOriginalPosition.y * 0.2f) % 2 * Mathf.PI;
        mStopTurnAngle = ((float)Random.Range(256, 1024) / 1024.0f) - 0.5f;

        GetComponent<Animator>().SetBool("car2", mIsCar2);
    }

    // Update is called once per frame
    void Update()
    {
        // Compute cop backaway from obstacles.
        mBackawayTimer += Time.deltaTime;
        if(mBackawayTimer > mBackawayReturnTime)
        {
            mBackawayTimer = mBackawayReturnTime;
            mBackawayDirection = 1.0f;
        }
        else
        {
            mBackawayDirection = -1.0f;
        }
        if (!RoadTileScript.mStopped)
        {
            mBackawayPosition += mBackawayDirection * Time.deltaTime * mBackawaySpeed;
        }
        if(mBackawayPosition < 0)
        {
            mBackawayPosition = 0;
        }
        else if(mBackawayPosition > 1)
        {
            mBackawayPosition = 1;
        }
        float backawayLerp = 1.0f - mBackawayPosition;

        if(!RoadTileScript.mStopped)
        {
            mTime = (Time.realtimeSinceStartup + mTimeSeed) % 2 * Mathf.PI;
        }

        float xMove = (Mathf.Sin(mTime) + Mathf.Sin(mTime + 1.458f)) * 0.5f * mBobXOffset;
        xMove -= backawayLerp * mBackawayDistance;
        float yMove = (Mathf.Cos(mTime) + Mathf.Cos(mTime + 1.273f)) * 0.5f * mBobYOffset;
        this.transform.position = mOriginalPosition + new Vector3(xMove, yMove, 0);

        // If the player wrecked turn the cops and push them further.
        if (RoadTileScript.mStopped && mBackawayDirection > 0)
        {
            Vector3 p = this.transform.position;
            mStopForwardTime += Time.deltaTime;
            if (mStopForwardTime > mStopForwardTimeMax)
            {
                mStopForwardTime = mStopForwardTimeMax;
            }
            p.x += 0.2f * RoadTileScript.mRoadSpeed * (mStopForwardTime / mStopForwardTimeMax);
            this.transform.position = p;

            float turnAngle = 30;
            RotateToAngle(mStopTurnAngle * turnAngle);
            this.transform.rotation = Quaternion.AngleAxis(mAngle, Vector3.forward);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Don't back away if the trigger hits the player car.
        if (other.gameObject.CompareTag("Player"))
        {
            return;
        }
        
        mBackawayTimer = 0.0f;
    }
}
