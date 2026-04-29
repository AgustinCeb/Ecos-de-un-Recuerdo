using UnityEngine;

[CreateAssetMenu(fileName = "StatsPerLevel", menuName = "ScriptableObject/StatsPerLevel")]
public class StatsPerLevel : ScriptableObject
{
    public LevelStats[] levels;
}
