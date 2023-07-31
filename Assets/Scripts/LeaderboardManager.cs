using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    private const int MaxRecords = 10;
    private static LeaderboardManager instance;
    private static List<LeaderboardEntry> leaderboardEntries = new List<LeaderboardEntry>();

    public static LeaderboardManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LeaderboardManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "LeaderboardManager";
                    instance = obj.AddComponent<LeaderboardManager>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadLeaderboard();
    }

    public static void SaveLeaderboard()
    {
        string json = JsonUtility.ToJson(new LeaderboardData(leaderboardEntries));
        System.IO.File.WriteAllText(GetSavePath(), json);
    }

    public static void LoadLeaderboard()
    {
        string savePath = GetSavePath();
        if (System.IO.File.Exists(savePath))
        {
            string json = System.IO.File.ReadAllText(savePath);
            LeaderboardData data = JsonUtility.FromJson<LeaderboardData>(json);

            leaderboardEntries.Clear();
            leaderboardEntries.AddRange(data.entries);
        }
    }

    public static bool CompareRecord(int score)
    {
        bool isHighScore = false;

        if (leaderboardEntries.Count < MaxRecords || score > leaderboardEntries.Last().score)
        {
            isHighScore = true;
            if (leaderboardEntries.Count >= MaxRecords)
            {
                int indexToReplace = leaderboardEntries.FindIndex(entry => entry.score < score);
                if (indexToReplace != -1)
                {
                    leaderboardEntries[indexToReplace].score = score;
                }
            }
        }

        leaderboardEntries = leaderboardEntries.OrderByDescending(entry => entry.score).ToList();

        return isHighScore;
    }

    public static List<LeaderboardEntry> GetLeaderboard()
    {
        return leaderboardEntries;
    }

    private static string GetSavePath()
    {
        return Application.persistentDataPath + "/leaderboard.json";
    }

    public static void FillHighScoreTable(HighScoreTable highScoreTable)
    {
        foreach (LeaderboardEntry entry in leaderboardEntries)
        {
            highScoreTable.AddHighScore(entry.playerName, entry.score);
        }
    }

    public static bool AddRecord(string playerName, int score)
    {
        LeaderboardEntry newEntry = new LeaderboardEntry(score, playerName);
        leaderboardEntries.Add(newEntry);
        leaderboardEntries = leaderboardEntries.OrderByDescending(entry => entry.score).ToList();

        if (leaderboardEntries.Count > MaxRecords)
        {
            leaderboardEntries.RemoveAt(MaxRecords);
        }

        SaveLeaderboard();

        return leaderboardEntries.Contains(newEntry);
    }
}

[System.Serializable]
public class LeaderboardData
{
    public List<LeaderboardEntry> entries;

    public LeaderboardData(List<LeaderboardEntry> entries)
    {
        this.entries = entries;
    }
}

[System.Serializable]
public class LeaderboardEntry
{
    public int score;
    public string playerName;

    public LeaderboardEntry(int score, string playerName)
    {
        this.score = score;
        this.playerName = playerName;
    }
}
