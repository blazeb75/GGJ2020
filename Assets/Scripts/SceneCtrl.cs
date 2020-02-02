using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCtrl : MonoBehaviour
{
 
    public void ReloadGame()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void QuitGame()
    {
        if (!Application.isEditor) System.Diagnostics.Process.GetCurrentProcess().Kill();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

#endif
    }
}
