using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HealthScript : MonoBehaviour 
{
    private int mNumHealth = 4;
    public float mSpacing = 2.0f;

    public GameObject mHealth0;
    public GameObject mHealth1;
    public GameObject mHealth2;
    public GameObject mHealth3;

    float mCaughtTimer = 0;
    float mCatchRepeatTime = 1.5f;

    public static HealthScript mPublicHealthScript;

	// Use this for initialization
	void Start ()
    {
        mPublicHealthScript = this;
        mNumHealth = 4;
        mCaughtTimer = 0;
	}

    public void TakeLife()
    {
        if (mCaughtTimer < mCatchRepeatTime)
        {
            return;
        }
        mCaughtTimer = 0;
        GameObject[] health = { mHealth0, mHealth1, mHealth2, mHealth3 };
        Destroy(health[--mNumHealth]);

        if (mNumHealth == 0)
        {
            var gameScript = Object.FindObjectOfType<GameScript>();
            gameScript.EndPhaseTwo();
        }
    }
	
	// Update is called once per frame
	void Update () 
    {
        mCaughtTimer += Time.deltaTime;
	}
}
