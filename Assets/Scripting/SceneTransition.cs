using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;

    // Scene to load
    public string sceneToLoad;

    private bool isTransitioning = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTransitioning)
        {
            StartCoroutine(FadeAndLoadScene());
        }
    }

    IEnumerator FadeAndLoadScene()
    {
        isTransitioning = true;

        // Fade to black
        float t = 0;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;

            float alpha = t / fadeDuration;

            fadeImage.color = new Color(0, 0, 0, alpha);

            yield return null;
        }

        // Load chosen scene
        SceneManager.LoadScene(sceneToLoad);
    }

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float t = 0;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;

            float alpha = 1 - (t / fadeDuration);

            fadeImage.color = new Color(0, 0, 0, alpha);

            yield return null;
        }
    }
}