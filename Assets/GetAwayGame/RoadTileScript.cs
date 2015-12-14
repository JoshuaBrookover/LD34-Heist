using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoadTileScript : MonoBehaviour
{
    public GameObject mRoadInstance;

    public GameObject mObstacleInstance0;
    public GameObject mObstacleInstance1;
    public GameObject mObstacleInstance2;

    public GameObject mDecorationInstance;

    private const int mNumRoadPieces = 3;
    private GameObject[] mRoadPieces = new GameObject[mNumRoadPieces];
    private float mSpawnX = 0;
    private float mLaneOffset = 1.1f;
    private float mDecorationOffset = 4.0f;

    public const float mStartSpeed = 12.0f;
    public const float mEndSpeed = 30.0f;
    public const float mSpeedupTime = 60.0f;
    public const float mAcceleration = (mEndSpeed - mStartSpeed) / mSpeedupTime;
    public static float mRoadSpeed = mStartSpeed;
    private float mRoadTileWidth = 0.0f;

    private List<GameObject> mObstacles = new List<GameObject>();

    float mSpawnTime = 0;
    float mNextSpawn = 0;

    float mDecorationSpawnTime = 0;
    float mDecorationNextSpawn = 0;

    public static bool mStopped = false;
    private bool mSpawnLane = false;
    private bool mDecorationSpawnLane = false;

    private Vector3 mBasePosition;

	// Use this for initialization
	void Start () 
    {
        Random.seed = (int)System.DateTime.Now.Ticks & 0x0000FFFF;
        mRoadSpeed = mStartSpeed;

        // Create road.
        var renderer = mRoadInstance.GetComponent<Renderer>();
        mRoadTileWidth = renderer.bounds.size.x-0.01f;

        mBasePosition = this.transform.parent.gameObject.transform.position;

        Vector3 basePosition = mBasePosition + new Vector3(-10, 0, 0);
        for (int i = 0; i < mNumRoadPieces; i++)
        {
            mRoadPieces[i] = (GameObject)Instantiate(mRoadInstance) as GameObject;
            Vector3 pos = basePosition;
            pos.x += mRoadTileWidth * i;
            pos.y -= 0.3f;
            mRoadPieces[i].transform.position = pos;
            mSpawnX = pos.x;
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(mStopped)
        {
            return;
        }

        mRoadSpeed += Time.deltaTime * mAcceleration;
        if(mRoadSpeed > mEndSpeed)
        {
            mRoadSpeed = mEndSpeed;
        }

        float moveDistance = mRoadSpeed * Time.deltaTime;
        for(int i = 0; i < mNumRoadPieces; i++)
        {
            Vector3 pos = mRoadPieces[i].transform.position;
            pos.x -= moveDistance;
            if (pos.x + mRoadTileWidth < -10)
            {
                pos.x += 10;
                pos.x = pos.x % mRoadTileWidth;
                pos.x += mSpawnX;
            }
            mRoadPieces[i].transform.position = pos;
        }

        List<GameObject> objectsToRemove = new List<GameObject>();
        foreach(GameObject g in mObstacles)
        {
            Vector3 p = g.transform.position;
            p.x -= moveDistance;
            g.transform.position = p;

            var renderer = g.GetComponent<Renderer>();
            if(g.transform.position.x + 2*renderer.bounds.size.x < -30)
            {
                objectsToRemove.Add(g);
            }
        }
        foreach(GameObject g in objectsToRemove)
        {
            Destroy(g);
            mObstacles.Remove(g);
        }
        if(mDecorationSpawnTime > mDecorationNextSpawn)
        {
            int obstacleIndex = Random.Range(0, 3);
            GameObject spawn = mDecorationInstance;
            GameObject newDecoration = Instantiate(spawn);
            Vector3 p = newDecoration.transform.position;
            p.x = mSpawnX;
            p.y = mBasePosition.y + (mDecorationSpawnLane ? -mDecorationOffset : mDecorationOffset);
            newDecoration.transform.position = p;
            mObstacles.Add(newDecoration);
            mDecorationSpawnTime -= mDecorationNextSpawn;

            mDecorationNextSpawn = Random.Range(0, 16) / 9.0f;
            if (mDecorationNextSpawn < 0.5f)
            {
                mDecorationNextSpawn = 0.5f;
            }

            mDecorationSpawnLane = !mDecorationSpawnLane;
        }

        if (mSpawnTime > mNextSpawn)
        {
            int obstacleIndex = Random.Range(0, 4);
            GameObject spawn;
            switch(obstacleIndex)
            {
                case 0:
                    spawn = mObstacleInstance0;
                    break;
                case 1:
                    spawn = mObstacleInstance1;
                    break;
                default:
                    spawn = mObstacleInstance2;
                    break;
            }

            GameObject newObstacle = Instantiate(spawn);
            Vector3 p = newObstacle.transform.position;
            p.x = mSpawnX;

            // If it's a pot hole give it a random rotation
            if (obstacleIndex != 0 && obstacleIndex != 1)
            {
                float randRotation = Random.Range(0, 359);
                newObstacle.transform.rotation = Quaternion.AngleAxis(randRotation, Vector3.forward);
            }
            else
            {
                float randRotation = Random.Range(0, 40) - 20;
                newObstacle.transform.rotation = Quaternion.AngleAxis(randRotation, Vector3.forward);
            }

            int laneSwitchRoll = Random.Range(0, 10);
            if(laneSwitchRoll > 4)
            {
                mSpawnLane = !mSpawnLane;
            }

            p.y = mBasePosition.y + (mSpawnLane ? -mLaneOffset : mLaneOffset);            
            newObstacle.transform.position = p;
            mObstacles.Add(newObstacle);
            mSpawnTime -= mNextSpawn;

            mNextSpawn = Random.Range(0, 30) / 15.0f;
            if(mNextSpawn < 1)
            {
                mNextSpawn = 1;
            }
        }
        mSpawnTime += Time.deltaTime;
        mDecorationSpawnTime += Time.deltaTime;
	}
}
