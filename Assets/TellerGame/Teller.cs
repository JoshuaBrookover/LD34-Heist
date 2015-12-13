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
    float shake = 0.0f;
    float progress = 0.0f;
    Vector3 position = new Vector3(0, 0, 0);
    Vector3 startPosition = new Vector3(0, 0, 0);
    Vector3 startScale = new Vector3(0, 0, 0);
    Vector3 endPosition = new Vector3(0, 0, 0);
    Parallax parallax;
    float multiplier = 1.0f;

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
	}

    // Update is called once per frame
    void Update() {
    }

    // order matters, do not trust regular update
    public void DoStuff() {
        multiplier += Time.deltaTime / 30.0f;
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
                shake += Time.deltaTime * multiplier;
                if (shake > 2.0f)
                {
                    state = State.DASH;
                }
                break;
            }
            case State.DASH:
            {
                if (!target)
                {
                    float progressTime = 2;//seconds
                    progress += Time.deltaTime / progressTime;
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
        position.x += parallax.offset * (1 + (0.5f * progress));
        if (state == State.DASH && !target)
        {
            float hop = Mathf.Abs(Mathf.Sin(progress * 3.14159f * 1.0f * 8)) * 0.5f;
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
            Quaternion to = Quaternion.AngleAxis(Mathf.Sin(progress * 3.14159f * 2.0f * 5) * 20.0f, new Vector3(0, 0, 1));
            GetComponent<Transform>().rotation = Quaternion.Slerp(GetComponent<Transform>().rotation, to, 0.3f);
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = Female ? ScaredSpriteFemale : ScaredSprite;
            GetComponent<Transform>().rotation = Quaternion.Slerp(GetComponent<Transform>().rotation, Quaternion.identity, 0.01f);
        }
	}

    public float GetTargetParallax()
    {
        Vector3 pos = Vector3.Lerp(startPosition, endPosition, progress);
        return -pos.x / (1 + (0.5f * progress));
    }
}
