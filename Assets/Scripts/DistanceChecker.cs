using UnityEngine;
using System.Collections;

public class DistanceChecker : MonoBehaviour {

	public float levelEndDistance = 200.0f;
	public GameObject stealthPlayer;

	public GameObject playerIcon;
	public GameObject houseIcon;

	// Update is called once per frame
	void Update () {
		float endX = houseIcon.transform.position.x;
		float currentX = Mathf.Max(stealthPlayer.transform.position.x, 0.0f);


		float interp = currentX / levelEndDistance;

		RectTransform rect = playerIcon.GetComponent<RectTransform>();
		Vector2 pos = rect.anchoredPosition;
		rect.anchoredPosition = new Vector2(endX * interp, pos.y);

		Debug.Log("interp " + interp.ToString());
		if ( interp >= 1.0f ) {
			// game over
            Object.FindObjectOfType<GameScript>().EndPhaseTwo();
		}
	}
}
