using UnityEngine;

public class PauseMenu : MonoBehaviour {
    public static bool gameIsPaused;

    [SerializeField] GameObject pauseMenuUI;

    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape))
            if (gameIsPaused)
                Resume();
            else
                Pause();
    }

    public void Resume() {
        pauseMenuUI.SetActive(false);
        #if !UNITY_ANDROID
        Cursor.visible = false;
        #endif
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    void Pause() {
        pauseMenuUI.SetActive(true);
        #if !UNITY_ANDROID
        Cursor.visible = true;
        #endif
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void LoadMenu() {
        Time.timeScale = 1f;
        FindObjectOfType<GameSession>().ReturnToMainMenu();
    }

    public void QuitGame() {
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
