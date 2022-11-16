using UnityEngine;

public class ProjectileMechanics : MonoBehaviour {
    [SerializeField] float projectileSpeed = 20f;
    [SerializeField] AudioClip enemyHitSFX;
    [SerializeField] AudioClip enemyDieSFX;

    Rigidbody2D projectileRigidbody;
    PlayerMechanics player;
    float xSpeed;
    GameSession gameSession;
    bool hasCollided = false;
    
    void Start() {
        gameSession = FindObjectOfType<GameSession>();
        projectileRigidbody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMechanics>();
        xSpeed = projectileSpeed * player.transform.localScale.x; 
        transform.localScale = new Vector2((Mathf.Sign(xSpeed)), transform.localScale.y);
    }
    
    void Update() {
        projectileRigidbody.velocity = new Vector2(xSpeed, 0f);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Enemy") && !hasCollided) {
            hasCollided = true;
            EnemyMechanics enemy = other.gameObject.GetComponent<EnemyMechanics>();
            enemy.HitPoints -= 1;
            if (enemy.HitPoints == 0) {
                player.gameObject.GetComponent<AudioSource>().PlayOneShot(enemyDieSFX);
                Destroy(other.gameObject);
                gameSession.IncreaseScore(other.GetComponent<EnemyMechanics>().PointsPerKill);
            }
            else {
                player.gameObject.GetComponent<AudioSource>().PlayOneShot(enemyHitSFX);
            }
        }

        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Destroy(this.gameObject);
    }
}
