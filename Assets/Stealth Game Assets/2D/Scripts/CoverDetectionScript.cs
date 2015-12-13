using UnityEngine;
using System.Collections;

public class CoverDetectionScript : MonoBehaviour {
    [SerializeField] private bool isOccluding = false ; // A boolean that denotes if this object's colliders are occluding anything else.
    [Range(0, 1)][SerializeField] private float m_lerpForce = 0.2f; //The factor of LERP applied to the movement action. higher values mean more "inertia"
    [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
    private CoverPositionController parentalHandler;

    private Rigidbody2D m_Rigidbody2D;

    void Awake()
    {
        if (parentalHandler == null) {
            parentalHandler = this.transform.parent.gameObject.GetComponent<CoverPositionController> ();
        }

        // Setting up references.
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

	/// <summary>
    /// Update is called once per draw frame-step
    /// </summary>
    /// <remarks>
    /// We're updating the color of the cover so the player can tell when it's 'lit'.
    /// </remarks>
	void Update () {
        if (isOccluding) {
            GetComponent<SpriteRenderer> ().color = Color.red;
        } else {
            GetComponent<SpriteRenderer> ().color = Color.white;
        }
	}


    /// <summary>
    /// This fires when a trigger collider contacts us.
    /// </summary>
    /// <remarks>
    /// We send a message to whoever hit us. 
    /// We expect the player object to react.
    /// </remarks>
    /// <param name="other">The other collider that hit us.</param>
    void OnTriggerEnter2D ( Collider2D other ) {
        
        this.isOccluding = true;

        other.gameObject.SendMessage (
            "InCover", 
            this.gameObject,  
            SendMessageOptions.DontRequireReceiver );
    }

    /// <summary>
    /// This fires when a trigger collider un-contacts us.
    /// </summary>
    /// <remarks>
    /// We send a message to whoever hit us. 
    /// and expect the player object to react.
    /// </remarks>
    /// <param name="other">The other collider that un-hit us.</param>
    void OnTriggerExit2D ( Collider2D other ) {
        
        this.isOccluding = false;

        other.gameObject.SendMessage (
            "OutOfCover", 
            this.gameObject,  
            SendMessageOptions.DontRequireReceiver );
    }

    /// <summary>
    /// Requests the character to move.
    /// </summary>
    /// <param name="move">Factor of movement on the x-axxis.</param>
    /// <remarks>
    /// This Method combines the requested value with the current to blend the two together.
    /// </remarks>
    public void Move(float moveXAxis)
    {
        // Move the character, simulating drag by lerping with current velocity.
        m_Rigidbody2D.velocity = Vector2.Lerp (
            new Vector2(moveXAxis*m_MaxSpeed, m_Rigidbody2D.velocity.y),
            m_Rigidbody2D.velocity,
            m_lerpForce );

    }        
}
