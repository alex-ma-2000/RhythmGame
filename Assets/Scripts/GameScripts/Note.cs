using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    // Camera
    private new Camera camera;

    // Conductor Class
    private Conductor conductor;

    // SongInformation
    private SongInformation song;

    // Determines if the note is hittable
    private bool hittable = false;

    // Start and End Positions
    public Transform startPos;
    public Transform endPos;

    // Note Hit Prefabs
    public GameObject perfect;
    public GameObject great;
    public GameObject good;
    public GameObject miss;

    // Beat of this note
    private float beat;

    // Hit field for the track
    private GameObject hitField; 

    // Start is called before the first frame update
    void Start()
    {
        conductor = GameObject.Find("Conductor").GetComponent<Conductor>();
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        song = GameObject.Find("Notes").GetComponent<SongInformation>();

        // Sets start and end positions
        startPos = transform.parent.GetComponent<Transform>();
        endPos = startPos.Find("EndPos").transform;

        // Calculates the beat of this note
        beat = conductor.songPositionInBeats + song.beatsShownInAdvance;
    }

    // Update is called once per frame
    void Update()
    {
        if (!conductor.paused)
        {
            // Registering Hits
            if (Input.touchCount > 0 && hittable)
            {
                /* Only can hit closest note (should implement later)
                Note[] notes = startPos.GetComponentsInChildren<Note>();
                GameObject hittableNote = notes[0].gameObject;

                for (int i = 1; i < notes.Length; i++)
                {
                    if (Vector3.Distance(notes[i].transform.position, endPos.position) < Vector3.Distance(hittableNote.transform.position, endPos.position))
                    {
                        hittableNote = notes[i].gameObject;
                    }
                } */

                Touch touch;
                Touch[] touches = Input.touches;
                for (int i = 0; i < touches.Length; i++)
                {
                    touch = touches[i];
                    Vector2 touchPos = camera.ScreenToWorldPoint(touch.position);
                    if (hitField != null && hitField.GetComponent<BoxCollider2D>().bounds.Contains(touchPos) && touch.phase == TouchPhase.Began)
                    {
                        hitNote();
                    }
                }
            }

            // Moving Notes
            transform.position = Vector2.Lerp(startPos.position, endPos.position, (song.beatsShownInAdvance - (beat - conductor.songPositionInBeats)) / song.beatsShownInAdvance);

            // Calculates Misses
            if (transform.position == endPos.position && gameObject.activeSelf)
            {
                // Create a Coroutine that gives the player a small window of time to hit the note b4 it misses. Make the note linger b4 getting destroyed.
                StartCoroutine(OnBeat());
            }
        }
    }

    void hitNote()
    {
        //gameObject.SetActive(false);
        Destroy(gameObject);
        //Debug.Log("hit");

        // Determines Perfects/Greats/Goods
        float beatsOff = Mathf.Abs(beat - conductor.songPositionInBeats);

        if (beatsOff < 0.15f)
        {
            conductor.HitPerfect();
            Instantiate(perfect, new Vector3(transform.position.x, transform.position.y, -3), Quaternion.identity, startPos);
        } 
        else if (beatsOff <= 0.25f)
        {
            conductor.HitGreat();
            Instantiate(great, new Vector3(transform.position.x, transform.position.y, -3), Quaternion.identity, startPos);
        }
        else if (beatsOff > 0.25f)
        {
            conductor.HitGood();
            Instantiate(good, new Vector3(transform.position.x, transform.position.y, -3), Quaternion.identity, startPos);
        }
    }

    IEnumerator OnBeat()
    {
        Color c = gameObject.GetComponent<SpriteRenderer>().color;
        c.a = 0f;
        gameObject.GetComponent<SpriteRenderer>().color = c;
        yield return new WaitForSeconds(conductor.secPerBeat/2);
        conductor.Miss();
        // Create a gameObject to store the position of where hit text will be.
        Instantiate(miss, new Vector3(transform.position.x, transform.position.y, -3), Quaternion.identity, startPos);
        Destroy(gameObject);
        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Activator")
        {
            hittable = true;
            hitField = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Activator")
        {
            hittable = false;
            hitField = null;
        }
    }
}
