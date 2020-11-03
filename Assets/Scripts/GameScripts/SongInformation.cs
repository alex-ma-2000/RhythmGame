using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class SongInformation : MonoBehaviour
{
    // Spawn Positions for each track
    public Transform spawnPos1;
    public Transform spawnPos2;
    public Transform spawnPos3;
    public Transform spawnPos4;

    // Note Totals and index for overall number of notes
    public List<(float, int)> notes = new List<(float, int)>();
    public List<(float, float, int)> holdNotes = new List<(float, float, int)>();
    private int nextIndex = 0;
    private int holdIndex = 0;

    // How many beats before a note is spawned
    public float beatsShownInAdvance;

    // Amount of World Space traveled in 1 beat
    public float worldSpacePerBeat;

    // Music Note Prefabs
    public GameObject rightNote;
    public GameObject leftNote;
    public GameObject holdRight;
    public GameObject holdLeft;

    // Conductor Class
    private Conductor conductor;

    // Song text File
    public TextAsset song;

    void Awake()
    {
        conductor = this.GetComponentInParent<Conductor>();

        // Reads song text file and produces a list of beats and tracks
        string[] songArray = song.text.Split('\n');
        for (int i = 0; i < songArray.Length; i++)
        {
            (float, int) beat;
            (float, float, int) holdBeat;
            string[] split = songArray[i].Split(',');
            if (split.Length == 2) 
            {
                beat = (float.Parse(split[0]), int.Parse(split[1]));
                notes.Add(beat);
            }
            if (split.Length == 3)
            {
                holdBeat = (float.Parse(split[0]), float.Parse(split[1]), int.Parse(split[2]));
                holdNotes.Add(holdBeat);
            }
        }

        // Initializes the maxCombo of the song
        conductor.SetMaxCombo(notes.Count + (holdNotes.Count * 2));
    }

    // Update is called once per frame
    void Update()
    {
        if (!conductor.paused)
        {
            // Spawns Normal Notes
            if (nextIndex < notes.Count && notes.ElementAt(nextIndex).Item1 < conductor.songPositionInBeats + beatsShownInAdvance)
            {
                switch (notes.ElementAt(nextIndex).Item2)
                {
                    case 1:
                        Instantiate(rightNote, spawnPos1.position, Quaternion.identity, spawnPos1.transform);
                        nextIndex++;
                        break;
                    case 2:
                        Instantiate(rightNote, spawnPos2.position, Quaternion.identity, spawnPos2.transform);
                        nextIndex++;
                        break;
                    case 3:
                        Instantiate(leftNote, spawnPos3.position, Quaternion.identity, spawnPos3.transform);
                        nextIndex++;
                        break;
                    case 4:
                        Instantiate(leftNote, spawnPos4.position, Quaternion.identity, spawnPos4.transform);
                        nextIndex++;
                        break;
                }
            }

            // Spawns Hold Notes
            if (holdIndex < holdNotes.Count && holdNotes.ElementAt(holdIndex).Item1 < conductor.songPositionInBeats + beatsShownInAdvance)
            {
                float startEndDist;
                float beatDiffs;

                GameObject note;
                Transform[] sprites;

                switch (holdNotes.ElementAt(holdIndex).Item3)
                {
                    case 1:
                        startEndDist = Mathf.Abs(Vector3.Distance(spawnPos1.transform.position, spawnPos1.GetChild(0).transform.position));
                        worldSpacePerBeat = startEndDist / beatsShownInAdvance;
                        beatDiffs = holdNotes.ElementAt(holdIndex).Item2 - holdNotes.ElementAt(holdIndex).Item1;

                        note = Instantiate(holdRight, spawnPos1.position, Quaternion.identity, spawnPos1.transform);
                        sprites = note.GetComponentsInChildren<Transform>();
                        for (int i = 0; i < sprites.Length; i++)
                        {
                            // Scaling the middle note to be stretch between the ends
                            if (sprites[i].name == "HoldMiddle")
                            {
                                // Move to midpoint between ends
                                float middlePos = worldSpacePerBeat * beatDiffs / 2;
                                sprites[i].transform.localPosition = new Vector3(-middlePos, 0f, 1f);
                                // Scale by 10 for every beatsShownInAdvance
                                sprites[i].transform.localScale = new Vector3(10 * (beatDiffs / beatsShownInAdvance), 1f, 1f);
                            }

                            // Moves the end note to the correct position
                            if (sprites[i].name == "HoldEnd")
                            {
                                GameObject end = sprites[i].gameObject;

                                end.transform.position = new Vector3(spawnPos1.position.x - worldSpacePerBeat * beatDiffs, spawnPos1.position.y, spawnPos1.position.z);
                            }
                        }
                        holdIndex++;
                        break;
                    case 2:
                        startEndDist = Mathf.Abs(Vector3.Distance(spawnPos2.transform.position, spawnPos2.GetChild(0).transform.position));
                        worldSpacePerBeat = startEndDist / beatsShownInAdvance;
                        beatDiffs = holdNotes.ElementAt(holdIndex).Item2 - holdNotes.ElementAt(holdIndex).Item1;

                        note = Instantiate(holdRight, spawnPos2.position, Quaternion.identity, spawnPos2.transform);
                        sprites = note.GetComponentsInChildren<Transform>();
                        for (int i = 0; i < sprites.Length; i++)
                        {
                            // Scaling the middle note to be stretch between the ends
                            if (sprites[i].name == "HoldMiddle")
                            {
                                // Move to midpoint between ends
                                float middlePos = worldSpacePerBeat * beatDiffs / 2;
                                sprites[i].transform.localPosition = new Vector3(-middlePos, 0f, 1f);
                                // Scale by 10 for every beatsShownInAdvance
                                sprites[i].transform.localScale = new Vector3(10 * (beatDiffs / beatsShownInAdvance), 1f, 1f);
                            }

                            // Moves the end note to the correct position
                            if (sprites[i].name == "HoldEnd")
                            {
                                GameObject end = sprites[i].gameObject;

                                end.transform.position = new Vector3(spawnPos2.position.x - worldSpacePerBeat * beatDiffs, spawnPos2.position.y, spawnPos2.position.z);
                            }
                        }
                        holdIndex++;
                        break;
                    case 3:
                        startEndDist = Mathf.Abs(Vector3.Distance(spawnPos3.transform.position, spawnPos3.GetChild(0).transform.position));
                        worldSpacePerBeat = startEndDist / beatsShownInAdvance;
                        beatDiffs = holdNotes.ElementAt(holdIndex).Item2 - holdNotes.ElementAt(holdIndex).Item1;

                        note = Instantiate(holdLeft, spawnPos3.position, Quaternion.identity, spawnPos3.transform);
                        sprites = note.GetComponentsInChildren<Transform>();
                        for (int i = 0; i < sprites.Length; i++)
                        {
                            // Scaling the middle note to be stretch between the ends
                            if (sprites[i].name == "HoldMiddle")
                            {
                                // Move to midpoint between ends
                                float middlePos = worldSpacePerBeat * beatDiffs / 2;
                                sprites[i].transform.localPosition = new Vector3(middlePos, 0f, 1f);
                                // Scale by 10 for every beatsShownInAdvance
                                sprites[i].transform.localScale = new Vector3(10 * (beatDiffs / beatsShownInAdvance), 1f, 1f);
                            }

                            // Moves the end note to the correct position
                            if (sprites[i].name == "HoldEnd")
                            {
                                GameObject end = sprites[i].gameObject;

                                end.transform.position = new Vector3(spawnPos3.position.x + worldSpacePerBeat * beatDiffs, spawnPos3.position.y, spawnPos3.position.z);
                            }
                        }
                        holdIndex++;
                        break;
                    case 4:
                        startEndDist = Mathf.Abs(Vector3.Distance(spawnPos4.transform.position, spawnPos4.GetChild(0).transform.position));
                        worldSpacePerBeat = startEndDist / beatsShownInAdvance;
                        beatDiffs = holdNotes.ElementAt(holdIndex).Item2 - holdNotes.ElementAt(holdIndex).Item1;

                        note = Instantiate(holdLeft, spawnPos4.position, Quaternion.identity, spawnPos4.transform);
                        sprites = note.GetComponentsInChildren<Transform>();
                        for (int i = 0; i < sprites.Length; i++)
                        {
                            // Scaling the middle note to be stretch between the ends
                            if (sprites[i].name == "HoldMiddle")
                            {
                                // Move to midpoint between ends
                                float middlePos = worldSpacePerBeat * beatDiffs / 2;
                                sprites[i].transform.localPosition = new Vector3(middlePos, 0f, 1f);
                                // Scale by 10 for every beatsShownInAdvance
                                sprites[i].transform.localScale = new Vector3(10 * (beatDiffs / beatsShownInAdvance), 1f, 1f);
                            }

                            // Moves the end note to the correct position
                            if (sprites[i].name == "HoldEnd")
                            {
                                GameObject end = sprites[i].gameObject;

                                end.transform.position = new Vector3(spawnPos4.position.x + worldSpacePerBeat * beatDiffs, spawnPos4.position.y, spawnPos4.position.z);
                            }
                        }
                        holdIndex++;
                        break;
                }
            }
        }
    }
}
