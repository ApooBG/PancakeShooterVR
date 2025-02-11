using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static SpawnerLevel;

public class PlayerStats : MonoBehaviour
{
    Stats stats;
    [SerializeField] TextMeshProUGUI waveNumber;
    [SerializeField] TextMeshProUGUI pancakesMadeUI;
    [SerializeField] TextMeshProUGUI killsMadeUI;
    [SerializeField] TextMeshProUGUI timeSurvivedUI;

    // Start is called before the first frame update
    void Start()
    {
        stats = SpawnerLevel.Instance.stats;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        killsMadeUI.text = stats.KilledZombies.ToString();
        pancakesMadeUI.text = stats.PancakesMade.ToString();
        TimeSpan timeSpan = TimeSpan.FromSeconds(stats.TimeSurvived);
        // Format the TimeSpan to a minute:second string format
        string timeText = timeSpan.ToString(@"mm\:ss");
        // Update the UI text
        timeSurvivedUI.text = timeText;
        waveNumber.text = ("WAVE " + (SpawnerLevel.Instance.currentLevel + 1f)).ToString();
    }
}
