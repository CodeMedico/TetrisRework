using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

public class LeaderBoardData
{
    public int Score;
    public string Name;

    public LeaderBoardData() { }

    public LeaderBoardData(int score, string name)
    {
        Score = score;
        Name = name;
    }
}

public class LeaderBoardManager
{
    private List<LeaderBoardData> data = new List<LeaderBoardData>();
    private string HighScoreFilePath = Path.Combine("save", "highscores.xml");

    public LeaderBoardManager()
    {
        LoadLeaderBoard();
    }

    public List<LeaderBoardData> GetLeaderBoard()
    {
        return data;
    }
    public void LoadLeaderBoard()
    {
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<LeaderBoardData>));
            using (FileStream stream = new FileStream(HighScoreFilePath, FileMode.OpenOrCreate))
            {
                data = (List<LeaderBoardData>)serializer.Deserialize(stream);
            }
        }
        catch
        {
            data = new List<LeaderBoardData>();
        }
    }

    public void SaveLeaderBoard()
    {
        if (File.Exists(HighScoreFilePath))
        {
            File.Delete(HighScoreFilePath);
        }
        XmlSerializer serializer = new XmlSerializer(typeof(List<LeaderBoardData>));
        using (FileStream stream = new FileStream(HighScoreFilePath, FileMode.Create))
        {
            serializer.Serialize(stream, data);
        }
    }

    public void AddHighScore(int score, string name)
    {
        data.Add(new LeaderBoardData() { Name = name, Score = score });
        data = data.OrderByDescending(score => score.Score).ToList();
        if (data.Count > 10)
            data.RemoveAt(data.Count - 1);
    }
}

