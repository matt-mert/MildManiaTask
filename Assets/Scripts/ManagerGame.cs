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

    [HideInInspector]
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
    }

    private void Start()
    {
        bool currentLevelFound = false;
        for (int i = 0; i < levelsList.Count; i++)
        {
            int isComplete = PlayerPrefs.GetInt("LEVEL_" + (i + 1).ToString(), 0);
            levelsPassedOrNot.Add(isComplete);
            if (isComplete == 1 && !currentLevelFound)
            {
                currentLevelFound = true;
                currentLevel = i + 1;
            }
        }
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
