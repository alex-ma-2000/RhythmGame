using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
    private Text text;

    private Conductor conductor;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        conductor = GameObject.Find("Conductor").GetComponent<Conductor>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = ("Score: " + (int)conductor.GetScore()) + "\nCombo: " + conductor.GetCombo();

        if (conductor.IsSongOver())
        {
            gameObject.SetActive(false);
        }
    }
}
