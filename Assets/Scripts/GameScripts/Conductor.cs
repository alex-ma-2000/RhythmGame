using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Conductor : MonoBehaviour
{
    // Song beat per minute
    public float songBpm;

    // The number of seconds for each song beat
    public float secPerBeat;
    public float beatTempo;

    // Current song position, in seconds
    public float songPosition;

    // Current song position, in beats -> begins at 0
    public float songPositionInBeats;

    // How many seconds have passed since the song started
    public float dspSongTime;

    // An AudioSource attached to this GameObject that will play the music
    public AudioSource musicSource;

    // Offset for the first beat of the song in seconds
    public float firstBeatOffset;

    // Delay before game starts
    public float countdown;

    // Is the song over?
    [SerializeField]
    private bool songOver = false;
    public GameObject results;
    public GameObject gameUI;
    public GameObject pauseMenu;

    // Is the game paused?
    public bool paused;

    [SerializeField]
    // Score variables
    private float score;
    [SerializeField]
    private int multiplier;
    [SerializeField]
    private int numberofMultipliers;
    [SerializeField]
    private int combo;
    [SerializeField]
    private int maxCombo;
    [SerializeField]
    private float pointsPerNote;
    [SerializeField]
    private float currentPerfectValue;

    // Stat Variables
    private int perfects;
    private int greats;
    private int goods;
    private int misses;
    private int highestCombo;

    // Start is called before the first frame update
    void Start()
    {
        // Load the AudioSource attached to the Conductor GameObject
        musicSource = GetComponent<AudioSource>();

        // Calculate the number of seconds in each beat
        secPerBeat = 60f / songBpm;
        beatTempo = songBpm / 60f;

        // Record the time when the music starts
        dspSongTime = (float)AudioSettings.dspTime;

        paused = false;
        AudioListener.pause = false;
        Time.timeScale = 1;
        musicSource.PlayDelayed(countdown);

        numberofMultipliers = ((int)Mathf.Floor(maxCombo / 25f)) + 1;

        CalculatePointsPerNote();

        gameUI.SetActive(true);
        results.SetActive(false);
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            // Calculates seconds since song started
            songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset - countdown);

            // Calculates how many beats since the song started
            songPositionInBeats = songPosition / secPerBeat;

            // Raises Multiplier based off of current Combo
            multiplier = (int)Mathf.Floor(combo / 25f) + 1;

            // Updates Highest Combo
            if (highestCombo < combo)
            {
                highestCombo = combo;
            }

            if (!musicSource.isPlaying)
            {
                songOver = true;
                results.SetActive(true);
            }

            // Developer Force Song End
            if (Input.GetKeyDown(KeyCode.L))
            {
                musicSource.Stop();
            }

            // Developer Force Restart
            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartSong();
            }
        }

        currentPerfectValue = pointsPerNote * (Mathf.Log(multiplier + 3, 4));
    }

    // Perfect Hits
    public void HitPerfect()
    {
        score += pointsPerNote * (Mathf.Log(multiplier + 3, 4));
        combo++;
        perfects++;
    }

    // Great Hits
    public void HitGreat()
    {
        score += pointsPerNote * (Mathf.Log(multiplier + 3, 4)) * 0.8f;
        combo++;
        greats++;
    }

    // Good Hits
    public void HitGood()
    {
        score += pointsPerNote * (Mathf.Log(multiplier + 3, 4)) * 0.5f;
        combo++;
        goods++;
    }

    public float GetScore()
    {
        return score;
    }

    public float GetMultiplier()
    {
        return multiplier;
    }

    public void SetMaxCombo(int max)
    {
        maxCombo = max;
    }

    public int GetCombo()
    {
        return combo;
    }

    public void Miss()
    {
        combo = 0;
        misses++;
    }

    private void CalculatePointsPerNote()
    {
        float total;
        total = (maxCombo % 25) * numberofMultipliers;
        for (int i = numberofMultipliers - 1; i > 0; i--)
        {
            total += (25 * (Mathf.Log(i + 3, 4)));
        }

        pointsPerNote = 1000000f / total;
    }

    public bool IsSongOver()
    {
        return songOver;
    }

    public int getPerfects()
    {
        return perfects;
    }

    public int getGreats()
    {
        return greats;
    }

    public int getGoods()
    {
        return goods;
    }

    public int getMisses()
    {
        return misses;
    }

    public int getHighestCombo()
    {
        return highestCombo;
    }

    public void RestartSong()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        Debug.Log("Restarted");
    }
}
