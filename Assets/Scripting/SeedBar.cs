using UnityEngine;
using UnityEngine.UI;

public class SeedBar : MonoBehaviour
{
    public int currentSeeds = 0;
    public int maxSeeds = 15;

    public Slider slider;

    void Start()
    {
        slider.maxValue = maxSeeds;
        slider.value = currentSeeds;
    }

    public void AddSeed()
    {
        currentSeeds++;
        slider.value = currentSeeds;
    }
}