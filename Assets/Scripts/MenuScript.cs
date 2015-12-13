using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuScript : MonoBehaviour {

	public Canvas quitMenu;
	public Button startText;
	public Button exitText;
	private Fader fader;

	// Use this for initialization
	void Start () {
		quitMenu = quitMenu.GetComponent<Canvas>();
		startText = startText.GetComponent<Button>();
		exitText = exitText.GetComponent<Button>();	
		fader = GameObject.FindWithTag( "Fader" ).GetComponent<Fader>();
		quitMenu.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ExitPress()
	{
		startText.interactable = false;
		exitText.interactable = false;
		quitMenu.enabled = true;
	}

	public void CancelExit()
	{
		startText.interactable = true;
		exitText.interactable = true;
		quitMenu.enabled = false;
	}

	public void StartLevel()
	{
		fader.FadeToLevel("cutscene1", 0.75f);
		startText.interactable = false;
		exitText.interactable = false;
		quitMenu.enabled = false;
	}

	public void ExitGame()
	{
		Application.Quit();
	}
}
