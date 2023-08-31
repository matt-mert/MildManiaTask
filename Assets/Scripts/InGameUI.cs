using UnityEngine;
using TMPro;

public class InGameUI : MonoBehaviour
{
    private ManagerGame managerGame;

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

    private void Awake()
    {
        managerGame = ManagerGame.Instance;
    }

    private void OnEnable()
    {
        BallBehaviour.OnHitUnmatched += OnHitUnmatchedHandler;
    }

    private void OnDisable()
    {
        BallBehaviour.OnHitUnmatched -= OnHitUnmatchedHandler;
    }

    private void Start()
    {
        levelText.text = "LEVEL 1";
        timescaleText.text = "X1";
        pausePanel.SetActive(false);
        failedPanel.SetActive(false);
        successPanel.SetActive(false);
    }

    private void OnHitUnmatchedHandler()
    {
        failedPanel.SetActive(true);
    }

    public void RetryButtonUI()
    {
        managerGame.ReloadActiveScene();
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
        managerGame.LoadMenuScene();
    }
}
