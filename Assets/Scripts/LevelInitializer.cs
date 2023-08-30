using System.Collections.Generic;
using UnityEngine;

public class LevelInitializer : MonoBehaviour
{
    [SerializeField]
    private LevelSO levelObject;

    // TODO: Generic Type buffers to hold color amounts etc.

    private void Start()
    {
        Time.timeScale *= levelObject.levelTimescale;
        List<BallSO> ballObjects = levelObject.ballObjectsInLevel;
        foreach (BallSO ballObject in ballObjects)
        {
            Vector3 position = new Vector3(ballObject.ballPositionX, 0f, ballObject.ballPositionY);
            GameObject current = Instantiate(ballObject.ballPrefab, position, Quaternion.identity);
            BallBehaviour currentBehaviour = current.GetComponent<BallBehaviour>();
            currentBehaviour.SetBallProperties(ballObject);
        }
    }

    public void InitializeGame(LevelSO levelSO)
    {
        Time.timeScale *= levelSO.levelTimescale;
        List<BallSO> ballObjects = levelSO.ballObjectsInLevel;
        foreach (BallSO ballObject in ballObjects)
        {
            Vector3 position = new Vector3(ballObject.ballPositionX, 0f, ballObject.ballPositionY);
            GameObject current = Instantiate(ballObject.ballPrefab, position, Quaternion.identity);
            BallBehaviour currentBehaviour = current.GetComponent<BallBehaviour>();
            currentBehaviour.SetBallProperties(ballObject);
        }
    }
}
