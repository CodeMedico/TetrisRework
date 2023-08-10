using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScorePopUp : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float fadeSpeed = 1f;
    public float destroyDelay = 1f;

    private Lines lines;
    private GameObject buba;
    private GameObject gameCanvas;

    private Text scoreText;
    private Color textColor;
    private Vector3 startPosition;
    

    void Start()
    {
        gameCanvas = GameObject.Find("CanvasGame");
        buba = GameObject.Find("buba");
        scoreText = GetComponent<Text>();
        textColor = scoreText.color;
        startPosition = transform.position + new Vector3(0,2,0);
        lines = buba.GetComponent<Lines>();
        SetScore(lines.linesInRow* lines.linesInRow*10 * (lines.difficulty > 0 ? lines.difficulty : 1));
        transform.SetParent(gameCanvas.transform, false);
        textColor.a = 1f;
    }

    public void SetScore(int score)
    {
        scoreText.text = "+" + score.ToString();
    }

    void Update()
    {
        // Move up
        startPosition += new Vector3(0,moveSpeed * Time.deltaTime,0);
        transform.position = startPosition;

        // Fade out
        textColor.a -= fadeSpeed * Time.deltaTime;
        scoreText.color = textColor;

        // Destroy after delay
        Destroy(gameObject, destroyDelay);
    }
}

