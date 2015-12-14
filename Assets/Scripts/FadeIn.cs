using UnityEngine;
using System.Collections;

public class FadeIn : MonoBehaviour {

	public Texture2D fadeTexture;
	public float fadeTime = 0.0f;

	private bool fading = false;
	private float fadeRemaining = 0.0f;

	void Update()
	{
		if (IsFading())
		{
			fadeRemaining -= Time.deltaTime;
			if (fadeRemaining <= 0.0f) {
				fading = false;
			}
		}
	}

	void OnLevelWasLoaded(int level) {
		fading = true;
		fadeRemaining = fadeTime;
	}

	void OnGUI()
	{
		if (IsFading())
		{
			float alpha = Mathf.Clamp01( fadeRemaining / fadeTime );
			GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
			GUI.depth = -1000;
			GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height), fadeTexture);
		}
	}

	bool IsFading()
	{
		return fading;
	}
}
