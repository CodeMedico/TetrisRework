using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private GameObject bricks;
    [SerializeField] private GameObject EventsSystem;
    [SerializeField] private GameObject NextPlace;
    [SerializeField] private GameObject CanvasGame;

    public GameObject BorderPrefab;
    public GameObject Figure;
    public GameObject NextFigure;

    public bool isDestroy = false;

    private ScoreEtc scoreEtcscript;
    private Lines bubablock;
    private EventsControl ECscript;
    private Vector3 nextPlacePosition;

    void Start()
    {
        nextPlacePosition = new Vector3(NextPlace.transform.position.x, NextPlace.transform.position.y, 0);
        bricks = GameObject.Find("buba");
        bubablock = bricks.GetComponent<Lines>();
        ECscript = EventsSystem.GetComponent<EventsControl>();
        scoreEtcscript = CanvasGame.GetComponent<ScoreEtc>();
        if (NextFigure == null)
            SpawnNextAndDisable();
        if (Figure == null)
        {
            SpawnCurrentAndEnable();
            SpawnNextAndDisable();
        }
    }
    public IEnumerator SpawnNextFigure()
    {
        bubablock.CheckLines();
        yield return new WaitForSeconds(0.01f);
        if (isDestroy)
            yield return new WaitForSeconds(0.26f);
        if (Figure == null)
            SpawnCurrentAndEnable();
        if (!ECscript.lost)
            SpawnNextAndDisable();
        scoreEtcscript.UpdateData();
        ECscript.lost = false;
    }
    public void NewGame()
    {
        if (Figure != null)
            Destroy(Figure);
        Destroy(NextFigure);
        SpawnNextAndDisable();
        StartCoroutine(SpawnNextFigure());
    }
    public void SpawnNext()
    {
        StartCoroutine(SpawnNextFigure());
    }

    private void SpawnNextAndDisable()
    {
        int randomIndex = Random.Range(0, prefabs.Length);
        NextFigure = Instantiate(prefabs[randomIndex], nextPlacePosition, Quaternion.identity);
        NextFigure.name = NextFigure.name.Replace("(Clone)", "");
        NextFigure.SendMessage("PreRotate");
        foreach (MonoBehaviour script in NextFigure.GetComponents<MonoBehaviour>())
            script.enabled = false;
    }

    private void SpawnCurrentAndEnable()
    {
        Figure = NextFigure;
        Figure.transform.position = transform.position;
        foreach (MonoBehaviour script in Figure.GetComponents<MonoBehaviour>())
            script.enabled = true;
    }
}
