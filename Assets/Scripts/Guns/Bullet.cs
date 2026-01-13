using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Properties")]
    [Tooltip("Damage dealt on impact")]
    public float damage = 10f;
    
    [Tooltip("Time before bullet is destroyed (seconds)")]
    public float lifetime = 5f;
    
    [Header("Physics")]
    [Tooltip("Initial velocity of the bullet in m/s")]
    public float initialVelocity = 20f;
    
    [Tooltip("Use gravity on the bullet")]
    public bool useGravity = true;
    
    // [Header("Impact Effects")]
    // public GameObject impactEffectPrefab;
    // public float impactEffectLifetime = 2f;

    private void Start()
    {
        // Debug.Log($"Bullet Start - Forward: {transform.forward}, Rotation: {transform.rotation.eulerAngles}");
        
        // Apply velocity in the forward direction
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = transform.forward * initialVelocity;
            rb.useGravity = useGravity;
            // Debug.Log($"Velocity set to: {rb.linearVelocity}, Gravity: {rb.useGravity}");
        }
        else
        {
            Debug.LogError("Bullet is missing Rigidbody component!");
        }
        
        // Auto-destroy after lifetime
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Spawn impact effect at collision point
        // if (impactEffectPrefab != null)
        // {
        //     ContactPoint contact = collision.GetContact(0);
        //     GameObject impact = Instantiate(impactEffectPrefab, contact.point, Quaternion.LookRotation(contact.normal));
        //     Destroy(impact, impactEffectLifetime);
        // }

        // Apply damage if target has a health component
        // You can add your own damage system here
        // Example: collision.gameObject.GetComponent<Health>()?.TakeDamage(damage);

        // Destroy bullet on impact
        Destroy(gameObject);
    }
}
