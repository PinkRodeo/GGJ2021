using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCoreLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartDown());
    }
    IEnumerator StartDown()
    {
        yield return null;
        float t = Time.time;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Game_Core");
        asyncLoad.allowSceneActivation = false;
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                Debug.Log(Time.time);
                if (Time.time - t > 10)
                    asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
     

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
