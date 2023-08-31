using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ManagerLevel : MonoBehaviour
{
    public static ManagerLevel Instance { get; private set; }

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

    private void Start()
    {
        ballBehaviours = FindObjectsOfType<BallBehaviour>().ToList();
        foreach (BallBehaviour ballBehaviour in ballBehaviours)
        {
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
}
