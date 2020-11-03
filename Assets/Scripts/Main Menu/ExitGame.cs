using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitGame : MonoBehaviour
{
    // Button attached to this script
    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(Quit);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Quits the application
    void Quit()
    {
        Application.Quit();
        Debug.Log("Exiting Game");
    } 
}
