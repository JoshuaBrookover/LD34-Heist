using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TellerManager : MonoBehaviour {

    public GameObject tellerPrefab;
    public List<AudioClip> threatenAudio;
    public List<AudioClip> scaredAudioFemale;
    public List<AudioClip> scaredAudioMale;
    AudioSource audioSource;
    Teller[] tellers;
    int target;
    int hero;
    Parallax parallax;
    float timer;
    int soundIndex = 0;
    int soundIndexScared = 0;
    bool tellerComplain = false;
    int tellerComplainIndex;
    bool yell = true;

	// Use this for initialization
	void Start () {
        parallax = GameObject.FindGameObjectWithTag("Parallax").GetComponent<Parallax>();
        audioSource = GetComponent<AudioSource>();

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
        if (!audioSource.isPlaying && tellerComplain)
        {
            tellerComplain = false;

            bool female = tellers[tellerComplainIndex].Female;
            if (female)
            {
                audioSource.volume = 0.08f;
                soundIndexScared = soundIndexScared % scaredAudioFemale.Count;
                audioSource.clip = scaredAudioFemale[soundIndexScared];
                audioSource.Play();
            }
            else
            {
                audioSource.volume = 0.08f;
                soundIndexScared = soundIndexScared % scaredAudioMale.Count;
                audioSource.clip = scaredAudioMale[soundIndexScared];
                audioSource.Play();
            }
            soundIndexScared++;
        }

        if (Input.GetButtonDown("Second"))
        {
            tellers[target].target = false;
            target = (target + 1) % tellers.Length;
            tellers[target].target = true;
            if (hero == target && yell)
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
                audioSource.clip = threatenAudio[soundIndex];
                audioSource.volume = 0.2f;
                audioSource.Play();
                soundIndex = (soundIndex + 1) % threatenAudio.Count;
                tellerComplain = Random.Range(0, 5) == 0;
                tellerComplainIndex = hero;
                yell = false;
            }
        }
        parallax.offset = Mathf.Lerp(parallax.offset, tellers[target].GetTargetParallax(), 0.3f);

        foreach (Teller teller in tellers)
        {
            teller.DoStuff();
        }

        if (!audioSource.isPlaying)
        {
            timer -= Time.deltaTime;
        }
        if (timer < 0 )
        {
            timer = Random.Range(1, 3);
            if (hero == target)
            {
                tellers[hero].hero = false;
                int oldHero = hero;
                while (hero == oldHero && tellers.Length != 1)
                {
                    hero = Random.Range(0, tellers.Length);
                }
                tellers[hero].hero = true;
                yell = true;
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
