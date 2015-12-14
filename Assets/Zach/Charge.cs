using UnityEngine;
using System.Collections;

public class Charge : MonoBehaviour 
{
	public GameObject cashSack;
	public AnimationClip SuccessAnim;
	public AnimationClip FailAnim;
    public AudioClip ChaChingSound;
    public AudioClip BuzzerSound;
	public float chargePerSecond;
	public float charge;
	public float chargeFraction;
	public float cooldownTime;
	public float chargeError;
	public float correctCharge;
	public float chargeValue;
	bool canCharge = true;
    AudioSource audioSource;
	
	// Use this for initialization
	void Start () 
	{
		charge = 0.0f;
		chargeFraction = 0.0f;
		// The correct charge is between 20% and 80%
		correctCharge = Random.Range ( 0.2f, 0.8f);
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () 
	{
        canCharge = true;
		if ( (Input.GetButtonDown( "First" ) && canCharge) )
		{
			Release();
			charge = 0.0f;
			canCharge = false;
			StartCoroutine( ChargeCooldown() );
		}
        else if ( ( chargeFraction >= 1.0f ) )
        {
            charge = 0.0f;
            canCharge = false;
            correctCharge = Random.Range(0.2f, 0.8f);
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
			Object.FindObjectOfType<GameScript>().GotMoneyBag();

            audioSource.Stop();
            audioSource.clip = ChaChingSound;
            audioSource.volume = 0.08f;
            audioSource.Play();
		}
		else
		{
			//Lose
			throwAnimation.AddClip( FailAnim, "BadThrow");
			throwAnimation.Play("BadThrow");

            audioSource.Stop();
            audioSource.clip = BuzzerSound;
            audioSource.volume = 0.07f;
            audioSource.Play();
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
