using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button ResumeButton;
    [SerializeField] private Button ExitButton;
    [SerializeField] private Button NewGameButton;
    [SerializeField] private Button SaveGameButton;
    [SerializeField] private GameObject EventSystem;

    private EventsControl EventSystemScript;

    void Start()
    {
        EventSystemScript = EventSystem.GetComponent<EventsControl>();
        ResumeButton.onClick.AddListener(EventSystemScript.Resume);
        ExitButton.onClick.AddListener(EventSystemScript.ExitGame);
        NewGameButton.onClick.AddListener(EventSystemScript.NewGame);  
    }
}
