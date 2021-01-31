using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Experimental.SceneManagement;

#endif

[ExecuteInEditMode]
public class EditorGameplaySceneManager : MonoBehaviour
{
#if UNITY_EDITOR

    public SceneAsset[] scenesToLoad;

    private void Awake()
    {
        if (PrefabStageUtility.GetCurrentPrefabStage() != null)
        {
            return;
        }

        if (Application.isPlaying)
        {
            return;
        }

        DelayedLoad();
    }

    private void DelayedLoad()
    {
        var manager = GetComponent<GameplaySceneManager>();
        manager.scenesToLoad.Clear();

        var currentScene = EditorSceneManager.GetActiveScene();

        foreach (var sceneAsset in scenesToLoad)
        {
            EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(sceneAsset), OpenSceneMode.Additive);
            manager.scenesToLoad.Add(sceneAsset.name);
        }

        EditorSceneManager.SetActiveScene(currentScene);
    }
#endif

}
