using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public void PlayGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        // save any game data here
        #if UNITY_EDITOR
            // 'UnityEditor.EditorApplication.isPlaying' needs to be set to false to end the game
            // Since 'Application.Quit()' does not work in the editor
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
