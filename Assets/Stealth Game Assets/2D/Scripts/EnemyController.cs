using UnityEngine;
using System.Collections;

namespace UnityStandardAssets._2D
{
    public class EnemyController : MonoBehaviour {

        [SerializeField] private float m_MaxSpeed = 5f; // The fastest the enemy can travel in the x axis.

        private Rigidbody2D m_Rigidbody2D;
        private bool m_FacingRight = true;  // For determining which way the player is currently facing.

        /// <summary>
        /// Awake Fires as soon as the object is enabled, even before Start().
        /// </summary>
        private void Awake()
        {
            // Setting up references.
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
        }

        /// <summary>
        /// Fixed Update runs every physics frame-step.
        /// </summary>
        /// <remarks>>
        /// Request a motion event towards the left edge.
        /// </remarks>
    	void FixedUpdate () {
            this.Move (-1);
    	}

        /// <summary>
        /// Order our rigidbod to move.
        /// </summary>
        /// <param name="move">direction and scale to move.</param>
        public void Move(float moveXAxis)
        {
            // Move the character
            m_Rigidbody2D.velocity = new Vector2(
                moveXAxis*m_MaxSpeed, 
                m_Rigidbody2D.velocity.y
            );

        }

        /// <summary>
        /// Flip the animator attached to this instance.
        /// </summary>
        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            m_FacingRight = !m_FacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        /// <summary>
        /// This event triggers while our collider box is overlaid with another.
        /// We're looking for the player object, and will send a message at whoever we're on top of.
        /// </summary>
        /// <remarks>
        /// Not assured to be fired every frame of contact, see documentation. 
        /// Good enough for our needs though.
        /// </remarks>
        /// <param name="other">
        /// The other collider we're on top of.
        /// </param>
        void OnTriggerStay2D ( Collider2D other ) {
            other.gameObject.SendMessage (
                "FindExposedPlayer", 
                this.gameObject,  
                SendMessageOptions.DontRequireReceiver );
        }

        /// <summary>
        /// Message Handler from the player object to stop moving, since we just hit them.
        /// </summary>
        void StopMoving() {
            m_MaxSpeed = 0f;
            GetComponent<SpriteRenderer> ().color = Color.blue;
        }
    }
}
