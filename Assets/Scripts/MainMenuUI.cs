using UnityEngine;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
    // Dependency to ManagerGame class.

    [SerializeField]
    private GameObject levelsPanel;
    [SerializeField]
    private GameObject levelsContent;

    private void Start()
    {
        foreach (Transform child in levelsContent.transform)
        {
            int levelNumber = int.Parse(child.name.Split("_")[1]);

            Transform levelInfo = child.GetChild(1);
            TextMeshProUGUI levelText = levelInfo.GetChild(0).GetComponent<TextMeshProUGUI>();
            if (ManagerGame.Instance.IsLevelPassed(levelNumber))
            {
                levelText.text = "PASSED";
                levelText.color = Color.green;
            }
            else if (ManagerGame.Instance.continueLevel == levelNumber)
            {
                levelText.text = "CURRENT";
                levelText.color = Color.cyan;
            }
            else
            {
                levelText.text = "LOCKED";
                levelText.color = Color.red;
            }
        }

        levelsPanel.SetActive(false);
    }

    public void ContinueButton()
    {
        ManagerGame.Instance.LoadContinueLevel();
    }

    public void LevelsButton()
    {
        levelsPanel.SetActive(true);
    }

    public void BackButton()
    {
        levelsPanel.SetActive(false);
    }

    public void LoadLevelButton(int level)
    {
        ManagerGame.Instance.LoadLevelScene(level);
    }
}
