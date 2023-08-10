using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class BorderBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject Spawner;

    private Spawner SpawnerScript;
    private Vector3 lastPosition;
    private float desiredScale = 3;
    private float currentDistanceToFigure;
    void Start()
    {
        Spawner = GameObject.Find("Spawner");
        SpawnerScript = Spawner.GetComponent<Spawner>();
        lastPosition = SpawnerScript.Figure.transform.position;
        transform.localScale = new Vector3(1, 1, 0);
        currentDistanceToFigure = CalculateDistance();
    }

    void Update()
    {
        if (SpawnerScript.Figure != null && lastPosition.y != SpawnerScript.Figure.transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, SpawnerScript.Figure.transform.Cast<Transform>().Average(child => child.position.y), 0);
            lastPosition = SpawnerScript.Figure.transform.position;
        }
        if (SpawnerScript.Figure != null && lastPosition.x != SpawnerScript.Figure.transform.position.x)
        {
            currentDistanceToFigure = CalculateDistance();
            lastPosition = SpawnerScript.Figure.transform.position;
        }
        if (desiredScale / Mathf.Pow(currentDistanceToFigure, 2) >= transform.localScale.y)
            transform.localScale += new Vector3(0, 0.1f * transform.localScale.y, 0);
        else
            transform.localScale -= new Vector3(0, 0.1f * transform.localScale.y, 0);
    }

    private int CalculateDistance()
    {
        int minXOfFigure = 5;
        int maxXOfFigure = -5;
        foreach (Transform child in SpawnerScript.Figure.transform)
        {
            minXOfFigure = minXOfFigure >= child.transform.position.x ? (int)child.transform.position.x : minXOfFigure;
            maxXOfFigure = maxXOfFigure <= child.transform.position.x ? (int)child.transform.position.x : maxXOfFigure;
        }
        MakeInvisible(transform.position.x < 0 ? minXOfFigure + 5 : 5 - maxXOfFigure);
        return transform.position.x < 0 ? minXOfFigure + 5 : 5 - maxXOfFigure;
    }

    private void MakeInvisible(int distance)
    {
        Color color = GetComponent<SpriteRenderer>().color;
        if (distance > 4)
            color.a = 0f;
        else 
            color.a = 0.6f;
        GetComponent<SpriteRenderer>().color = color;
    }
}
