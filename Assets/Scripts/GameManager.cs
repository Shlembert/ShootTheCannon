using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    public Action StopTime;
    public Action PlayTime;
    public static GameManager Instance;
    public Transform enemyTarget;
    public ProjectilePool projectilePool;
    public List<EnemyPool> enemyPools;

    [SerializeField] private Cannon cannon;
    [SerializeField] private Vector3 minPoint;
    [SerializeField] private Vector3 maxPoint;
    [SerializeField] private float spawnTimer;
    [SerializeField] private AnimationCurve complexityProgress, reloadCannonSpeed, delaySpawn;
    [SerializeField] private Transform statisticsPanel, gameOverPanel;
    [SerializeField] private int delayPortal;
    [SerializeField] private TMP_Text scoreTxt, killMonstersTxt, currentMonstersTxt, saveText;
    [SerializeField] private TMP_Text scoreGameOverTxt, killMonstersGameOverTxt;
    [SerializeField] private Transform enemyPool;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private HighScoreTable highScoreTable;
    [SerializeField] private TMP_InputField inputName;
    [SerializeField] private Button saveRecordButton;
    [SerializeField] private PortalPool portalPool;

    private int _totalCountMonsers;
    private int _currentCountMonsters;
    private int _killCountMonster;
    private int _score;

    private float _spawnTimer;
    private bool _isGame;

    private void Start()
    {
        Instance = this;
        _isGame = true;
        cannon.IsGame = _isGame;
        Cursor.visible = false;
        _spawnTimer = spawnTimer;
        musicSource.Play();
        ShowStatistics();
        saveRecordButton.onClick.AddListener(SaveRecord);
    }

    private void Update()
    {
        if (_isGame)
        {
            _spawnTimer -= Time.deltaTime;

            if (_spawnTimer <= 0)
            {
                _spawnTimer = spawnTimer;
                OpenPortal();
            }
        }
    }

    private async void OpenPortal()
    {
        int eIndex = UnityEngine.Random.Range(0, enemyPools.Count);

        float x = UnityEngine.Random.Range(minPoint.x, maxPoint.x);
        float y = UnityEngine.Random.Range(minPoint.y, maxPoint.y);
        float z = UnityEngine.Random.Range(minPoint.z, maxPoint.z);

        float t = _killCountMonster * 0.01f;

        
        Vector3 spawnPos = new Vector3(x, y, z);

        Portal currentPortal = portalPool.Create(spawnPos + Vector3.up * 1.2f);

        currentPortal.transform.DOScale(0f, 0.3f).From().OnComplete(() =>
        {
            if (_isGame)
            {
                enemyPools[eIndex].Create(spawnPos, enemyTarget, complexityProgress.Evaluate(t), complexityProgress.Evaluate(t));
                cannon.FireRate = reloadCannonSpeed.Evaluate(t);
                spawnTimer = delaySpawn.Evaluate(t);
            }
        });

        await UniTask.Delay(delayPortal * 100);

        _totalCountMonsers++;
        _currentCountMonsters++;
        SendDangerousValue();
        currentMonstersTxt.text = _currentCountMonsters.ToString();

        if (_currentCountMonsters >= 10) GameOver();

        currentPortal.transform.DOScale(0f, 0.2f).OnComplete(() =>
        {
            currentPortal.Release();
            currentPortal.SetOrigSize();
        });
    }

    public void MonsterDead(int score)
    {
        _currentCountMonsters--;
        SendDangerousValue();
        _killCountMonster++;
        _score += score;
        currentMonstersTxt.text = _currentCountMonsters.ToString();
        scoreTxt.text = _score.ToString();
        killMonstersTxt.text = _killCountMonster.ToString();
    }

    private void ShowStatistics()
    {
        statisticsPanel.gameObject.SetActive(true);
        statisticsPanel.DOMoveY(2000, 0.8f, false).From();
    }

    private void HideStatistics()
    {
        Vector3 origPos = statisticsPanel.position;

        statisticsPanel.DOMoveY(2000, 0.8f, false).OnComplete(() =>
        {
            ShowGameOverPanel();
            statisticsPanel.gameObject.SetActive(false);
            statisticsPanel.position = origPos;
        });
    }

    private void ShowGameOverPanel()
    {
        LeaderboardManager.FillHighScoreTable(highScoreTable);
        
        EnabledSaveInput(LeaderboardManager.CompareRecord(_score));

        gameOverPanel.gameObject.SetActive(true);
        scoreGameOverTxt.text = _score.ToString();
        killMonstersGameOverTxt.text = _killCountMonster.ToString();
        gameOverPanel.DOMoveY(-2000, 0.8f, false).From();
    }

    private void SendDangerousValue()
    {
        if (_currentCountMonsters <= 4) currentMonstersTxt.color = Color.white;
        else if (_currentCountMonsters <= 6) currentMonstersTxt.color = Color.gray;
        else currentMonstersTxt.color = Color.black;
    }

    private void GameOver()
    {
        musicSource.Stop();
        _isGame = false;
        cannon.IsGame = _isGame;
        Cursor.visible = true;
        HideStatistics();
        KillAllEnemy(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(minPoint, 0.2f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(maxPoint, 0.2f);
    }

    public async void SpawnPause(float value)
    {
        _spawnTimer += value;
        StopTime?.Invoke();
        await UniTask.Delay((int)value * 1000);
        PlayTime?.Invoke();
    }

    private void EnabledSaveInput(bool flag)
    {
        inputName.interactable = flag;
        saveRecordButton.interactable = flag;
    }

    public void PressButtonGameOver( int scene)
    {
        gameOverPanel.DOMoveY(-2000, 0.8f, false).OnComplete(() =>
        {
            SceneManager.LoadScene(scene);
        });
    }

    public void SaveRecord()
    {
        highScoreTable.ClearTable();

        string name = inputName.text.ToString();
        
        bool isNewHighScore = LeaderboardManager.AddRecord(name, _score);

        if (isNewHighScore) LeaderboardManager.FillHighScoreTable(highScoreTable);
       
        EnabledSaveInput(false);
    }

    public void KillAllEnemy(bool buster)
    {
        foreach (Transform item in enemyPool)
        {
            if (item.gameObject.activeSelf && !item.GetComponent<Enemy>().Dead)
            {
                if (buster) item.GetComponent<Enemy>().ApplayDamage(int.MaxValue);
                else item.GetComponent<Enemy>().Release();
            }
        }
    }
}
