using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
 
public class AutoType : MonoBehaviour {
 
 	public float startPause = 0.0f;
 	public float linePause = 0.75f;
	public float letterPause = 0.2f;
	public float nextPause = 1.0f;
	public float symbolPause = 0.0f;

	public AudioClip sound;
	public List<GameObject> specials;
	public UnityEvent myEvent;
 
	string message;
 
	// Use this for initialization
	void Start () {

		message = GetComponent<Text>().text;
		GetComponent<Text>().text = "";
		StartCoroutine(TypeText ());
	}

	void HideAllSpecials()	{
		foreach (GameObject special in specials) {
			special.SetActive(false);
		}
	}
 
	IEnumerator TypeText () {

		yield return new WaitForSeconds(startPause);

		int specialCount = 0;
		foreach (char letter in message.ToCharArray()) {
			if (letter == '@')
			{
				yield return new WaitForSeconds (symbolPause);
				specials[specialCount++].SetActive(true);
			}
			else
			{
				GetComponent<Text>().text += letter;
			}

			if (letter != ' ')
			{
				if (sound)
					GetComponent<AudioSource>().PlayOneShot (sound);
					yield return 0;

				if ( letter == '\n' ) {
					yield return new WaitForSeconds (linePause);

				}
				else {
					yield return new WaitForSeconds (letterPause);
				}
			}
		}      

		yield return new WaitForSeconds(nextPause);

		gameObject.SetActive(false);
		myEvent.Invoke();
	}
}