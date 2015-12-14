using UnityEngine;
using System.Collections;

public class Tutorial2 : MonoBehaviour {
    public GameObject Text1;
    public GameObject Text2;
    public GameObject Focus1;
    public GameObject Focus2;
    public GameObject Focus3;
    public GameObject ButtonIcon1;
    public GameObject ButtonIcon2;

    int state = 1;

    float delay = 2.0f;
    float s1FocusFade = 0.0f;
    float s2Timer = 3.0f;
    float s4Timer = 2.0f;
    float s5FocusFade = 0.0f;
    float s6Timer = 2.0f;

	// Use this for initialization
	void Start () {
        Text1.SetActive(false);
        Text2.SetActive(false);
        ButtonIcon1.SetActive(false);
        ButtonIcon2.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (delay > 0)
        {
            Focus1.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            Focus2.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            Focus3.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            delay -= Time.deltaTime;
            return;
        }
        if (state == 1)
        {
            Text1.SetActive(true);
            ButtonIcon1.SetActive(true);
            s1FocusFade += Time.deltaTime;
            Focus1.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, s1FocusFade);
            Focus2.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, s1FocusFade);
            if (s1FocusFade >= 1)
            {
                state = 2;
            }
        }
        else if (state == 2)
        {
            s2Timer -= Time.deltaTime;
            if (s2Timer <= 0)
            {
                state = 3;
            }
        }
        else if (state == 3)
        {
            s1FocusFade -= Time.deltaTime;
            Focus1.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, s1FocusFade);
            Focus2.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, s1FocusFade);
            if (s1FocusFade <= 0)
            {
                state = 4;
            }
        }
        else if (state == 4)
        {
            Text1.SetActive(false);
            ButtonIcon1.SetActive(false);
            s4Timer -= Time.deltaTime;
            if (s4Timer <= 0)
            {
                state = 5;
            }
        }
        else if (state == 5)
        {
            Text2.SetActive(true);
            ButtonIcon2.SetActive(true);
            s5FocusFade += Time.deltaTime;
            Focus3.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, s5FocusFade);
            if (s5FocusFade >= 1)
            {
                state = 6;
            }
        }
        else if (state == 6)
        {
            s6Timer -= Time.deltaTime;
            if (s6Timer <= 0)
            {
                state = 7;
            }
        }
        else if (state == 7)
        {
            s5FocusFade -= Time.deltaTime;
            Focus3.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, s5FocusFade);
            if (s5FocusFade <= 0)
            {
                state = 8;
            }
        }
        else if (state == 8)
        {
            Text2.SetActive(false);
            ButtonIcon2.SetActive(false);
            state = 9;
        }
	}
}
