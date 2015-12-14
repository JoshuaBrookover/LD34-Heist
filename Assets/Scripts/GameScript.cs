using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameScript : MonoBehaviour {

	public GameObject scoreParent;
	public Text scoreText;
    public GameObject Moneybag;
    public AnimationClip MoneybagShake;
	public int moneyBagScore = 1000;

    public GameObject GainMoneyUI;
    public AnimationClip GainMoneyAnim;
    public GameObject LoseMoneyUI;
    public AnimationClip LoseMoneyAnim;

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

            GameObject sack = Instantiate(GainMoneyUI) as GameObject;
            sack.transform.position = this.transform.position;
            sack.transform.parent = this.gameObject.transform;
            sack.AddComponent<Animation>();
            Animation throwAnimation = sack.GetComponent<Animation>();

            throwAnimation = GainMoneyUI.GetComponent<Animation>();
            throwAnimation.AddClip(GainMoneyAnim, "GainMoneyUI");
            throwAnimation.Play("GainMoneyUI");
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
            gainMoneyAnim.Play("GainMoney");

            GameObject sack = Instantiate(LoseMoneyUI) as GameObject;
            sack.transform.position = this.transform.position;
            sack.transform.parent = this.gameObject.transform;
            sack.AddComponent<Animation>();
            Animation throwAnimation = sack.GetComponent<Animation>();

            throwAnimation = LoseMoneyUI.GetComponent<Animation>();
            throwAnimation.AddClip(LoseMoneyAnim, "LoseMoneyUI");
            throwAnimation.Play("LoseMoneyUI");
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
