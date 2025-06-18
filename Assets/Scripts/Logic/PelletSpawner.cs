using System.Collections.Generic;
using UnityEngine;

public class PelletSpawner : MonoBehaviour
{
    public static PelletSpawner Instance;

    [Header("Spawn Settings")]
    [SerializeField] private GameObject _pelletPrefab;
    [SerializeField] private int _numberOfPellets = 10;
    [SerializeField] private float _arenaWidth = 20f;
    [SerializeField] private float _arenaDepth = 20f;

    [Header("Auto-Fix Settings")]
    [SerializeField] private string _pelletPrefabName = ""; // Store prefab name as backup

    // Public property
    public int NumberToSpawn => _numberOfPellets;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        // Store prefab name for auto-recovery
        if (_pelletPrefab != null && string.IsNullOrEmpty(_pelletPrefabName))
        {
            _pelletPrefabName = _pelletPrefab.name;
        }
    }

    public void SpawnPellets()
    {
        // IMMEDIATE FIX: If prefab is null, try to restore it
        if (_pelletPrefab == null)
        {
            Debug.LogWarning("⚠️ Pellet prefab is null - attempting auto-fix...");
            TryRestorePrefab();
        }

        if (_pelletPrefab == null)
        {
            Debug.LogError("❌ Cannot spawn pellets - no prefab available!");
            Debug.LogError("💡 MANUAL FIX: Drag your pellet prefab into the Pellet Prefab field");
            return;
        }

        Debug.Log($"🔄 Spawning {_numberOfPellets} pellets using: {_pelletPrefab.name}");

        // Clear existing pellets
        ClearExistingPellets();

        // Spawn pellets
        for (int i = 0; i < _numberOfPellets; i++)
        {
            Vector3 position = GetRandomPosition();
            GameObject pellet = Instantiate(_pelletPrefab, position, Quaternion.identity);
            pellet.name = $"Pellet_{i + 1}";
        }

        Debug.Log($"✅ All {_numberOfPellets} pellets spawned successfully!");
    }

    private void TryRestorePrefab()
    {
        Debug.Log("🔍 Attempting to restore pellet prefab...");

        // Method 1: Try to find by stored name
        if (!string.IsNullOrEmpty(_pelletPrefabName))
        {
            GameObject[] allObjects = Resources.LoadAll<GameObject>("");
            foreach (GameObject obj in allObjects)
            {
                if (obj.name == _pelletPrefabName && obj.GetComponent<Pellet>() != null)
                {
                    _pelletPrefab = obj;
                    Debug.Log($"✅ Restored prefab by name: {_pelletPrefabName}");
                    return;
                }
            }
        }

        // Method 2: Find any object with "Pellet" in name and Pellet component
        GameObject[] allPrefabs = Resources.LoadAll<GameObject>("");
        foreach (GameObject prefab in allPrefabs)
        {
            if (prefab.name.ToLower().Contains("pellet") && prefab.GetComponent<Pellet>() != null)
            {
                _pelletPrefab = prefab;
                _pelletPrefabName = prefab.name;
                Debug.Log($"✅ Auto-found pellet prefab: {prefab.name}");
                return;
            }
        }

        // Method 3: Look for prefabs in the project (Unity Editor only)
#if UNITY_EDITOR
        string[] guids = UnityEditor.AssetDatabase.FindAssets("t:GameObject");
        foreach (string guid in guids)
        {
            string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
            GameObject asset = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (asset != null && asset.GetComponent<Pellet>() != null)
            {
                _pelletPrefab = asset;
                _pelletPrefabName = asset.name;
                Debug.Log($"✅ Found pellet prefab in project: {asset.name}");
                return;
            }
        }
#endif

        Debug.LogError("❌ Could not auto-restore pellet prefab!");
    }

    private Vector3 GetRandomPosition()
    {
        float x = Random.Range(-_arenaWidth * 0.5f, _arenaWidth * 0.5f);
        float z = Random.Range(-_arenaDepth * 0.5f, _arenaDepth * 0.5f);
        return new Vector3(x, 0f, z);
    }

    private void ClearExistingPellets()
    {
        Pellet[] existingPellets = FindObjectsByType<Pellet>(FindObjectsSortMode.None);
        foreach (Pellet pellet in existingPellets)
        {
            if (pellet != null)
            {
                Destroy(pellet.gameObject);
            }
        }
    }

    // Manual fix method - call this if auto-fix fails
    [ContextMenu("Force Find Pellet Prefab")]
    public void ForceFindPelletPrefab()
    {
        _pelletPrefab = null;
        TryRestorePrefab();
    }
}