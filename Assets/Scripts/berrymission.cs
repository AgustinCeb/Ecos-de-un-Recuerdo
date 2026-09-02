using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class berrymission : MonoBehaviour
{
    bool isMissionActive;
    [SerializeField] private GameObject missionUI;
    [SerializeField]TextMeshProUGUI progressText;
    [SerializeField]Slider progressSlider;
    int heldBerries;
    [SerializeField]int maxBerries;
    [SerializeField] GameObject completedMissionUI;
    public static Action onReturn;

    private void Awake()
    {
        isMissionActive = true;
        heldBerries = 0;
        missionUI.SetActive(false);
        completedMissionUI.SetActive(false);
        progressSlider.maxValue = maxBerries;
    }
    private void OnEnable()
    {
        MissionTrigger.onStartMission += StartBerryMission;
        MissionTrigger.onEndMission += EndMission;
        MissionCollectible.onMissionCollect += addBerry;
    }
    private void OnDisable()
    {
        MissionTrigger.onStartMission -= StartBerryMission;
        MissionCollectible.onMissionCollect -= addBerry;
    }
    void StartBerryMission()
    {
        Debug.Log("MissionStarted");
        missionUI.SetActive(true);
        UpdateProgress();
    }
    void UpdateProgress()
    {
        progressText.text = heldBerries + "/5";
        progressSlider.value = heldBerries;
    }
    void addBerry()
    {
        heldBerries++;
        UpdateProgress();
        if (heldBerries == maxBerries)
        {
            onReturn?.Invoke();
        }
    }
    void EndMission()
    {
        StartCoroutine (MissionComplete());
    }
    IEnumerator MissionComplete()
    {
        missionUI.SetActive(false);
        completedMissionUI.SetActive(true);
        yield return new WaitForSeconds(5);
        completedMissionUI.SetActive(false);
    }
}
