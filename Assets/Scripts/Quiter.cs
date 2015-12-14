using UnityEngine;
using System.Collections;

public class Quiter : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Quit")) {
			Application.Quit();
		}
	}
}
