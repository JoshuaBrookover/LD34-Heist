using UnityEngine;
using System.Collections;

public class Fader : MonoBehaviour {

	public Texture2D fadeTexture;
	public string nextLevel;
	public float fadeTime = 0.0f;

	private bool fading = false;
	private float fadeRemaining = 0.0f;

	void Update()
	{
		if (IsFading())
		{
			fadeRemaining -= Time.deltaTime;
			fadeRemaining = Mathf.Max(fadeRemaining, 0.0f);

			if (fadeRemaining <= 0.0f )
			{
				Application.LoadLevel(nextLevel); 
			}
		}
	}

	void OnGUI()
	{
		if (IsFading())
		{
			float alpha = Mathf.Clamp01( 1.0f - fadeRemaining / fadeTime );
			GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
			GUI.depth = -1000;
			GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height), fadeTexture);
		}
	}

	public void Fade()
	{
		fading = true;
		fadeRemaining = fadeTime;
	}

	public void FadeToLevel(string name, float time)
	{
		if (!IsFading())
		{
			nextLevel = name;
			fadeTime = time;
			fadeRemaining = fadeTime;
			fading = true;
		}
	}

	bool IsFading()
	{
		return fading;
	}
}
