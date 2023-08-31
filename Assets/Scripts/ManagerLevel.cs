using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class ManagerLevel : MonoBehaviour
{
    // Dependency to ManagerGame class.

    public static ManagerLevel Instance { get; private set; }

    public delegate void OnLevelFailedDelegate();
    public event OnLevelFailedDelegate OnLevelFailed;
    public delegate void OnLevelSuccessDelegate();
    public event OnLevelSuccessDelegate OnLevelSuccess;
    public delegate void OnColorFinishedDelegate(BallBehaviour.BallColor color);
    public event OnColorFinishedDelegate OnColorFinished;

    private List<BallBehaviour> ballBehaviours;
    private Dictionary<BallBehaviour.BallColor, int> ballColorAmounts;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        ballColorAmounts = new Dictionary<BallBehaviour.BallColor, int>();
    }

    private void OnEnable()
    {
        BallBehaviour.OnHitUnmatched += HitUnmatchedHandler;
        BallBehaviour.OnHitMatched += HitMatchedHandler;
    }

    private void OnDisable()
    {
        BallBehaviour.OnHitUnmatched -= HitUnmatchedHandler;
        BallBehaviour.OnHitMatched -= HitMatchedHandler;
    }

    private void Start()
    {
        LevelSO currentSO = ManagerGame.Instance.GetCurrentLevelSO();

        ballBehaviours = FindObjectsOfType<BallBehaviour>().ToList();
        foreach (BallBehaviour ballBehaviour in ballBehaviours)
        {
            ballBehaviour.MultiplyAttractedSpeed(currentSO.levelTimescale);

            BallBehaviour.BallColor ballColor = ballBehaviour.GetBallColor();
            if (ballColorAmounts.ContainsKey(ballColor))
            {
                ballColorAmounts[ballColor]++;
            }
            else
            {
                ballColorAmounts.Add(ballColor, 1);
            }
        }

        // foreach (KeyValuePair<BallBehaviour.BallColor, int> keyValuePair in ballColorAmounts)
        // {
        //     Debug.Log(keyValuePair);
        // }
    }

    private void HitUnmatchedHandler()
    {
        OnLevelFailed?.Invoke();
    }

    private void HitMatchedHandler(BallBehaviour.BallColor color)
    {
        ballColorAmounts[color]--;
        if (ballColorAmounts[color] == 1)
        {
            OnColorFinished?.Invoke(color);
            ballColorAmounts[color] = 0;
        }

        bool allColorsFinished = true;
        foreach (int value in ballColorAmounts.Values)
        {
            if (value != 0)
            {
                allColorsFinished = false;
                break;
            }
        }

        if (allColorsFinished)
        {
            ManagerGame.Instance.SetCurrentLevelPassed();
            OnLevelSuccess?.Invoke();
        }
    }
}
