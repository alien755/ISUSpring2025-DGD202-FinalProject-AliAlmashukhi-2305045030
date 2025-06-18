using UnityEngine;

public class Pellet : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Player collected pellet: {gameObject.name}");

            // Notify the collector
            if (PelletCollector.Instance != null)
            {
                PelletCollector.Instance.PelletCollected();
                Debug.Log("PelletCollected() called successfully");
            }
            else
            {
                Debug.LogError("PelletCollector.Instance is NULL!");
            }

            // DESTROY the pellet - NO RESPAWNING!
            Destroy(gameObject);
            Debug.Log("Pellet destroyed - no respawning");
        }
    }
}