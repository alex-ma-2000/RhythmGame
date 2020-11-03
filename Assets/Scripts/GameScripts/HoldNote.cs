using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class HoldNote : MonoBehaviour
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
    public Vector3 startPosEndNote;
    public Transform endPos;
    public Vector3 startPosMiddle;

    private Vector3 currentPos = new Vector3(0f,0f,0f);
    private Vector3 endPosMiddle;

    // Beat of this note
    [SerializeField]
    private float beatStart;
    [SerializeField]
    private float beatEnd;
    private float beatDelay;

    // Note Objects
    public GameObject startNote;
    public GameObject middle;
    public GameObject endNote;

    // Note Direction
    public bool positive; // If positive; up or right

    // Note Hit Sprites
    public GameObject perfect;
    public GameObject great;
    public GameObject good;
    public GameObject miss;

    // Hit field for the track
    private GameObject hitField;

    // Is a touch being held?
    private bool holding;

    // Start is called before the first frame update
    void Start()
    {
        conductor = GameObject.Find("Conductor").GetComponent<Conductor>();
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        song = GameObject.Find("Notes").GetComponent<SongInformation>();

        // Sets start and end positions
        startPos = transform.parent.GetComponent<Transform>();
        startPosEndNote = new Vector3(endNote.transform.position.x, endNote.transform.position.y, endNote.transform.position.z);
        startPosMiddle = new Vector3(middle.transform.position.x, middle.transform.position.y, middle.transform.position.z);
        endPos = startPos.Find("EndPos").transform;

        // Sets the Sorting Layer to the track number for all children
        startNote.GetComponent<SpriteRenderer>().sortingOrder = int.Parse(startPos.name.Substring(8));
        middle.GetComponent<SpriteRenderer>().sortingOrder = int.Parse(startPos.name.Substring(8));
        endNote.GetComponent<SpriteRenderer>().sortingOrder = int.Parse(startPos.name.Substring(8));

        // Calculates the start beat of this note
        beatStart = conductor.songPositionInBeats + song.beatsShownInAdvance;

        // Calculates the end beat of this note
        float startEndDist = Mathf.Abs(Vector3.Distance(startPos.position, endPos.position));
        float worldSpacePerBeat = startEndDist / song.beatsShownInAdvance;
        beatEnd = Vector3.Distance(startNote.transform.position, endNote.transform.position) / worldSpacePerBeat + beatStart;

        beatDelay = beatEnd - beatStart;

        // Determines if the note is moving in the positive or negative x direction
        if (startPos.gameObject.name.Equals("SpawnPos1") || startPos.gameObject.name.Equals("SpawnPos2"))
        {
            positive = true;
            endPosMiddle = new Vector3(endPos.transform.position.x + beatDelay * worldSpacePerBeat / 2, endPos.transform.position.y, 1f);
        } 
        else
        {
            positive = false;
            endPosMiddle = new Vector3(endPos.transform.position.x - beatDelay * worldSpacePerBeat / 2, endPos.transform.position.y, 1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!conductor.paused)
        {

            // Registering Hits
            if (Input.touchCount > 0 && hittable)
            {
                Touch touch;
                Touch[] touches = Input.touches;
                for (int i = 0; i < touches.Length; i++)
                {
                    touch = touches[i];
                    Vector2 touchPos = camera.ScreenToWorldPoint(touch.position);
                    switch (touch.phase)
                    {
                        case TouchPhase.Began:
                            if (hitField != null && hitField.GetComponent<BoxCollider2D>().bounds.Contains(touchPos))
                            {
                                hitNote();
                                holding = true;
                                //StartCoroutine("Holding");
                            }
                            break;
                        case TouchPhase.Ended:
                            if (hitField != null && hitField.GetComponent<BoxCollider2D>().bounds.Contains(touchPos))
                            {
                                holding = false;
                                hitEndNote();
                            }
                            else if (hitField != null && hitField.GetComponent<BoxCollider2D>().bounds.Contains(touchPos)) // Miss Case
                            {
                                StartCoroutine(OnBeat());
                            }
                            break;
                    }
                }
            }

            // Moving Notes
            /*
            if (beatStart - conductor.songPositionInBeats >= 0)
            {
                transform.position = Vector2.Lerp(startPos.position, endPos.position, (song.beatsShownInAdvance - (beatStart - conductor.songPositionInBeats)) / song.beatsShownInAdvance);
                currentPos = transform.position;
            }
            if (startNote.activeSelf == false)
            {
                transform.position = Vector2.Lerp(currentPos, endPos.position, (song.beatsShownInAdvance - (beatEnd - conductor.songPositionInBeats)) / song.beatsShownInAdvance);
            }*/
            startNote.transform.position = Vector2.Lerp(startPos.position, endPos.position, (song.beatsShownInAdvance - (beatStart - conductor.songPositionInBeats)) / song.beatsShownInAdvance);
            endNote.transform.position = Vector2.Lerp(startPosEndNote, endPos.position,
                (song.beatsShownInAdvance + beatDelay - (beatEnd - conductor.songPositionInBeats)) / (song.beatsShownInAdvance + beatDelay));
            middle.transform.position = Vector3.Lerp(startPosMiddle, endPosMiddle,
                (song.beatsShownInAdvance + beatDelay - (beatEnd - conductor.songPositionInBeats)) / (song.beatsShownInAdvance + beatDelay));

            // Calculates Misses
            if (Vector3.Distance(startNote.transform.position, endPos.position) <= 0.01f && startNote.activeSelf)
            {
                conductor.Miss();
                conductor.Miss();
                Instantiate(miss, new Vector3(startNote.transform.position.x, transform.position.y, -3), Quaternion.identity, conductor.transform);
                Instantiate(miss, new Vector3(endNote.transform.position.x, transform.position.y, -3), Quaternion.identity, conductor.transform);
                Destroy(gameObject);
            }
            else if (Vector3.Distance(endNote.transform.position, endPos.position) <= 0.01f)
            {
                StartCoroutine(OnBeat());
            }
        }
    }

    void hitNote()
    {
        //gameObject.SetActive(false);
        startNote.SetActive(false);
        //Debug.Log("hit");

        // Determines Perfects/Greats/Goods
        float beatsOff = Mathf.Abs(beatStart - conductor.songPositionInBeats);

        if (beatsOff < 0.15f)
        {
            conductor.HitPerfect();
            Instantiate(perfect, new Vector3(startNote.transform.position.x, startNote.transform.position.y, -3), Quaternion.identity, startPos);
        }
        else if (beatsOff <= 0.25f)
        {
            conductor.HitGreat();
            Instantiate(great, new Vector3(startNote.transform.position.x, startNote.transform.position.y, -3), Quaternion.identity, startPos);
        }
        else if (beatsOff > 0.25f)
        {
            conductor.HitGood();
            Instantiate(good, new Vector3(startNote.transform.position.x, startNote.transform.position.y, -3), Quaternion.identity, startPos);
        }
    }

    IEnumerator Holding()
    {
        while (holding)
        {
            // Can be negative distance?
            // This seems really wrong
            float startEndDist = Vector3.Distance(startPos.position, endPos.position);
            float worldSpacePerSecond = startEndDist / conductor.secPerBeat;
            // Move to adjust for scaling 
            if (positive) // Moving Right or Up
            {
                 //middle.transform.localPosition -= new Vector3(worldSpacePerSecond * 0.1f, 0f, 0f );
            } 
            else // Moving Left or Down
            {
                middle.transform.localPosition += new Vector3(worldSpacePerSecond * 0.1f, 0f, 0f);
            }
            // Scale smaller
            middle.transform.localScale -= new Vector3(worldSpacePerSecond/7.5f * Time.deltaTime, 0f, 0f);
            yield return null;
        }
    }

    void hitEndNote()
    {
        //gameObject.SetActive(false);
        endNote.SetActive(false);
        //Debug.Log("hit");

        // Determines Perfects/Greats/Goods
        float beatsOff = Mathf.Abs(beatEnd - conductor.songPositionInBeats);

        if (beatsOff < 0.15f)
        {
            conductor.HitPerfect();
            Instantiate(perfect, new Vector3(endNote.transform.position.x, endNote.transform.position.y, -3), Quaternion.identity, startPos);
        }
        else if (beatsOff <= 0.25f)
        {
            conductor.HitGreat();
            Instantiate(great, new Vector3(endNote.transform.position.x, endNote.transform.position.y, -3), Quaternion.identity, startPos);
        }
        else if (beatsOff > 0.25f)
        {
            conductor.HitGood();
            Instantiate(good, new Vector3(endNote.transform.position.x, endNote.transform.position.y, -3), Quaternion.identity, startPos);
        }
        Destroy(gameObject);
    }

    IEnumerator OnBeat()
    {
        Color c = endNote.GetComponent<SpriteRenderer>().color;
        c.a = 0f;
        endNote.GetComponent<SpriteRenderer>().color = c;
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
