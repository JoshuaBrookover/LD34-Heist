using UnityEngine;
using System.Collections;

public class ParallaxComponent : MonoBehaviour {
    public float offset;
    Vector3 startPosition;
    Parallax parallax;

    // Use this for initialization
    void Start () {
        parallax = GameObject.FindGameObjectWithTag("Parallax").GetComponent<Parallax>();
        startPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 position = startPosition;
        position.x += parallax.offset * offset;
        transform.position = position;
	}
}
