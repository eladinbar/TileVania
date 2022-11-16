using System.Collections;
using UnityEngine;

public class CoinPickup : MonoBehaviour {
    [SerializeField] int pointsPerPickup = 100;
    [SerializeField] AudioClip coinPickupSFX;

    bool wasCollected = false;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && !wasCollected) {
            wasCollected = true;
            FindObjectOfType<GameSession>().IncreaseScore(pointsPerPickup);
            other.GetComponent<AudioSource>().PlayOneShot(coinPickupSFX);
            Destroy(this.gameObject);
        }
    }
}
