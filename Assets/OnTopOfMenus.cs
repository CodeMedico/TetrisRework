using UnityEngine;
using UnityEngine.UI;

public class OnTopOfMenus : MonoBehaviour
{
    [SerializeField] private Button Close;
    [SerializeField] private Button LeaderBoard;
    [SerializeField] private GameObject Scroller;
    [SerializeField] private GameObject Container;

    private PopulateLeaderBoards containerScript;
    void Start()
    {
        LeaderBoard.onClick.AddListener(OpenLeaderBoard);
        Close.onClick.AddListener(CloseLeaderBoard);
        containerScript = Container.GetComponent<PopulateLeaderBoards>();
    }

    private void OpenLeaderBoard()
    {
        Scroller.SetActive(true);
        LeaderBoard.interactable = false;
        containerScript.UpdateTable();
    }

    private void CloseLeaderBoard()
    {
        Scroller.SetActive(false);
        LeaderBoard.interactable = true;
    }
}
