using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button NewGameButton;
    public Button ExitButton;
    public Button ResumeLastGame;
    public EventsControl eventsControlScript;
    public GameObject Spawner;
    public Spawner SpawnerScript;
    public GameObject StartScreen;
    public GameObject MainMenuObject;
    public GameObject OnTopOfMenu;

    void Start()
    {
        NewGameButton.onClick.AddListener(StartGame);
        ExitButton.onClick.AddListener(Application.Quit);
        ResumeLastGame.onClick.AddListener(LoadGame);
        SpawnerScript = Spawner.GetComponent<Spawner>();
        if (!File.Exists(Path.Combine("save", "game_data.dat")))
            ResumeLastGame.interactable = false;
    }
    void StartGame()
    {
        MainMenuObject.SetActive(false);
        Spawner.SetActive(true);
        OnTopOfMenu.SetActive(false);
    }
    void LoadGame()
    {
        StartGame();
        eventsControlScript.LoadGame();
    }
    public void DisableGameObject()
    {
        gameObject.SetActive(false);
    }
}
