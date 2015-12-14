using System;
using UnityEngine;
//using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (PlatformerCharacter2D))]
    [RequireComponent(typeof (PlayerVisibilityLogic))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        private CoverPositionController m_sceneMotionHandler;
        private PlatformerCharacter2D m_Character;
        private PlayerVisibilityLogic m_visibiliy;

        [SerializeField] private bool m_DashRequested = true;
//        [SerializeField] private bool m_buttonDown;
        [SerializeField] private GameObject coverHandler;

        private float m_axisStrenght;

        /// <summary>
        /// Awake Fires as soon as the object is enabled, even before Start().
        /// </summary>
        private void Awake()
        {
            //m_sceneMotionHandler = coverHandler.GetComponent<CoverPositionController>();
            m_Character = GetComponent<PlatformerCharacter2D> ();
            m_visibiliy = GetComponent<PlayerVisibilityLogic> ();

        }

        /// <summary>
        /// Update runs every draw frame-step.
        /// </summary>
        /// <remarks>
        /// We use this step to read in input. 
        /// </remarks>
        private void Update()
        {
            // If we're not in cover, keep running.
            if (m_visibiliy.GetIsVisible ()) {
               // m_DashRequested = true;
            }

            // If they hit the button.
            if (Input.GetButton("First")) {
                m_DashRequested = true;
            }
        }

        /// <summary>
        /// Fixed Update runs every physics frame-step.
        /// </summary>
        /// <remarks>>
        /// We're going to read in input and then tell the character script to move.
        /// </remarks>
        private void FixedUpdate()
        {
            // if not ordered to move, default to 0.
            float movement = 0;

            // Standard movement.
            if (m_DashRequested ) {  // || m_buttonDown) {
                movement = 1;
            }

            if(m_DashRequested)
            {
                GetComponent<AudioSource>().UnPause();
            }
            else
            {
                GetComponent<AudioSource>().Pause();
            }
            


            //m_sceneMotionHandler.BroadcastPlayerMovment (movement);
            m_Character.Move (movement);

        }

        /// <summary>
        /// Message Handler for a cover object informing us it has been reached.
        /// </summary>
        /// <remarks> 
        /// We currently don't validate that it was the right type of object. Make sure the other folks play nice. 
        /// </remarks>
        /// <param name="caller">The Cover Object that called this, unused, but included to maintain the same signature.</param>
        void InCover (GameObject caller)
        {
            m_DashRequested = false;
            //m_Character.Move (0);
        }

    }
}
