using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UILib;

public class BarManager : MonoBehaviour
{
    public ProgressBar healthBar;
    public ProgressBar hungerBar;

    private void Start()
    {
        healthBar.max = 100;
        healthBar.min = 0;
        healthBar.current = 100;

        hungerBar.max = 100;
        hungerBar.min = 0;
        hungerBar.current = 100;
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

}
