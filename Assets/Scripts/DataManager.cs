using UnityEngine;
using Newtonsoft.Json; 

public class DataManager : MonoBehaviour
{
   
    public static DataManager instance;

    private int _totalCountMonsers;
    private int _killCountMonster;
    private int _score;

    private const string SaveFileName = "SaveData.json";

    private void Start()
    {
        instance = this;
    }

    public void SaveData()
    {
        SaveDataModel dataModel = new SaveDataModel()
        {
            totalCountMonsers = _totalCountMonsers,
            killCountMonster = _killCountMonster,
            score = _score
        };

        string json = JsonConvert.SerializeObject(dataModel); 
        System.IO.File.WriteAllText(GetSavePath(), json);
    }

    public void LoadData()
    {
        string savePath = GetSavePath();
        if (System.IO.File.Exists(savePath))
        {
            string json = System.IO.File.ReadAllText(savePath);
            SaveDataModel dataModel = JsonConvert.DeserializeObject<SaveDataModel>(json); 

            _totalCountMonsers = dataModel.totalCountMonsers;
            _killCountMonster = dataModel.killCountMonster;
            _score = dataModel.score;
        }
    }

    private string GetSavePath()
    {
        return Application.persistentDataPath + "/" + SaveFileName;
    }
}

[System.Serializable]
public class SaveDataModel
{
    public int totalCountMonsers;
    public int killCountMonster;
    public int score;
}
