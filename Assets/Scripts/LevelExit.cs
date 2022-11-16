using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour {
    [SerializeField] float levelLoadDelay = 1.5f;
    
    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player"))
            StartCoroutine(LoadNextLevel());
    }

    private IEnumerator LoadNextLevel() {
        GetComponent<AudioSource>().Play();
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
