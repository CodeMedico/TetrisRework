using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Lines : MonoBehaviour
{
    public GameObject scorePopUpPrefab;
    public GameObject Spawner;

    private GameObject EventSystem;
    private EventsControl EventsControlScript;

    private List<List<Transform>> lines = new List<List<Transform>>();
    private int axis = 1;
    private float[] Ypos;
    private bool[] LineIs;
    private Transform[] children;
    private int jcount;
    private Spawner SpawnerScript;

    public int linesInRow;
    public int score { get; set; }
    public int linesCount { get; set; }
    public int difficulty { get; set; }
    void Start()
    {
        score = 0;
        linesCount = 0;
        EventSystem = GameObject.Find("EventSystem");
        EventsControlScript = EventSystem.GetComponent<EventsControl>();
        SpawnerScript = Spawner.GetComponent<Spawner>();
        Ypos = new float[] { -9.5f, -8.5f, -7.5f, -6.5f, -5.5f, -4.5f, -3.5f, -2.5f, -1.5f, -0.5f, 0.5f, 1.5f, 2.5f, 3.5f, 4.5f, 5.5f, 6.5f, 7.5f, 8.5f, 9.5f };
        LineIs = new bool[21];
    }
    public void CheckLines()
    {
        LineIs = new bool[] { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
        jcount = 0;
        linesInRow = 0;
        children = GetComponentsInChildren<Transform>();
        for (int i = 0; i < children.Length; i++)
            if (children[i].transform.position.y > 9.6f)
            {
                EventsControlScript.Pause();
                EventsControlScript.Loose();
                break;
            }
        lines.Clear();
        for (int i = 0; i < Ypos.Length; i++)
        {
            List<Transform> line = new List<Transform>();
            for (int j = 0; j < children.Length; j++)
                if (Ypos[i] - 0.5f < children[j].transform.position.y && children[j].transform.position.y < Ypos[i] + 0.5f && children[j].name == "block")
                    line.Add(children[j]);
            lines.Add(line);
        }
        for (int i = 0; i < lines.Count; i++)
            if (lines[i].Count == 10)
            {
                lines[i].Sort((a, b) => a.transform.position.x.CompareTo(b.transform.position.x));
                SpawnerScript.isDestroy = true;
                StartCoroutine(DestroyBlocksDelayed(lines[i], 0.05f));
                LineIs[i] = true;
                linesCount += 1;
                linesInRow += 1;
            }
        score += 10 * linesInRow * linesInRow * (difficulty > 0 ? difficulty : 1);
        difficulty = linesCount / 10;
        if (linesInRow > 0)
            StartCoroutine(DelayedBlockDrop());
    }
    public void NewGame()
    {
        children = GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
            if (child.name == "block")
                Destroy(child.gameObject);
        score = 0;
        linesCount = 0;
        difficulty = 0;

    }
    IEnumerator DestroyBlocksDelayed(List<Transform> blocks, float delay)
    {
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(delay);
            Destroy(blocks[4 - i].gameObject);
            Destroy(blocks[5 + i].gameObject);
            SpawnerScript.isDestroy = false;
        }
    }
    private IEnumerator DelayedBlockDrop()
    {
        GameObject popup = Instantiate(scorePopUpPrefab, SpawnerScript.Figure.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.27f);
        for (int i = 0; i < lines.Count; i++)
        {
            if (LineIs[i] == true)
            {
                jcount += 1;
                int j = i;
                do
                {
                    foreach (Transform child in lines[j])
                        if (child != null)
                            child.transform.position += new Vector3(0, -jcount, 0);
                    j++;

                } while (LineIs[j] == false && j <= 19);
            }
        }
    }

}
