using UnityEngine;
using UnityEngine.SceneManagement;

// Controller that holds all ui actions
public static class UIController
{
    // Dirty way to restart a scene
    public static void RestartGame()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log($"Restarting scene with index: {sceneIndex}");
        SceneManager.LoadScene(sceneIndex);
    }
}
