using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Restart : MonoBehaviour
{
    // Button attached to this script
    private Button button;

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
        button.onClick.AddListener(RestartSong);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Restarts the song
    void RestartSong()
    {
        conductor.RestartSong();
    }
}
