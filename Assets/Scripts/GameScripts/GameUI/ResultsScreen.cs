using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultsScreen : MonoBehaviour
{
    // Text UI Elements
    public TMP_Text perfect;
    public TMP_Text great;
    public TMP_Text good;
    public TMP_Text miss;
    public TMP_Text score;
    public TMP_Text maxCombo;
    public Image rank;

    // Rank Sprites
    public Sprite ss;
    public Sprite s;
    public Sprite a;
    public Sprite b;
    public Sprite c;
    public Sprite d;

    // Conductor Class
    private Conductor conductor;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        conductor = GameObject.Find("Conductor").GetComponent<Conductor>();
    }

    // Update is called once per frame
    void Update()
    {
        if (conductor.IsSongOver())
        {
            perfect.text = conductor.getPerfects().ToString();
            great.text = conductor.getGreats().ToString();
            good.text = conductor.getGoods().ToString();
            miss.text = conductor.getMisses().ToString();
            score.text = ((int)conductor.GetScore()).ToString();
            maxCombo.text = conductor.getHighestCombo().ToString();
            
            // Rank determining 
            if (conductor.GetScore() == 1000000)
            {
                rank.sprite = ss;
            }
            else if (conductor.GetScore() >= 950000)
            {
                rank.sprite = s;
            }
            else if (conductor.GetScore() >= 900000)
            {
                rank.sprite = a;
            }
            else if (conductor.GetScore() >= 800000)
            {
                rank.sprite = b;
            }
            else if (conductor.GetScore() >= 700000)
            {
                rank.sprite = c;
            }
            else if (conductor.GetScore() < 700000)
            {
                rank.sprite = d;
            }
        }
    }
}
