using UnityEngine;
using System.Collections;

public class Teller : MonoBehaviour {
    public bool target = false;
    public bool hero = false;
    public Sprite ScaredSprite;
    public Sprite BraveSprite;
    public Sprite ScaredSpriteFemale;
    public Sprite BraveSpriteFemale;
    public bool Female;
    public GameObject ExclamationPrefab;
    public GameObject SpeechPrefab;
    float shake = 0.0f;
    float progress = 0.0f;
    Vector3 position = new Vector3(0, 0, 0);
    Vector3 startPosition = new Vector3(0, 0, 0);
    Vector3 startScale = new Vector3(0, 0, 0);
    Vector3 endPosition = new Vector3(0, 0, 0);
    Parallax parallax;
    float multiplier = 0.0f;
    GameObject exclamation;
    GameObject speech;

    enum State
    {
        IDLE,
        PANIC,
        DASH
    };
    State state = State.IDLE;

	// Use this for initialization
	void Start () {
        parallax = GameObject.FindGameObjectWithTag("Parallax").GetComponent<Parallax>();

        position = transform.position;
        startPosition = position;
        startScale = transform.localScale;

        endPosition = startPosition;
        endPosition.x -= startPosition.x * 0.1f;
        endPosition.y = -3;

        GetComponent<SpriteRenderer>().sprite = ScaredSprite;

        exclamation = (GameObject)Instantiate(ExclamationPrefab, transform.position + new Vector3(0, 3, 0), Quaternion.identity);
        speech = (GameObject)Instantiate(SpeechPrefab, transform.position + new Vector3(0, 3, 0), Quaternion.identity);
        speech.SetActive(false);
	}

    // Update is called once per frame
    void Update() {
    }

    // order matters, do not trust regular update
    public void DoStuff() {
        multiplier += Time.deltaTime * 0.1f;
        if (hero)
        {
            if (target || state == State.DASH)
            {
                state = State.DASH;
            }
            else
            {
                state = State.PANIC;
            }
        }
        else if (!hero)
        {
            state = State.IDLE;
            shake = 0.0f;
        }

        switch (state)
        {
            case State.PANIC:
            {
                shake += Time.deltaTime * (1 + multiplier * 2.0f);
                if (shake > 5.0f)
                {
                    state = State.DASH;
                }
                break;
            }
            case State.DASH:
            {
                if (!target)
                {
                    float progressTime = 5;//seconds
                    progress += Time.deltaTime / progressTime * (1 + multiplier / 10.0f);
                }
                if (progress > 1.0f)
                {
                    progress = 1.0f;

                    // game over
                    Object.FindObjectOfType<GameScript>().EndPhaseOne();
                }
                break;
            }
        }

        position = Vector3.Lerp(startPosition, endPosition, progress);
        switch (state)
        {
            case State.PANIC:
            {
                float shakeAmount = 0.025f;
                position.x += Random.Range(-shakeAmount, shakeAmount);
                position.y += Random.Range(-shakeAmount, shakeAmount);
                break;
            }
        }
        if (state == State.PANIC)
        {
            exclamation.SetActive(true);
        }
        else
        {
            exclamation.SetActive(false);
        }
        position.x += parallax.offset * (1 + (0.5f * progress));
        if (state == State.DASH && !target)
        {
            float hop = Mathf.Abs(Mathf.Sin(progress * 3.14159f * 1.0f * 16)) * 0.5f;
            Vector3 to = position;
            to.y = Mathf.Lerp(GetComponent<Transform>().position.y, to.y + hop, 0.4f);
            GetComponent<Transform>().position = to;
        }
        else
        {
            Vector3 to = position;
            to.y = Mathf.Lerp(GetComponent<Transform>().position.y, to.y, 0.3f);
            GetComponent<Transform>().position = to;
        }

        float distance = Mathf.Abs(startPosition.y - position.y);
        float scale = 1 + distance / 2.0f;
        GetComponent<Transform>().localScale = new Vector3(startScale.x * scale, startScale.y * scale, startScale.z);

        if (state == State.DASH && !target)
        {
            GetComponent<SpriteRenderer>().sprite = Female ? BraveSpriteFemale : BraveSprite;
            Quaternion to = Quaternion.AngleAxis(Mathf.Sin(progress * 3.14159f * 2.0f * 8 ) * 20.0f, new Vector3(0, 0, 1));
            GetComponent<Transform>().rotation = Quaternion.Slerp(GetComponent<Transform>().rotation, to, 0.3f);
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = Female ? ScaredSpriteFemale : ScaredSprite;
            GetComponent<Transform>().rotation = Quaternion.Slerp(GetComponent<Transform>().rotation, Quaternion.identity, 0.01f);
        }

        exclamation.transform.position = transform.position + new Vector3(0, 2, 0) * (1 + scale * 0.7f);
        speech.transform.position = transform.position + new Vector3(1.3f, 2 * scale, 0);
	}

    public float GetTargetParallax()
    {
        Vector3 pos = Vector3.Lerp(startPosition, endPosition, progress);
        return -pos.x / (1 + (0.5f * progress));
    }

    public void SetTalking(bool talk)
    {
        speech.SetActive(talk);
    }
}
