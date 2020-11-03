using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    // Button attached to this script
    private Button button;

    // Pause Screen
    public GameObject pauseMenu;

    // Game UI
    public GameObject gameScreen;

    // Conductor for this song
    private Conductor conductor;

    // Current song
    private SongInformation song;

    // Start is called before the first frame update
    void Start()
    {
        conductor = GameObject.Find("Conductor").GetComponent<Conductor>();
        song = GameObject.Find("Notes").GetComponent<SongInformation>();

        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(PauseGame);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Pauses the game
    // Add pause menu stuff
    void PauseGame()
    {
        conductor.musicSource.Pause();
        conductor.paused = true;
        AudioListener.pause = true;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        gameScreen.SetActive(false);
    }
}
