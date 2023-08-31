using UnityEngine;
using TMPro;

public class InGameUI : MonoBehaviour
{
    // Dependency to ManagerGame class.
    // Dependecy to ManagerLevel class.

    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private TextMeshProUGUI timescaleText;
    [SerializeField]
    private GameObject pausePanel;
    [SerializeField]
    private GameObject failedPanel;
    [SerializeField]
    private GameObject successPanel;

    private float tempTimescale;

    private void OnEnable()
    {
        ManagerLevel.Instance.OnLevelFailed += LevelFailedHandler;
        ManagerLevel.Instance.OnLevelSuccess += LevelSuccessHandler;
    }

    private void OnDisable()
    {
        ManagerLevel.Instance.OnLevelFailed -= LevelFailedHandler;
        ManagerLevel.Instance.OnLevelSuccess -= LevelSuccessHandler;
    }

    private void Start()
    {
        LevelSO currentSO = ManagerGame.Instance.GetCurrentLevelSO();
        int currentLevel = ManagerGame.Instance.currentLevel;
        levelText.text = "LEVEL " + currentLevel.ToString();
        string scaleText = currentSO.levelTimescale.ToString();
        if (scaleText.Contains(".")) scaleText = scaleText.Split(".")[0] + scaleText.Split(".")[1][0];
        timescaleText.text = "X" + scaleText;
        
        pausePanel.SetActive(false);
        failedPanel.SetActive(false);
        successPanel.SetActive(false);
    }

    private void LevelFailedHandler()
    {
        failedPanel.SetActive(true);
    }

    private void LevelSuccessHandler()
    {
        successPanel.SetActive(true);
    }

    public void NextButtonUI()
    {
        ManagerGame.Instance.LoadNextLevel();
    }

    public void RetryButtonUI()
    {
        ManagerGame.Instance.ReloadActiveScene();
    }

    public void PauseButtonUI()
    {
        tempTimescale = Time.timeScale;
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }

    public void ResumeButtonUI()
    {
        Time.timeScale = tempTimescale;
        pausePanel.SetActive(false);
    }

    public void MenuButtonUI()
    {
        ManagerGame.Instance.LoadMenuScene();
    }
}
