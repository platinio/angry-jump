using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadLevelManager : MonoBehaviour
{

    public List<GameObject> elementsToDeactivated;
    public Slider loadBar;
    public float loadSpeed;

    private float progress;

    public void LoadLevel(string name)
    {
        if (Time.timeScale != 1)
            Time.timeScale = 1;

        for (int n = 0; n < elementsToDeactivated.Count; n++)
            elementsToDeactivated[n].SetActive(false);

        loadBar.gameObject.SetActive(true);

        StartCoroutine(CO_ChangeLevel(name));
    }

    IEnumerator CO_ChangeLevel(string scene)
    {
        progress = 0;
        UpdateProgressBar();

        AsyncOperation asyn = SceneManager.LoadSceneAsync(scene);
        asyn.allowSceneActivation = false;

        do
        {
            if (progress < asyn.progress || asyn.progress >= 0.9f)
            {
                progress += Time.deltaTime * loadSpeed;
            }

            UpdateProgressBar();
            yield return new WaitForSeconds(Time.deltaTime);

        } while (progress < 1.0f);

        progress = 1.0f;
        UpdateProgressBar();
        asyn.allowSceneActivation = true;

    }

    private void UpdateProgressBar()
    {
        loadBar.value = progress;
    }
}
