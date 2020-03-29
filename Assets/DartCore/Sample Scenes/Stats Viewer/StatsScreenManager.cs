using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DartCore.UI;
using DartCore.Utilities;
using TMPro;

public class StatsScreenManager : MonoBehaviour
{
    private enum Section
    { 
        Kills = 0,
        Wins = 1,
    }

    private int minSection = 0;
    private int maxSection = 1;
    [SerializeField] private Section currentSection = Section.Kills;

    [SerializeField] private Graph graph;
    [SerializeField] private ProgressBar unlockProgressBar;
    [SerializeField] private TextMeshProUGUI unlockText;
    [SerializeField] private TextMeshProUGUI rewardText;
    [SerializeField] private TextMeshProUGUI graphTitle;
    [SerializeField] private TextMeshProUGUI sectionTitle;

    [SerializeField] private ProgressBar nextScreenProgress;
    [SerializeField] private float screenChangeTime = 10f;
    private float timer = 0;

    private void Start()
    {
        UpdateScreen();
        nextScreenProgress.max = screenChangeTime;
        nextScreenProgress.min = 0;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= screenChangeTime)
            NextSection();
        nextScreenProgress.current = timer;
    }

    public void UpdateScreen()
    {
        List<int> list = new List<int>();
        switch (currentSection)
        {
            case Section.Kills:
                sectionTitle.text = "Kills";
                graphTitle.text = "Kills in the last 30 days";
                rewardText.text = "left to unlock\n<color=\"yellow\">the golden skin </color>";
                unlockProgressBar.fillerColor = Color.yellow;

                list = Mathd.RandomIntList(30, 0, 20);
                unlockText.text = Mathd.SumList(list).ToString() + "/1000 Kills";
                unlockProgressBar.min = 0;
                unlockProgressBar.max = 1000;
                break;
            case Section.Wins:
                sectionTitle.text = "Wins";
                graphTitle.text = "Wins in the last 30 days";
                rewardText.text = "left to unlock\n<color=\"green\">the emerald banner </color>";
                unlockProgressBar.fillerColor = Color.green;

                list = Mathd.RandomIntList(30, 0, 5);
                unlockText.text = Mathd.SumList(list).ToString() + "/150 Wins";
                unlockProgressBar.min = 0;
                unlockProgressBar.max = 150;
                break;
            default:
                break;
        }
        graph.ShowGraph(list, GraphType.Bar, false);
        unlockProgressBar.current = Mathd.SumList(list);
    }

    public void PreviousSection()
    {
        if (currentSection <= (Section)minSection)
            currentSection = (Section)maxSection;
        else
            currentSection = (Section)((int)currentSection - 1);

        timer = 0;
        UpdateScreen();
    }

    public void NextSection()
    {
        if (currentSection >= (Section)maxSection)
            currentSection = (Section)minSection;
        else
            currentSection = (Section)((int)currentSection + 1);

        timer = 0;
        UpdateScreen();
    }

}
