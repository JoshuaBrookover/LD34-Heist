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

	private IEnumerator typeCoroutine;

	enum DialogStage 
	{ 
		NotStarted, 
		Typing, 
		Pausing, 
		Done 
	};
	private DialogStage stage = DialogStage.NotStarted;
 
	// Use this for initialization
	void Start () {

		message = GetComponent<Text>().text;
		GetComponent<Text>().text = "";

		typeCoroutine = DoDialogStage(true);
		StartCoroutine( typeCoroutine );
	}

	void Update() {
		if ((stage == DialogStage.Typing || stage == DialogStage.Pausing) && Input.GetButtonDown("First")) {
			Skip();
		}
	}

	void HideAllSpecials()	{
		foreach (GameObject special in specials) {
			special.SetActive(false);
		}
	}

	void ShowAllSpecials()	{
		foreach (GameObject special in specials) {
			special.SetActive(true);
		}
	}

	IEnumerator DoDialogStage(bool typewriter) {
		if ( typewriter ) {
			stage = DialogStage.Typing;
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
		} else {
			ShortCutText();
		}

		stage = DialogStage.Pausing;
		yield return new WaitForSeconds(nextPause);
		Complete();
	}

	void Complete() {
		stage = DialogStage.Done;
		gameObject.SetActive(false);
		myEvent.Invoke();
	}

	void ShortCutText() {
		ShowAllSpecials();
		GetComponent<Text>().text = "";
		foreach (char letter in message.ToCharArray()) {
			if (letter != '@')
			{
				GetComponent<Text>().text += letter;
			}
		}      
	}

	void Skip() {
		if (stage == DialogStage.Typing) {
			StopCoroutine(typeCoroutine);
			typeCoroutine = DoDialogStage(false);
			StartCoroutine(typeCoroutine);
		} else if (stage == DialogStage.Pausing) {
			StopCoroutine(typeCoroutine);

			Complete();
		}
	}
}