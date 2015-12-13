using UnityEngine;
using System.Collections.Generic;


public class PlayerVisibilityLogic : MonoBehaviour {


    [SerializeField] private bool isVisible = true;
    [SerializeField] private HashSet <GameObject> coverObjects; 
    [SerializeField] private int coverSetCount;

	/// <summary>
    /// Start() fires before any Updating can happen.
    /// </summary>
	void Start () {
        coverObjects = new HashSet <GameObject> ();
	}
	
	// Update is called once per draw frame
	void Update () {
        coverSetCount = coverObjects.Count;

        isVisible = (coverSetCount == 0);
	}

    /// <summary>
    /// Getter for isVisible.
    /// </summary>
    /// <returns>The value of isVisible</returns>
    public bool GetIsVisible () {
        return isVisible;
    }

    /// <summary>
    /// Message Handler for the Cop Car objects attempting to detect an exposed player.
    /// </summary>
    /// <param name="caller">Caller.</param>
    void FindExposedPlayer(GameObject caller) {
        
        // Ignore if hidden.
        if (isVisible) {
            print ("Found you!");
            this.gameObject.SetActive (false);

            // Counter-call the Cop Car to stop, no return object needed.
            caller.SendMessage (
                "StopMoving", 
                null, 
                SendMessageOptions.DontRequireReceiver
            );
        }

    }

    /// <summary>
    /// Message Handler for a cover object informing us it has been reached.
    /// </summary>
    /// <remarks> 
    /// We currently don't validate that it was the right type of object. Make sure the other folks play nice. 
    /// <remarks>
    /// <param name="caller">The Cover Object that called this.</param>
    void InCover (GameObject caller)
    {
        coverObjects.Add (caller);
    }

    /// <summary>
    /// Message Handler for a cover object informing us it has been left.
    /// </summary>
    /// <remarks> 
    /// We currently don't validate that it was the right type of object. Make sure the other folks play nice. 
    /// <remarks>
    /// <param name="caller">The Cover Object that called this.</param>
    void OutOfCover (GameObject caller)
    {
        coverObjects.Remove (caller);
    }

}
