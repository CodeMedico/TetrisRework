using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using static GameData;

public class EventsControl : MonoBehaviour
{
    [SerializeField] private GameObject CanvasPauseMenu;
    [SerializeField] private GameObject Buba;
    [SerializeField] private GameObject Spawner;
    [SerializeField] private GameData gameData;
    [SerializeField] private GameObject OnTopOfMenu;
    [SerializeField] private BlockData blockData;
    [SerializeField] private InputField playerName;

    public bool lost = false;

    private Spawner SpawnerScript;
    private Lines BubaScript;
    private GameObject NewGameButton;
    private GameObject shadow;
    private GameObject NewScore;
    private ShadowCaster shadowCaster;
    private LeaderBoardManager leaderBoardManager = new LeaderBoardManager();
    private fall fallScript;

    void Start()
    {
        BubaScript = Buba.GetComponent<Lines>();
        SpawnerScript = Spawner.GetComponent<Spawner>();
        playerName.onEndEdit.AddListener(EndEdit);
        NewScore = CanvasPauseMenu.transform.Find("NewScore").gameObject;
        NewGameButton = CanvasPauseMenu.transform.Find("NewGameButton").gameObject;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !lost)

            if (Time.timeScale == 0)
                Resume();
            else
                Pause();
    }
    public void Resume()
    {
        CanvasPauseMenu.SetActive(false);
        OnTopOfMenu.SetActive(false);
        Time.timeScale = 1;
    }
    public void Pause()
    {
        OnTopOfMenu.SetActive(true);
        CanvasPauseMenu.SetActive(true);
        Time.timeScale = 0;
    }
    public void NewGame()
    {
        lost = false;
        SpawnerScript.NewGame();
        BubaScript.NewGame();
        if (SpawnerScript.Figure != null)
            fallScript = SpawnerScript.Figure.GetComponent<fall>();
        if (fallScript != null)
            fallScript.NewGame();
        Resume();
        GameObject child = CanvasPauseMenu.transform.Find("gameover").gameObject;
        GameObject ResumeButton = CanvasPauseMenu.transform.Find("ResumeButton").gameObject;
        ResumeButton.SetActive(true);
        child.SetActive(false);

    }
    public void Loose()
    {
        lost = true;
        GameObject child = CanvasPauseMenu.transform.Find("gameover").gameObject;
        GameObject ResumeButton = CanvasPauseMenu.transform.Find("ResumeButton").gameObject;

        ResumeButton.SetActive(false);
        child.SetActive(true);
        File.Delete(Path.Combine("save", "game_data.dat"));
        leaderBoardManager.LoadLeaderBoard();
        if (BubaScript.score > leaderBoardManager.GetLeaderBoard().LastOrDefault()?.Score || leaderBoardManager.GetLeaderBoard().Count < 10)
        {
            NewScore.SetActive(true);
            TextMeshProUGUI scoreText = NewScore.GetComponent<TextMeshProUGUI>();
            scoreText.text = "You Set New Highscore!   " + BubaScript.score.ToString();
            playerName.gameObject.SetActive(true);
            NewGameButton.SetActive(false);
        }
    }
    public void SaveGame()
    {
        gameData = new GameData();
        GameData.vector3S postition = new GameData.vector3S(SpawnerScript.Figure.transform.position);
        GameData.vector3S positionNext = new GameData.vector3S(SpawnerScript.NextFigure.transform.position);
        gameData.score = BubaScript.score;
        gameData.lines = BubaScript.linesCount;
        gameData.difficulty = BubaScript.difficulty;
        gameData.currentTetrominoName = SpawnerScript.Figure.transform.name;
        gameData.currentTetrominoPosition = postition;
        gameData.currentTetrominoRotation = (int)SpawnerScript.Figure.transform.rotation.z;
        gameData.nextTetrominoName = SpawnerScript.NextFigure.transform.name;
        gameData.nextTetrominoPosition = positionNext;
        gameData.nextTetrominoRotation = (int)SpawnerScript.NextFigure.transform.rotation.z;

        foreach (Transform block in Buba.transform)
        {
            blockData = new BlockData();
            if (block.name == "block")
            {
                BlockData.vector3S blockPosition = new BlockData.vector3S(block.position);
                blockData.blockPosition = blockPosition;
                BlockData.vector3S texturePosition = new BlockData.vector3S(block.GetChild(0).position);
                BlockData.vector3S textureScale = new BlockData.vector3S(block.GetChild(0).localScale);
                blockData.texturePosition = texturePosition;
                blockData.textureRotation = (int)block.GetChild(0).rotation.z;
                blockData.textureName = block.GetChild(0).name;
                blockData.textureScale = textureScale;
            }
            gameData.blockDataList.Add(blockData);
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream filestream = File.Create(Path.Combine("save", "game_data.dat"));
        formatter.Serialize(filestream, gameData);
        filestream.Close();

    }
    public void LoadGame()
    {
        if (SpawnerScript.Figure != null)
        {
            Destroy(SpawnerScript.Figure.GetComponent<fall>().borderLeft);
            Destroy(SpawnerScript.Figure.GetComponent<fall>().borderRight);
        }
        Destroy(SpawnerScript.Figure);
        Destroy(SpawnerScript.NextFigure);
        shadow = GameObject.Find("shadow");
        shadowCaster = shadow?.GetComponent<ShadowCaster>();
        if (shadowCaster != null)
            Destroy(shadowCaster.gameObject);
        BubaScript.NewGame();
        if (File.Exists(Path.Combine("save", "game_data.dat")))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = File.Open(Path.Combine("save", "game_data.dat"), FileMode.Open);
            GameData gameData = (GameData)formatter.Deserialize(fileStream);
            fileStream.Close();

            BubaScript.score = gameData.score;
            BubaScript.linesCount = gameData.lines;
            BubaScript.difficulty = gameData.difficulty;
            SpawnerScript.Figure = Instantiate(Resources.Load<GameObject>(gameData.currentTetrominoName));
            SpawnerScript.Figure.name = SpawnerScript.Figure.name.Replace("(Clone)", "");
            SpawnerScript.Figure.transform.position = new Vector3(gameData.currentTetrominoPosition.x, gameData.currentTetrominoPosition.y, gameData.currentTetrominoPosition.z);
            SpawnerScript.Figure.transform.Rotate(new Vector3(0, 0, gameData.currentTetrominoRotation), Space.Self);
            SpawnerScript.NextFigure = Instantiate(Resources.Load<GameObject>(gameData.nextTetrominoName));
            SpawnerScript.NextFigure.name = SpawnerScript.NextFigure.name.Replace("(Clone)", "");
            foreach (var script in SpawnerScript.NextFigure.GetComponents<MonoBehaviour>())
                script.enabled = false;
            SpawnerScript.NextFigure.transform.position = new Vector3(gameData.nextTetrominoPosition.x, gameData.nextTetrominoPosition.y, gameData.nextTetrominoPosition.z);
            SpawnerScript.NextFigure.transform.Rotate(new Vector3(0, 0, gameData.nextTetrominoRotation), Space.Self);

            foreach (var blockData in gameData.blockDataList)
            {
                GameObject blockObject = new GameObject();
                blockObject.transform.parent = BubaScript.transform;
                GameObject texture = new GameObject();
                blockObject.name = "block";
                blockObject.transform.position = new Vector3(blockData.blockPosition.x, blockData.blockPosition.y, blockData.blockPosition.z);
                texture.transform.parent = blockObject.transform;
                texture.transform.name = blockData.textureName;
                texture.transform.position = new Vector3(blockData.texturePosition.x, blockData.texturePosition.y, blockData.texturePosition.z);
                texture.transform.Rotate(new Vector3(0, 0, blockData.textureRotation), Space.Self);
                texture.transform.localScale = new Vector3(blockData.textureScale.x, blockData.textureScale.y, blockData.textureScale.z);
                SpriteRenderer spriteRenderer = texture.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = Resources.Load<Sprite>(Path.Combine("textures", blockData.textureName));
            }
        }
        Resume();
    }

    public void ExitGame()
    {
        if (lost == false)
            SaveGame();
        Application.Quit();
    }

    private void EndEdit(string text)
    {
        playerName.gameObject.SetActive(false);
        NewScore.gameObject.SetActive(false);
        NewGameButton.gameObject.SetActive(true);
        leaderBoardManager.AddHighScore(BubaScript.score, text);
        leaderBoardManager.SaveLeaderBoard();
    }
}
