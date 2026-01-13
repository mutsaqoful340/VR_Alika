using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject bulletPrefab;

    [Header("Gun Settings")]
    public Transform firingPoint;
    
    [Header("Fire Rate")]
    [Tooltip("Time between shots in seconds")]
    public float fireRate = 0.1f;
    private float lastFireTime;

    public void Shoot()
    {
        Debug.Log("Shoot() called");
        
        // Check fire rate cooldown
        if (Time.time - lastFireTime < fireRate)
        {
            Debug.Log("Fire rate cooldown active");
            return;
        }

        // Check if bullet prefab is assigned
        if (bulletPrefab == null)
        {
            Debug.LogError("Bullet prefab is not assigned!");
            return;
        }

        // Check if firing point is assigned
        if (firingPoint == null)
        {
            Debug.LogError("Firing point is not assigned!");
            return;
        }

        // Instantiate bullet
        GameObject bullet = Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);
        Debug.Log($"Bullet spawned at {firingPoint.position}");
        
        // Ignore collision with the gun and parent objects
        Collider bulletCollider = bullet.GetComponent<Collider>();
        if (bulletCollider != null)
        {
            // Ignore collision with all colliders on this gun
            Collider[] gunColliders = GetComponentsInParent<Collider>();
            foreach (Collider gunCollider in gunColliders)
            {
                Physics.IgnoreCollision(bulletCollider, gunCollider, true);
            }
            
            gunColliders = GetComponentsInChildren<Collider>();
            foreach (Collider gunCollider in gunColliders)
            {
                Physics.IgnoreCollision(bulletCollider, gunCollider, true);
            }
        }

        // Update fire time
        lastFireTime = Time.time;
    }
}
