using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameScript : MonoBehaviour {

	public Text scoreText;
	public int moneyBagScore = 1000;

	int score = 0;
	float outroPause = 1.0f;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void GotMoneyBag() {
		score += moneyBagScore;

		if ( scoreText != null ) {
			scoreText.text = "$" + score.ToString();
		}
	}

	public void StartPhaseOne() {
		
	}

	public void EndPhaseOne() {
 		TellerManager tellerManager = Object.FindObjectOfType<TellerManager>();
 		tellerManager.GameOver();

 		Charge thrower = Object.FindObjectOfType<Charge>();
		thrower.enabled = false;

		StartCoroutine( PhaseOneOutro() );
	}

	IEnumerator PhaseOneOutro()	{
		yield return new WaitForSeconds(outroPause);

		Object.FindObjectOfType<Fader>().Fade();
	}
}
