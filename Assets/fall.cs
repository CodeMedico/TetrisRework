using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class fall : MonoBehaviour
{
    [SerializeField] private GameObject buba;
    [SerializeField] private GameObject Spawner;

    public GameObject borderRight;
    public GameObject borderLeft;

    public float dropInterval = 0.8f;

    private GameObject shadow;
    private Lines Linesscript;
    private Spawner Spawnerscript;
    private float lastDropTime;
    private bool isFalling = true;
    private int a;


    void Start()
    {
        buba = GameObject.Find("buba");
        Linesscript = buba.GetComponent<Lines>();
        Spawner = GameObject.Find("Spawner");
        Spawnerscript = Spawner.GetComponent<Spawner>();
        a = Linesscript.difficulty;
        if (Linesscript.difficulty < 9)
        {
            dropInterval = (float)(0.8f - Linesscript.difficulty * 0.083);
            a = Linesscript.difficulty;
        }
        if (Linesscript.difficulty >= 9)
        {
            dropInterval = (float)(0.8f / (Linesscript.difficulty * 0.88) + 0.01f);
            a = Linesscript.difficulty;
        }
        ShadowCast();
        CreateBorder();
    }

    void Update()
    {

        if (Linesscript.difficulty < 9 && Linesscript.difficulty > a)
        {
            dropInterval = 0.8f - Linesscript.difficulty / 12;
            a = Linesscript.difficulty;
        }
        if (Linesscript.difficulty >= 9 && Linesscript.difficulty > a)
        {
            dropInterval = (float)(0.8f / (Linesscript.difficulty * 0.88) + 0.01f);
            a = Linesscript.difficulty;
        }
        if (Time.time - lastDropTime > dropInterval && IsMoveValid(new Vector3(0, -1, 0)))
            lastDropTime = Time.time;
        else if (Time.time - lastDropTime > dropInterval)
            FallEnd();
        if (!isFalling && Input.GetKey(KeyCode.DownArrow))
            FallEnd();
    }
    private bool IsMoveValid(Vector3 direction)
    {
        transform.position += direction;
        foreach (Transform block in transform)
        {
            if (Mathf.Round(block.position.y * 2) / 2 < -9.5f)
            {
                transform.position -= direction;
                return false;
            }
            foreach (Transform bubablock in buba.transform)
            {
                if (Mathf.Round(block.position.x * 2) / 2 == Mathf.Round(bubablock.position.x * 2) / 2 && Mathf.Round(block.position.y * 2) / 2 == Mathf.Round(bubablock.position.y * 2) / 2)
                {
                    transform.position -= direction;
                    return false;
                }
            }
        }
        ShadowCast();
        return true;
    }
    private void FallEnd()
    {
        Transform[] children = GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
            if (child.name == "block")
                child.transform.parent = GameObject.Find("buba").transform;
        Destroy(borderLeft);
        Destroy(borderRight);
        Destroy(gameObject);
        Destroy(shadow);
        Spawnerscript.SpawnNext();
    }

    public void ShadowCast()
    {
        if (shadow != null)
            Destroy(shadow);
        shadow = Instantiate(gameObject);
        shadow.GetComponent<fall>().enabled = false;
        shadow.GetComponent<control>().enabled = false;
        shadow.AddComponent<ShadowCaster>();
    }

    public void CreateBorder()
    {
        borderLeft = Instantiate(Spawnerscript.BorderPrefab, new Vector3(-5.1f, transform.position.y, 0), Quaternion.identity);
        borderRight = Instantiate(Spawnerscript.BorderPrefab, new Vector3(5.1f, transform.position.y, 0), Quaternion.identity);
    }

    public void NewGame()
    {
        if (shadow != null)
            Destroy(shadow);
        if (borderLeft != null)
            Destroy(borderLeft);
        if (borderRight != null)
            Destroy(borderRight);
    }
}