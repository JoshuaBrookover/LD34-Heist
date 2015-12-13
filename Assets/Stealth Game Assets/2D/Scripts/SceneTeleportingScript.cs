using UnityEngine;
using System.Collections;

public class SceneTeleportingScript : MonoBehaviour {

    [SerializeField] private float minimumTeleportDistance ; 
    [SerializeField] private float maximumTeleportDistance ; 

    private Random randomNumbers;

	// Use this for initialization
	void Start () {
        randomNumbers = new Random ();
	}
	
    void OnTriggerEnter2D ( Collider2D other )
    {
        if (other.attachedRigidbody == null) {
            return;
        }

        Vector2 itemPosition = other.attachedRigidbody.position;

        // make a new x position
        float newXposition = itemPosition.x + Random.Range (minimumTeleportDistance, maximumTeleportDistance);

        itemPosition.x = newXposition;

        other.attachedRigidbody.MovePosition (itemPosition);
    }
}
