using TMPro;
using UnityEngine;

public class ScenePersist : MonoBehaviour {
    private int currentCount = 0;
    private int totalCount;

    [SerializeField] TextMeshProUGUI coinCounterText;

    void Awake() {
        int gameScenePersists = FindObjectsOfType<ScenePersist>().Length;
        if(gameScenePersists > 1)
            Destroy(this.gameObject);
        else
            DontDestroyOnLoad(this.gameObject);
    }

    public void ResetScenePersist() {
        Destroy(this.gameObject);
    }

    public void InitializeCoinCollection(int totalCount) {
        this.totalCount = totalCount;
        coinCounterText.text = currentCount.ToString() + "/" + totalCount.ToString();
    }

    public void ProcessCoinCollection() {
        currentCount++;
        coinCounterText.text = currentCount.ToString() + "/" + totalCount.ToString();
    }
}
