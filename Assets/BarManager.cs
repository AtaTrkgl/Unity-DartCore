using System.Collections.Generic;
using UnityEngine;
using UILib;

public class BarManager : MonoBehaviour
{
    public ProgressBar healthBar;
    public ProgressBar hungerBar;
    public TogglePlus togglePlus;
    public Graph graph;

    List<int> values;

    private void Start()
    {
        healthBar.max = 100;
        healthBar.min = 0;
        healthBar.current = 100;

        hungerBar.max = 100;
        hungerBar.min = 0;
        hungerBar.current = 100;

        values = new List<int>() {0,1,4,7,12,18,26,43,45,46,43,36,25,13,0,5,15,26,47,75};
    }

    private void Update()
    {
        healthBar.current -= Time.deltaTime * 10f;
        hungerBar.current -= Time.deltaTime * 10f;
    }

    public void Eat()
    {
        hungerBar.current += 40;
    }
    public void Heal()
    {
        healthBar.current += 40;
    }

    public void UpdateGraph()
    {
        if (togglePlus.isOn)
            graph.ShowGraph(values, GraphType.Line, true);
        else
            graph.ShowGraph(values, GraphType.Bar, true);
    }

}
