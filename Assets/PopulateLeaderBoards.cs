using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateLeaderBoards : MonoBehaviour
{
    public GameObject ContainerScore;
    public GameObject ContainerName;

    public void UpdateTable()
    {
        LeaderBoardManager leaderBoardManager = new LeaderBoardManager();
        var leaderBoard = leaderBoardManager.GetLeaderBoard();
        for (int i = 0; i < leaderBoard.Count; i++)
        {
            Transform scoreTransform = ContainerScore.transform.GetChild(i + 1);
            Transform nameTransform = ContainerName.transform.GetChild(i + 1);
            Text scoreText = scoreTransform.GetComponent<Text>();
            Text nameText = nameTransform.GetComponent<Text>();
            scoreText.text = leaderBoard[i].Score.ToString();
            nameText.text = leaderBoard[i].Name;
        }
    }
}
