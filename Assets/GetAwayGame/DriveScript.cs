﻿using UnityEngine;
using System.Collections;

public class DriveScript : MonoBehaviour
{
    float mPosition = 1.0f;
    bool mDirection = true;
    float mAngle = 0;
    int mInLane = 1;

    float mBobXOffset = 0.003f;
    float mBobYOffset = 0.04f;

    float mTurnAngleSpeed = 65.0f;
    float mMaxTurnAngle = 40.0f;
    private float mLaneSwitchSpeed = 8.5f;

    Vector3 mOriginalPosition;

    bool mStop = false;

    private float mTime = 0.0f;

    bool mSpinning = false;
    float mSpinLerp = 0.0f;
    float mSpinLerpSpeed = 1.5f;
    int mNumSpins = 0;

	// Use this for initialization
	void Start () 
    {
        mOriginalPosition = this.transform.position;
	}

    void RotateToAngle(float angle)
    {
        float dist = mAngle - angle;
        float speed = (angle == 0.0f) ? 2 * mTurnAngleSpeed : mTurnAngleSpeed;
        float turnDist = speed * Time.deltaTime;
        if (Mathf.Abs(dist) < turnDist)
        {
            mAngle = angle;
            return;
        }

        float direction = dist < 0 ? 1 : -1;
        mAngle += direction * turnDist;
    }
	
	// Update is called once per frame
	void Update () 
    {
        if(mStop)
        {
            return;
        }

        if(mPosition >= 0.9)
        {
            mInLane = 1;
        }
        else if(mPosition <= -0.9)
        {
            mInLane = -1;
        }

        // If we're not stopped, and we're not switching lanes.
        if (!mStop && mInLane != 0)
        {
            mTime = Time.realtimeSinceStartup % 2 * Mathf.PI;
        }

        if (Input.GetButtonDown("Second"))
        {
            mDirection = !mDirection;

            var screech = GetComponents<AudioSource>()[2];
            if(!screech.isPlaying)
            {
                screech.Play();

                int rand = Random.Range(0, 41);
                rand -= 20;
                float pitchAdjust = (rand / 20.0f) * 0.2f;
                screech.pitch = 1.0f + pitchAdjust;
            }
        }

        if (mDirection == false && mInLane != -1)
        {
            RotateToAngle(-mMaxTurnAngle);
        }
        else if (mDirection == true && mInLane != 1)
        {
            RotateToAngle(mMaxTurnAngle);
        }
        else
        {
            RotateToAngle(0);
        }

        if(mSpinning)
        {
            mSpinLerp += Time.deltaTime * mSpinLerpSpeed;
            if(mSpinLerp >= 1.0f)
            {
                mNumSpins--;
                if(mNumSpins == 0)
                {
                    mSpinLerp = 1.0f;
                    mSpinning = false;
                }
                else
                {
                    mSpinLerp = mSpinLerp % 1.0f;
                }        
            }
        }

        float finalAngle = (mAngle + mSpinLerp * 360.0f) % 360.0f;
        this.transform.rotation = Quaternion.AngleAxis(finalAngle, Vector3.forward);

        mPosition += (mDirection ? 1.0f : -1.0f) * mLaneSwitchSpeed * Time.deltaTime;
        if(mPosition > 1.0f)
        {
            mPosition = 1.0f;
        }
        else if(mPosition < -1.0f)
        {
            mPosition = -1.0f;
        }

        Vector3 p = this.transform.position;
        p.y = mOriginalPosition.y + mPosition * 1.3f;

        float xMove = (Mathf.Sin(mTime) + Mathf.Sin(mTime + 1.256f)) * 0.5f * mBobXOffset;
        float yMove = (Mathf.Cos(mTime) + Mathf.Cos(mTime + 1.83f)) * 0.5f * mBobYOffset;
        p.x += xMove;
        p.y += yMove;

        this.transform.position = p;
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        // Don't collide cops.
        if (other.gameObject.CompareTag("Cop"))
        {
            return;
        }

//         mStop = true;
//         RoadTileScript.mStopped = true;

        mSpinning = true;
        if(mSpinLerp >= 1.0f)
        {
            mSpinLerp = 0.0f;
        }
        mNumSpins++;

        var smash = GetComponents<AudioSource>()[0];
        smash.Play();
        int rand = Random.Range(0, 41);
        rand -= 20;
        float pitchAdjust = (rand / 20.0f) * 0.2f;
        smash.pitch = 1.0f + pitchAdjust;

        var gameScript = Object.FindObjectOfType<GameScript>();
        Object.FindObjectOfType<GameScript>().LoseMoney();
        if(gameScript.GetScore() == 0)
        {
            gameScript.EndPhaseTwo();
        }
    }
}
