using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class ManagerGame : MonoBehaviour
{
    public static ManagerGame Instance { get; private set; }

    [SerializeField]
    private List<LevelSO> levelsList;

    private List<int> levelsPassedOrNot;

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
    }

    private void Start()
    {
        bool continueLevelFound = false;
        for (int i = 0; i < levelsList.Count; i++)
        {
            int isComplete = PlayerPrefs.GetInt("LEVEL_" + (i + 1).ToString(), 0);
            levelsPassedOrNot.Add(isComplete);
            if (isComplete == 1 && !continueLevelFound)
            {
                continueLevelFound = true;
                continueLevel = i + 1;
            }
        }
    }

    public void ReloadActiveScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadContinueLevel()
    {
        SceneManager.LoadScene("Level" + continueLevel.ToString());
    }

    public void LoadCurrentLevelScene()
    {
        SceneManager.LoadScene(currentLevel);
    }

    public void LoadLevelScene(int level)
    {
        SceneManager.LoadScene(level);
    }
}
