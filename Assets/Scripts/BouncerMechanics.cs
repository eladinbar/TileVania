using UnityEngine;

public class BouncerMechanics : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Player"))
            GetComponent<AudioSource>().Play();
    }
}
