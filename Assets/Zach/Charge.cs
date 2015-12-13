using UnityEngine;
using System.Collections;

public class Charge : MonoBehaviour 
{
	public GameObject cashSack;
	public AnimationClip SuccessAnim;
	public AnimationClip FailAnim;
	public float chargePerSecond;
	public float charge;
	public float chargeFraction;
	public float cooldownTime;
	public float chargeError;
	public float correctCharge;
	public float chargeValue;
	bool canCharge = true;
	
	// Use this for initialization
	void Start () 
	{
		charge = 0.0f;
		chargeFraction = 0.0f;
		// The correct charge is between 20% and 80%
		correctCharge = Random.Range ( 0.2f, 0.8f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if ( (Input.GetAxis( "First" ) > 0 && canCharge) || ( chargeFraction >= 1.0f ) )
		{
			Release();
			charge = 0.0f;
			canCharge = false;
			StartCoroutine( ChargeCooldown() );
		}
		// Generate 10% every second
		charge += chargePerSecond * Time.deltaTime;
		chargeFraction = Mathf.Min( charge / 100.0f, 1.0f);
	}
	
	void Release()
	{
		GameObject sack = Instantiate( cashSack ) as GameObject;
		sack.transform.position = this.transform.position;
		sack.transform.parent = this.gameObject.transform;
		sack.AddComponent<Animation>();
		Animation throwAnimation = sack.GetComponent<Animation>();
		
		chargeFraction = charge / 100.0f;
		if ( chargeFraction > correctCharge - chargeError && chargeFraction < correctCharge + chargeError )
		{
			//Win
			throwAnimation.AddClip( SuccessAnim, "GoodThrow");
			throwAnimation.Play("GoodThrow");
		}
		else
		{
			//Lose
			throwAnimation.AddClip( FailAnim, "BadThrow");
			throwAnimation.Play("BadThrow");
		}
		
		// The correct charge is between 20% and 80%
		correctCharge = Random.Range ( 0.2f, 0.8f);
	}

	IEnumerator ChargeCooldown()
	{
		yield return new WaitForSeconds( cooldownTime );
		canCharge = true;
	}
}
