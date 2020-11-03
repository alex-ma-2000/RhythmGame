using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SongSelect : MonoBehaviour
{
    // Song ID corresponding to the specific song
    public int songID;

    // Button attached to this script
    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(SongStart);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Changes the scene to the song with everything configured.
    void SongStart()
    {
        // This will be the method to load songs based off of IDs after there are more songs (using scene indexes)
        SceneManager.LoadScene(songID);

        //SceneManager.LoadScene("Main Menu");
    }
}
