using UnityEngine;
using TMPro;
using System.Collections;

public class DefeatGate : MonoBehaviour
{
    [Header("Gate Settings")]
    public GameObject defeatPanel;
    
    [Tooltip("Delay before activating defeat (seconds)")]
    public float defeatDelay = 0.5f;

    private bool hasTriggered = false;

    void Start()
    {
        defeatPanel.SetActive(false);
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger entered by: {other.gameObject.name}, Tag: {other.tag}");
        
        if (other.CompareTag("Enemy") && !hasTriggered)
        {
            hasTriggered = true;
            StartCoroutine(TriggerDefeatSequence());
        }
    }

    private IEnumerator TriggerDefeatSequence()
    {
        Debug.Log($"Waiting {defeatDelay} seconds before defeat...");
        yield return new WaitForSeconds(defeatDelay);

        // Stop ALL enemies in the scene
        Enemy[] allEnemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        Debug.Log($"Found {allEnemies.Length} enemies to stop");
        
        foreach (Enemy enemy in allEnemies)
        {
            if (enemy != null)
            {
                enemy.StopMoving();
            }
        }

        // Stop the spawner
        EnemySpawner spawner = FindAnyObjectByType<EnemySpawner>();
        if (spawner != null)
        {
            spawner.StopSpawning();
            Debug.Log("Enemy entering DefeatGate - Spawner stopped");
        }

        // Show defeat panel
        if (defeatPanel != null)
        {
            // Activate all parents first
            Transform current = defeatPanel.transform;
            while (current != null)
            {
                current.gameObject.SetActive(true);
                Debug.Log($"Activating: {current.name}");
                current = current.parent;
            }
            
            // Activate all children
            foreach (Transform child in defeatPanel.GetComponentsInChildren<Transform>(true))
            {
                child.gameObject.SetActive(true);
            }
            
            defeatPanel.SetActive(true);
            
            Debug.Log($"Defeat panel activated - Active: {defeatPanel.activeSelf}, ActiveInHierarchy: {defeatPanel.activeInHierarchy}");
            
            // List all Canvas components in scene
            Canvas[] allCanvases = FindObjectsByType<Canvas>(FindObjectsSortMode.None);
            Debug.Log($"Total canvases in scene: {allCanvases.Length}");
            foreach (Canvas c in allCanvases)
            {
                Debug.Log($"Canvas: {c.name}, Active: {c.gameObject.activeInHierarchy}, Enabled: {c.enabled}");
            }
        }
    }
}
