using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject prevCanvas;

    // Screens
    public GameObject titleScreen;
    public GameObject chapterSelect;
    public GameObject songSelect;

    // Start is called before the first frame update
    void Start()
    {
        titleScreen.SetActive(true);
        chapterSelect.SetActive(false);
        songSelect.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setPrevCanvas(GameObject canvas)
    {
        prevCanvas = canvas;
    }

    public GameObject getPrevCanvas()
    {
        return prevCanvas;
    }
}
