using UnityEngine;
using UnityEngine.UI;

public class SeedBar : MonoBehaviour
{
    public int currentSeeds = 0;
    public int maxSeeds = 6;

    public Slider slider;

    void Start()
    {
        slider.maxValue = maxSeeds;
        slider.value = currentSeeds;
    }

    void Update()
    {
        slider.value = Mathf.Lerp(slider.value, currentSeeds, Time.deltaTime * 5f);
    }

    public void CollectSeed()
    {
        if (currentSeeds < maxSeeds)
        {
            currentSeeds++;
            slider.value = currentSeeds;
        }
    }
}