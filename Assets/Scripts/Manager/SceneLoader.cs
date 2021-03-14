using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    public bool LoadOnEnable;
    public int sceneIndex;

    public float loadDelay = 3f;

    public GameObject loadingIndicator;

    //TODO
    //public GameObject loadingUI;

    private void OnEnable()
    {
        if (LoadOnEnable)
        {
            LoadScene();
        }
    }

    //TODO: option to allow COMPLETE scene load aka destroy all objects tagged dontdestroyonload
    public void LoadScene()
    {
        if(loadDelay == 0)
        {
            if(loadingIndicator != null)
            {
                loadingIndicator.SetActive(true);
            }
            SceneManager.LoadSceneAsync(sceneIndex);
        }
        else
        {
            StartCoroutine(LoadSceneWithDelay(sceneIndex, loadDelay));
        }
    }
    public void LoadScene(int index)
    {
        if(loadDelay == 0)
        {
            if (loadingIndicator != null)
            {
                loadingIndicator.SetActive(true);
            }
            SceneManager.LoadSceneAsync(index);
        }
        else
        {
            StartCoroutine(LoadSceneWithDelay(index, loadDelay));
        }
    }
    IEnumerator LoadSceneWithDelay(int index, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (loadingIndicator != null)
        {
            loadingIndicator.SetActive(true);
        }
        SceneManager.LoadSceneAsync(index);

    }
}
