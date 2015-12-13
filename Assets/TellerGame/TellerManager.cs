using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TellerManager : MonoBehaviour {

    public GameObject tellerPrefab;
    public List<AudioClip> threatenAudio;
    Teller[] tellers;
    int target;
    int hero;
    Parallax parallax;
    float timer;
    int soundIndex = 0;

	// Use this for initialization
	void Start () {
        parallax = GameObject.FindGameObjectWithTag("Parallax").GetComponent<Parallax>();

        tellers = (Teller[])FindObjectsOfType<Teller>().Clone();
        if (tellers.Length == 0)
        {
            GameObject.FindWithTag("GameManager").GetComponent<GameScript>().EndPhaseOne();
            return;
        }

        // sort tellers by X position
        bool sort = true;
        while (sort)
        {
            sort = false;
            for (int i = 0; i < tellers.Length - 1; ++i)
            {
                if (tellers[i].transform.position.x > tellers[i + 1].transform.position.x)
                {
                    Teller temp = tellers[i];
                    tellers[i] = tellers[i + 1];
                    tellers[i + 1] = temp;
                    sort = true;
                }
            }
        }

        target = tellers.Length / 2;
        hero = target;
        tellers[target].target = true;

        timer = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            tellers[target].target = false;
            target = (target + 1) % tellers.Length;
            tellers[target].target = true;
            if (hero == target && !GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().clip = threatenAudio[soundIndex];
                GetComponent<AudioSource>().volume = 0.7f;
                GetComponent<AudioSource>().Play();
                soundIndex = (soundIndex + 1) % threatenAudio.Count;
            }
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            parallax.offset -= 0.1f;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            parallax.offset += 0.1f;
        }
        parallax.offset = Mathf.Lerp(parallax.offset, tellers[target].GetTargetParallax(), 0.3f);

        foreach (Teller teller in tellers)
        {
            teller.DoStuff();
        }

        timer -= Time.deltaTime;
        if (timer < 0)
        {
            timer = Random.Range(2, 6);
            if (hero == target)
            {
                tellers[hero].hero = false;
                int oldHero = hero;
                while (hero == oldHero && tellers.Length != 1)
                {
                    hero = Random.Range(0, tellers.Length);
                }
                tellers[hero].hero = true;
            }
        }
	}

    public void GameOver() {
        for (int i = 0; i < tellers.Length - 1; ++i) {
            tellers[i].enabled = false;
            this.enabled = false;
        }
    }
}
