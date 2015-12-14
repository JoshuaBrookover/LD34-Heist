using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameScript : MonoBehaviour {

	public GameObject scoreParent;
	public Text scoreText;
    public GameObject Moneybag;
    public AnimationClip MoneybagShake;
	public int moneyBagScore = 1000;

	int score = 0;
	float outroPause = 1.0f;
    int mPhaseOneMoney;
    Animation gainMoneyAnim;

	// Use this for initialization
	void Start () {
        gainMoneyAnim = Moneybag.GetComponent<Animation>();
        gainMoneyAnim.AddClip(MoneybagShake, "GainMoney");
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void OnLevelWasLoaded(int level) {

		if (level == 3) { //cutscene2
			scoreParent.SetActive(false);
		} else if (level == 4) { //phase2
			scoreParent.SetActive(true);
		}
	}


	public void GotMoneyBag() {
		score += moneyBagScore;

		if ( scoreText != null ) {
            scoreText.text = "$" + score.ToString();
            gainMoneyAnim.Play("GainMoney");
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

	public int GetScore() {
		return score;
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

	public void EndPhaseTwo() {
		Debug.Log("EndPhaseTwo");
		Object.FindObjectOfType<DistanceChecker>().enabled = false;
		Application.LoadLevel("endgame");
	}
}
