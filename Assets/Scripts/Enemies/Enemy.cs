using UnityEngine;

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

    private bool isDead = false;
    private bool isStopped = false;

    private void Update()
    {
        if (!isDead && !isStopped)
        {
            // Move forward continuously
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
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
