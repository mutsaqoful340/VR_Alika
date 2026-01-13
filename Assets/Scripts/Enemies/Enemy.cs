using UnityEngine;

public enum MovementAxis
{
    Forward,
    Backward,
    Left,
    Right,
    Up,
    Down
}

public class Enemy : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Speed of forward movement")]
    public float moveSpeed = 2f;
    
    [Header("Death")]
    [Tooltip("Destroy enemy immediately on death, or just disable")]
    public bool destroyOnDeath = true;
    public float destroyDelay = 0f;
    
    [Header("Optional Death Effect")]
    public GameObject deathEffectPrefab;
    public float deathEffectLifetime = 2f;

    [Header("Move Direction")]
    public MovementAxis movementAxis = MovementAxis.Forward;
    
    [Tooltip("Should enemy rotate to face movement direction?")]
    public bool faceMovementDirection = true;

    private Vector3 moveDirection;
    private bool isDead = false;
    private bool isStopped = false;

    private void Start()
    {
        // Convert enum to Vector3 direction
        moveDirection = GetDirectionVector(movementAxis);
        
        // Rotate enemy to face movement direction
        if (faceMovementDirection && moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
    }

    private void Update()
    {
        if (!isDead && !isStopped)
        {
            // Move forward continuously
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if hit by bullet - one hit kill
        Bullet bullet = collision.gameObject.GetComponent<Bullet>();
        if (bullet != null)
        {
            Die();
        }
    }

    public void StopMoving()
    {
        isStopped = true;
        Debug.Log("Enemy stopped moving");
    }

    public void ResumeMoving()
    {
        isStopped = false;
    }

    private Vector3 GetDirectionVector(MovementAxis axis)
    {
        switch (axis)
        {
            case MovementAxis.Forward:
                return Vector3.forward;
            case MovementAxis.Backward:
                return Vector3.back;
            case MovementAxis.Left:
                return Vector3.left;
            case MovementAxis.Right:
                return Vector3.right;
            case MovementAxis.Up:
                return Vector3.up;
            case MovementAxis.Down:
                return Vector3.down;
            default:
                return Vector3.forward;
        }
    }

    private void Die()
    {
        if (isDead) return;
        
        isDead = true;
        Debug.Log("Enemy died!");
        
        // Notify spawner
        EnemySpawner spawner = FindAnyObjectByType<EnemySpawner>();
        if (spawner != null)
        {
            spawner.OnEnemyKilled();
        }

        // Spawn death effect
        if (deathEffectPrefab != null)
        {
            GameObject effect = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, deathEffectLifetime);
        }

        // Destroy or disable enemy
        if (destroyOnDeath)
        {
            Destroy(gameObject, destroyDelay);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
