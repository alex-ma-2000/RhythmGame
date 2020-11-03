using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChapterSelect : MonoBehaviour
{
    // Button attached to this script
    private Button button;

    // Number of the chapter for songlists.
    public int chapterNumber;
    public GameObject chapterSelect;
    public GameObject songSelect;

    // Text Object for this button
    public TMP_Text text;

    private MainMenu mainMenu;

    // Start is called before the first frame update
    void Start()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(SongList);

        mainMenu = GameObject.Find("Main Menu").GetComponent<MainMenu>();

        text.text = text.text + " " + chapterNumber;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Opens the song list of this chapter
    void SongList()
    {
        Debug.Log("SongSelect");

        mainMenu.setPrevCanvas(chapterSelect);
        chapterSelect.SetActive(false);

        // Add Song List Loading depending on the chapter
        songSelect.SetActive(true);
    }
}
