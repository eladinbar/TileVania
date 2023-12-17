using System.Collections;
using UnityEngine;

public class CoinCounter : MonoBehaviour
{
    public static CoinCounter instance;
    
    private ScenePersist scenePersist;

    void Start() {
        instance = this;
        StartCoroutine(DelayedInitialization());
    }

    // Ensure all redundant 'Scene Persist' instances have finished the destruction process
    IEnumerator DelayedInitialization() {
        yield return new WaitForEndOfFrame();

        scenePersist = FindObjectOfType<ScenePersist>();
        scenePersist.InitializeCoinCollection(this.transform.childCount);
    }

    public void CollectCoin() {
        scenePersist.ProcessCoinCollection();
    }
}
