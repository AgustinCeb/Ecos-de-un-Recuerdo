using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMission", menuName = "ScriptableObject/Mission")]
public class MissionObj : ScriptableObject
{
    public string missionId;
    public string missionName;
    public string missionDescription;

    private void OnEnable()
    {
        if (string.IsNullOrEmpty(missionId))
        {
            missionId = Guid.NewGuid().ToString();    
        }
    }
}
