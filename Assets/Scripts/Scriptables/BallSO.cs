using UnityEngine;

[CreateAssetMenu(fileName = "Sample Ball", menuName = "Scriptables/Ball Object")]
public class BallSO : ScriptableObject
{
    [Header("Make sure to fill these properties.")]

    [Tooltip("Prefab of the ball to be instantiated.")]
    public GameObject ballPrefab;

    [Tooltip("Color determines the winning/losing conditions.")]
    public BallColor ballColor;

    [Range(-5f, 5f)]
    [Tooltip("The position of the ball on X axis.")]
    public float ballPositionX;

    [Range(-8f, 8f)]
    [Tooltip("The position of the ball on Y axis.")]
    public float ballPositionY;

    [Tooltip("Should the ball be continuously moving?")]
    public bool isMoving;

    [Header("Fill the remaining sections if ball is moving.")]

    [Header("Note: Make sure to keep the speed curve \nnormalized.")]

    [Tooltip("The moving behaviour of the moving ball object.")]
    public AnimationCurve speedCurve;

    [Range(1f, 10f)]
    [Tooltip("The maximum speed the moving ball reaches during its movement.")]
    public float maxSpeed;

    [Header("Note: The time it takes is clearly \na function of displacement and speed.")]

    [Range(-5f, 5f)]
    [Tooltip("The position on X axis the ball reaches at the end of its movement.")]
    public float finalPositionX;

    [Range(-8f, 8f)]
    [Tooltip("The position on Y axis the ball reaches at the end of its movement.")]
    public float finalPositionY;

    /// <summary>
    /// Enum for keeping various ball colors.
    /// </summary>
    public enum BallColor
    {
        RED,
        BLUE,
        GREEN,
        PINK,
        WHITE,
        BLACK,
    }
}
