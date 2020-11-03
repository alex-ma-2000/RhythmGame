using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ToSongSelect : MonoBehaviour
{
    // Button attached to this script
    private Button button;

    // Conductor Class
    private Conductor conductor;

    // Start is called before the first frame update
    void Start()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(SongList);

        conductor = GameObject.Find("Conductor").GetComponent<Conductor>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Returns the player to song select
    void SongList()
    {
        conductor.paused = false;
        AudioListener.pause = false;
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
    }
}
