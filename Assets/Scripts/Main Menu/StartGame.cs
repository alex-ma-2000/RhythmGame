using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    // The button this script is attached to
    private Button button;

    public GameObject titleScreen;
    public GameObject chapterSelect;

    private MainMenu mainMenu;

    // Start is called before the first frame update
    void Start()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(EnterGame);

        mainMenu = GameObject.Find("Main Menu").GetComponent<MainMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EnterGame()
    {
        // Brings user from title screen to song select
        Debug.Log("Game Started");

        mainMenu.setPrevCanvas(titleScreen);
        titleScreen.SetActive(false);
        chapterSelect.SetActive(true);
    }
}
