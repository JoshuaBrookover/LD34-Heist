using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndGameScript : MonoBehaviour {

	public Text scoreText;
	public float screenPause = 10.0f;

    private int mMoneyCounter = 0;
    private int mScore = 0;
    private float mWaitTime = 0;

    void SetText(int count)
    {
        if (count > 0)
        {
            scoreText.text = "You made it back to the safehouse with $" + count.ToString() + " dollars!";
        }
        else
        {
            scoreText.text = "You made it back to the safehouse with nothing.";
        }
    }

	// Use this for initialization
	void Start () {
        GameScript gameScript = Object.FindObjectOfType<GameScript>();
        int score = gameScript.GetScore();
        mScore = score;
		GameObject.Destroy(gameScript.gameObject);
        mWaitTime = 0;

        SetText(score);

        mMoneyCounter = score > 0 ? 1 : 0;
	}

    void Update()
    {
        float rate = mScore / 3.0f;
        mMoneyCounter += (int)(Time.deltaTime * rate);

        if(mMoneyCounter > mScore)
        {
            mMoneyCounter = mScore;
            mWaitTime += Time.deltaTime;
        }

        SetText(mMoneyCounter);


        if (mWaitTime > screenPause)
        {
            //yield return new WaitForSeconds(screenPause);
            Application.LoadLevel("menu");     
        }
    }

// 	IEnumerator EndGame() {
// 
// 		yield return new WaitForSeconds(screenPause);
// 		Application.LoadLevel("menu");
// 	}
}
