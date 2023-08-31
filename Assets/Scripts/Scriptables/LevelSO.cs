using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sample Level", menuName = "Scriptables/Level Object")]
public class LevelSO : ScriptableObject
{
    [Header("Be careful not to coincide two ball objects.")]

    [Tooltip("Number of the corresponding level.")]
    public int levelNumber;

    [Range(0.1f, 4f)]
    [Tooltip("The multiplier of the flow of time.")]
    public float levelTimescale;

    [Tooltip("Whether the user is racing against time.")]
    public bool isTimeLimited;

    [Header("Fill this section if the level is time-limited.")]

    [Tooltip("Duration of the round.")]
    public int duration;

    [Tooltip("Bonus is added upon a successful match.")]
    public int bonusTime;
}
