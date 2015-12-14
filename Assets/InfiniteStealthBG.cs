using UnityEngine;
using System.Collections;

public class InfiniteStealthBG : MonoBehaviour {
    public GameObject player;
    public GameObject bgPrefab;
    public GameObject baseBG;
    Vector3 offset = new Vector3(26.85f, 0);
    int num = 1;

	// Use this for initialization
	void Start () {
        Instantiate(bgPrefab, baseBG.transform.position + offset * (num++), Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
        while (player.transform.position.x + 60.0f > baseBG.transform.position.x + offset.x * num)
        {
            Instantiate(bgPrefab, baseBG.transform.position + offset * (num++), Quaternion.identity);
        }
	}
}
