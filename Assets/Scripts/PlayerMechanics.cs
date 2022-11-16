using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMechanics : MonoBehaviour {
    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    private static readonly int IsClimbing = Animator.StringToHash("isClimbing");
    private static readonly int IsClimbingIdling = Animator.StringToHash("isClimbingIdling");
    private static readonly int IsBouncing = Animator.StringToHash("isBouncing");
    private static readonly int IsSwimming = Animator.StringToHash("isSwimming");
    private static readonly int FireBow = Animator.StringToHash("FireBow");
    private static readonly int Death = Animator.StringToHash("Death");
    
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] float swimSpeed = 5f;
    [SerializeField] float waterGravity = 1f;
    [SerializeField] Vector2 deathKick = new Vector2(0f, 10f);
    [SerializeField] GameObject projectile;
    [SerializeField] Transform bowTransform;
    [SerializeField] AudioClip jumpSFX;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] AudioClip bowShotSFX;
    AudioSource audioSource;
    
    Vector2 moveInput;
    Rigidbody2D playerRigidbody;
    Animator playerAnimator;
    BoxCollider2D feetCollider;
    CapsuleCollider2D bodyCollider;
    float originalGravity;
    bool isAlive = true;
    bool isAttachedToLadder = false;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        feetCollider = GetComponent<BoxCollider2D>();
        bodyCollider = GetComponent<CapsuleCollider2D>();
        originalGravity = playerRigidbody.gravityScale;
    }
    
    void Update() {
        if (isAlive) {
            Run();
            Swim();
            Climb();
            Bounce();
        }
    }

    void OnMove(InputValue value) {
        if(isAlive)
            moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value) {
        if (isAlive) {
            isAttachedToLadder = false;
            bool playerCanJump = feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground", "Climbing")) && 
                                 !feetCollider.IsTouchingLayers(LayerMask.GetMask("Water"));

            if (value.isPressed && playerCanJump) {
                audioSource.PlayOneShot(jumpSFX);
                playerRigidbody.velocity += new Vector2(0f, jumpSpeed);
            }
        }
    }

    void OnFire(InputValue value) {
        if (isAlive) {
            audioSource.PlayOneShot(bowShotSFX);
            Instantiate(projectile, bowTransform.position, transform.rotation);
            playerAnimator.SetTrigger(FireBow);
        }
    }
    
    void Run() {
        Vector2 runVelocity = new Vector2(moveInput.x * runSpeed, playerRigidbody.velocity.y);
        playerRigidbody.velocity = runVelocity;
        
        bool playerIsRunning = Mathf.Abs(runVelocity.x) > Mathf.Epsilon;
        playerAnimator.SetBool(IsRunning, playerIsRunning);
        
        FlipSprite(playerIsRunning, runVelocity.x);
    }

    void Climb() {
        if (feetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")) && Input.GetAxis("Vertical") != 0) {
            isAttachedToLadder = true;
            
            Vector2 climbVelocity = new Vector2(playerRigidbody.velocity.x / 2f, moveInput.y * climbSpeed);
            playerRigidbody.velocity = climbVelocity;
            
            AdjustGravity(true, false);
            AdjustClimbingState(true, Mathf.Abs(climbVelocity.y) > Mathf.Epsilon);
        } else if (isAttachedToLadder && feetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))) {
            AdjustGravity(true, false);
            AdjustClimbingState(true, false);
        } else {
            AdjustGravity(false, false);
            AdjustClimbingState(false, false);
        }
    }

    private void AdjustClimbingState(bool playerIsOnLadder, bool playerIsClimbing) {
        playerAnimator.SetBool(IsClimbingIdling, playerIsOnLadder & !playerIsClimbing);
        playerAnimator.SetBool(IsClimbing, playerIsClimbing);
    }

    void Bounce() {
        playerAnimator.SetBool(IsBouncing, bodyCollider.IsTouchingLayers(LayerMask.GetMask("Bouncing")));
    }

    void Swim() {
        if (feetCollider.IsTouchingLayers(LayerMask.GetMask("Water"))) {
            Vector2 climbVelocity = new Vector2(playerRigidbody.velocity.x / 2f, moveInput.y * swimSpeed);
            playerRigidbody.velocity = climbVelocity;
            
            AdjustGravity(false, true);
            playerAnimator.SetBool(IsSwimming, true);
        } else {
            AdjustGravity(false, false);
            playerAnimator.SetBool(IsSwimming, false);
        }
    }

    void AdjustGravity(bool playerIsClimbing, bool playerIsSwimming) {
        if(playerIsClimbing)
            playerRigidbody.gravityScale = 0f;
        else if(playerIsSwimming)
            playerRigidbody.gravityScale = waterGravity;
        else
            playerRigidbody.gravityScale = originalGravity;
    }

    void FlipSprite(bool playerIsRunning, float moveSpeed) {
        if (playerIsRunning)
            transform.localScale = new Vector2(Mathf.Sign(moveSpeed), transform.localScale.y);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if ((other.CompareTag("Enemy") || other.CompareTag("Hazard")) && isAlive)
            Die();
    }

    void Die() {
        isAlive = false;
        audioSource.PlayOneShot(deathSFX);
        playerAnimator.SetTrigger(Death);
        playerRigidbody.velocity = deathKick;
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }
}
