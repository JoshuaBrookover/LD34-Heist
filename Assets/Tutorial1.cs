using UnityEngine;
using System.Collections;

public class Tutorial1 : MonoBehaviour {
    public GameObject Text1;
    public GameObject Text2;
    public GameObject Text3;
    public GameObject Focus1;
    public GameObject Focus2;
    public GameObject ButtonIcon1;
    public GameObject ButtonIcon2;

    int state = 1;

    float delay = 2.0f;
    float s1FocusFade = 0.0f;
    float s2Timer = 5.0f;
    float s4Timer = 2.0f;
    float s5FocusFade = 0.0f;
    float s6Timer = 5.0f;
    float s8Timer = 1.0f;

	// Use this for initialization
	void Start () {
        Text1.SetActive(false);
        Text2.SetActive(false);
        Text3.SetActive(false);
        ButtonIcon1.SetActive(false);
        ButtonIcon2.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (delay > 0)
        {
            Focus1.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            Focus2.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            delay -= Time.deltaTime;
            return;
        }
        if (state == 1)
        {
            s1FocusFade += Time.deltaTime;
            Focus1.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, s1FocusFade);
            if (s1FocusFade >= 1)
            {
                state = 2;
            }
        }
        else if (state == 2)
        {
            ButtonIcon1.SetActive(true);
            Text1.SetActive(true);
            s2Timer -= Time.deltaTime;
            if (s2Timer <= 0.0f)
            {
                state = 3;
            }
        }
        else if (state == 3)
        {
            s1FocusFade -= Time.deltaTime;
            Focus1.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, s1FocusFade);
            if (s1FocusFade <= 0)
            {
                state = 4;
            }
        }
        else if (state == 4)
        {
            ButtonIcon1.SetActive(false);
            Text1.SetActive(false);
            s4Timer -= Time.deltaTime;
            if (s4Timer <= 0.0f)
            {
                state = 5;
            }
        }
        else if (state == 5)
        {
            s5FocusFade += Time.deltaTime;
            Focus2.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, s5FocusFade);
            if (s5FocusFade >= 1)
            {
                state = 6;
            }
        }
        else if (state == 6)
        {
            ButtonIcon2.SetActive(true);
            Text2.SetActive(true);
            s6Timer -= Time.deltaTime;
            if (s6Timer <= 0.0f)
            {
                state = 7;
            }
        }
        else if (state == 7)
        {
            s5FocusFade -= Time.deltaTime;
            Text2.SetActive(false);
            Text3.SetActive(true);
            ButtonIcon2.SetActive(false);
            Focus2.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, s5FocusFade);
            if (s5FocusFade <= 0)
            {
                state = 8;
            }
        }
        else if (state == 8)
        {
            s8Timer -= Time.deltaTime;
            if (s8Timer <= 0.0f)
            {
                state = 9;
            }
        }
        else if (state == 9)
        {
            Text3.SetActive(false);
            state = 10;
        }
	}
}
