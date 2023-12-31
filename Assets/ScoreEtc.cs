using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreEtc : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI linesText;
    public TextMeshProUGUI difficultyText;
    public GameObject bricks;
    private Lines Linesscript;

    void Start()
    {
        bricks = GameObject.Find("buba");
        Linesscript = bricks.GetComponent<Lines> ();
    }

    public void UpdateData()
    {
        scoreText.text = Linesscript.score.ToString();
        linesText.text = Linesscript.linesCount.ToString();
        difficultyText.text = Linesscript.difficulty.ToString();
    }
}
