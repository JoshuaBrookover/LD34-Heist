using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndGameScript : MonoBehaviour {

	public Text scoreText;
	public float screenPause = 15.0f;

	// Use this for initialization
	void Start () {
        GameScript gameScript = Object.FindObjectOfType<GameScript>();
		int score = gameScript.GetScore();
		GameObject.Destroy(gameScript.gameObject);

		if ( score > 0 ) {
			scoreText.text = "You made it back to the safehouse with $" + score.ToString() + " dollars";
		} else {
			scoreText.text = "You made it back to the safehouse with nothing";
		}

		StartCoroutine( EndGame() );
	}
	
	IEnumerator EndGame() {

		yield return new WaitForSeconds(screenPause);
		Application.LoadLevel("menu");
	}
}
