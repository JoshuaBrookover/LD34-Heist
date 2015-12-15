using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndGameScript : MonoBehaviour {

	public Text scoreText;
	public float screenPause = 10.0f;

    private int mMoneyCounter = 0;
    private int mScore = 0;
    private float mWaitTime = 0;
    private float mButtonDelay = 1.0f;

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
        int score = 0;
        
        GameScript gameScript = Object.FindObjectOfType<GameScript>();
        if ( gameScript != null ) {
            score = gameScript.GetScore();
            GameObject.Destroy(gameScript.gameObject);
        }
        mScore = score;
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


        mButtonDelay -= Time.deltaTime;
        if (mWaitTime > screenPause || (Input.GetButtonDown("First") && mButtonDelay < 0.0f))
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
