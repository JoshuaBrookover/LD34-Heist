using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameScript : MonoBehaviour {

	public Text scoreText;
	public int moneyBagScore = 1000;

	int score = 0;
	float outroPause = 1.0f;
    int mPhaseOneMoney;

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

    public void LoseMoney()
    {
        score -= (int)(mPhaseOneMoney * 0.25f);
        if(score < 0)
        {
            score = 0;
        }

        if (scoreText != null)
        {
            scoreText.text = "$" + score.ToString();
        }
    }

	public void StartPhaseOne() {
		
	}

	public void EndPhaseOne() {
 		TellerManager tellerManager = Object.FindObjectOfType<TellerManager>();
 		tellerManager.GameOver();

        mPhaseOneMoney = score;

 		Charge thrower = Object.FindObjectOfType<Charge>();
		thrower.enabled = false;

		StartCoroutine( PhaseOneOutro() );
	}

	IEnumerator PhaseOneOutro()	{
		yield return new WaitForSeconds(outroPause);

		Object.FindObjectOfType<Fader>().Fade();
	}
}
