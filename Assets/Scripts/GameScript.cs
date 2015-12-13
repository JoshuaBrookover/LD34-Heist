using UnityEngine;
using System.Collections;

public class GameScript : MonoBehaviour {

	float outroPause = 1.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartPhaseOne() {
		
	}

	public void EndPhaseOne() {
		Debug.Log("EndPhaseOne");
 		TellerManager tellerManager = Object.FindObjectOfType<TellerManager>();
 		tellerManager.GameOver();

 		Charge thrower = Object.FindObjectOfType<Charge>();
		thrower.enabled = false;

		StartCoroutine( PhaseOneOutro() );
	}

	IEnumerator PhaseOneOutro()	{
		yield return new WaitForSeconds(outroPause);

		GetComponent<Fader>().Fade();
	}
}
