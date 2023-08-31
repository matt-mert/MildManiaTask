using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-2)]
public class ManagerGame : MonoBehaviour
{
    public static ManagerGame Instance { get; private set; }

    [SerializeField]
    private List<LevelSO> levelsList;

    private List<bool> levelsPassedOrNot;

    public int continueLevel { get; private set; }
    public int currentLevel { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        Application.targetFrameRate = 60;
        levelsPassedOrNot = new List<bool>();
    }

    private void Start()
    {
        Debug.Log("Start in ManagerGame was called.");
        bool continueLevelFound = false;
        continueLevel = 1;
        for (int i = 0; i < levelsList.Count; i++)
        {
            int isComplete = PlayerPrefs.GetInt("LEVEL_" + (i + 1).ToString(), 0);
            if (isComplete == 0) levelsPassedOrNot.Add(false);
            else levelsPassedOrNot.Add(true);

            if (isComplete == 0 && !continueLevelFound)
            {
                continueLevelFound = true;
                continueLevel = i + 1;
            }
        }
    }

    public LevelSO GetCurrentLevelSO()
    {
        return levelsList[currentLevel - 1];
    }

    public void SetCurrentLevelPassed()
    {
        levelsPassedOrNot[currentLevel] = true;
        PlayerPrefs.SetInt("LEVEL_" + currentLevel.ToString(), 1);
        continueLevel++;
    }

    public bool IsLevelPassed(int level)
    {
        return levelsPassedOrNot[level - 1];
    }

    public void ReloadActiveScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMenuScene()
    {
        currentLevel = 0;
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadContinueLevel()
    {
        currentLevel = continueLevel;
        SceneManager.LoadScene("Level" + continueLevel.ToString());
    }

    public void LoadNextLevel()
    {
        currentLevel++;
        SceneManager.LoadScene("Level" + currentLevel.ToString());
    }

    public void LoadLevelScene(int level)
    {
        currentLevel = level;
        SceneManager.LoadScene(level);
    }
}
