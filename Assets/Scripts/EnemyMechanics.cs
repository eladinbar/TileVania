using UnityEngine;

public class EnemyMechanics : MonoBehaviour {
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] int pointsPerKill = 50;
    [SerializeField] int hitPoints = 1;

    Rigidbody2D enemyRigidbody;

    public int PointsPerKill => pointsPerKill;

    public int HitPoints {
        get => hitPoints;
        set => hitPoints = value;
    }

    void Start() {
        enemyRigidbody = GetComponent<Rigidbody2D>();
    }
    
    void Update() {
        enemyRigidbody.velocity = new Vector2(moveSpeed, 0f);
    }

    void OnTriggerExit2D(Collider2D collider2d) {
        moveSpeed = -moveSpeed;
        
        FlipSprite(collider2d);
    }

    void FlipSprite(Collider2D other) {
        transform.localScale = new Vector2(Mathf.Sign(moveSpeed), transform.localScale.y);
    }
}
