using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoadTileScript : MonoBehaviour
{
    public GameObject mRoadInstance;
    public GameObject mBlockInstance;

    private const int mNumRoadPieces = 5;
    private GameObject[] mRoadPieces = new GameObject[mNumRoadPieces];
    private float mSpawnX = 0;
    private float mLaneOffset = 1.1f;

    public const float mRoadSpeed = 8.0f;
    private float mRoadTileWidth = 0.0f;

    private List<GameObject> mObstacles = new List<GameObject>();

    float mSpawnTime = 0;
    float mNextSpawn = 0;

    public static bool mStopped = false;

    private Vector3 mBasePosition;

	// Use this for initialization
	void Start () 
    {
        Random.seed = (int)System.DateTime.Now.Ticks & 0x0000FFFF;

        // Create road.
        var renderer = mRoadInstance.GetComponent<Renderer>();
        mRoadTileWidth = renderer.bounds.size.x-0.0001f;

        mBasePosition = this.transform.parent.gameObject.transform.position;

        Vector3 basePosition = mBasePosition + new Vector3(-10, 0, 0);
        for (int i = 0; i < mNumRoadPieces; i++)
        {
            mRoadPieces[i] = (GameObject)Instantiate(mRoadInstance) as GameObject;
            Vector3 pos = basePosition;
            pos.x += mRoadTileWidth * i;
            //pos.y = 
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
            if(g.transform.position.x + 2*renderer.bounds.size.x < -10)
            {
                objectsToRemove.Add(g);
            }
        }
        foreach(GameObject g in objectsToRemove)
        {
            Destroy(g);
            mObstacles.Remove(g);
        }

        if (mSpawnTime > mNextSpawn)
        {
            GameObject newObstacle = Instantiate(mBlockInstance);
            Vector3 p = newObstacle.transform.position;
            p.x = mSpawnX;

            int lane = Random.Range(0, 2);
            p.y = mBasePosition.y + ((lane == 0) ? -mLaneOffset : mLaneOffset);            
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
	}
}
