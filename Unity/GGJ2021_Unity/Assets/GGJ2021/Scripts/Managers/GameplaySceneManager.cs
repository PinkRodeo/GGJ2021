using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class GameplaySceneManager : MonoBehaviour
{

    [SerializeField]
    public List<string> scenesToLoad = new List<string>();


    private void Start()
    {
        LoadLevels();
    }

    public void LoadLevels()
    {
#if UNITY_EDITOR

#else

        // SceneManager.LoadScene(scenesToLoad[0], LoadSceneMode.Single);

        foreach (var sceneName in scenesToLoad)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
#endif
        // SceneManager.LoadScene(levelDesignScene, LoadSceneMode.Additive);
        // SceneManager.LoadScene(environmentScene, LoadSceneMode.Additive);
    }

}
