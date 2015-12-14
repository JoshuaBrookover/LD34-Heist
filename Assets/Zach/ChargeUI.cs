using UnityEngine;
using System.Collections;

public class ChargeUI : MonoBehaviour 
{
	public GameObject charger;
	public GameObject sweetSpot;
	public GameObject charge;

	private Charge playerCharge;
	// Use this for initialization
	void Start () 
	{
		playerCharge = charger.GetComponent<Charge>();
	}
	
	// Update is called once per frame
	void LateUpdate () 
	{
		RectTransform sweetSpotRect = sweetSpot.GetComponent<RectTransform>();
		Vector2 newSweetSpotPosition = 397.0f * playerCharge.correctCharge * new Vector2( 1.0f, 0.0f );
		newSweetSpotPosition.x += 8.0f;
		sweetSpotRect.anchoredPosition = newSweetSpotPosition;
		
		RectTransform chargeSpotRect = charge.GetComponent<RectTransform>();
		Vector2 newChargeSpotPosition = 397.0f * playerCharge.chargeFraction * new Vector2( 1.0f, 0.0f );
		newChargeSpotPosition.x += 8.0f;
		chargeSpotRect.anchoredPosition = newChargeSpotPosition;

        if ( playerCharge.chargeFraction > playerCharge.correctCharge - playerCharge.chargeError && playerCharge.chargeFraction < playerCharge.correctCharge + playerCharge.chargeError )
        {
            float newScale = ( playerCharge.chargeError * 10 ) - Mathf.Abs( ( playerCharge.correctCharge - playerCharge.chargeFraction ) * 10 );
            Debug.Log(((playerCharge.correctCharge - playerCharge.chargeFraction) * 10));
            chargeSpotRect.localScale = new Vector3(1 + newScale, 1 + newScale, 1);
        }
        else
        {
            chargeSpotRect.localScale = new Vector3(1, 1, 1);
        }
	}
}
