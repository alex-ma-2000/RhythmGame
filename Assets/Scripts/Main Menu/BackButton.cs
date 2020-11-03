using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackButton : MonoBehaviour
{
    // Button attached to this script
    private Button button;

    // GameObject of the screen this button is on
    public GameObject thisScreen;

    // Previous Screen connected to this screen
    public GameObject prevScreen;

    private MainMenu mainMenu;
    // Start is called before the first frame update
    void Start()
    {
        button = gameObject.GetComponent<Button>();
        mainMenu = GameObject.Find("Main Menu").GetComponent<MainMenu>();
        button.onClick.AddListener(Back);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Changes the current screen to the previous screen
    void Back()
    {
        thisScreen.SetActive(false);
        prevScreen.SetActive(true);
    }
}
