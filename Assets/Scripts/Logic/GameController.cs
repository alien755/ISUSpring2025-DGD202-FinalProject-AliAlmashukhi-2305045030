using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Components")]
    private PelletSpawner _pelletSpawner;
    private PelletCollector _pelletCollector;

    [Header("UI")]
    [SerializeField] private GameObject _gameOverScreen;

    private void Awake()
    {
        _pelletSpawner = GetComponent<PelletSpawner>();
        _pelletCollector = GetComponent<PelletCollector>();

        if (_pelletSpawner == null)
            Debug.LogError("❌ PelletSpawner component missing!");
        if (_pelletCollector == null)
            Debug.LogError("❌ PelletCollector component missing!");
    }

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        Debug.Log("🎮 StartGame() called");

        // Hide game over screen
        if (_gameOverScreen != null)
        {
            _gameOverScreen.SetActive(false);
        }

        // Reset counter
        if (_pelletCollector != null)
        {
            _pelletCollector.ResetCounter();
            Debug.Log("✅ Counter reset");
        }

        // Spawn pellets - let the spawner handle prefab checking internally
        if (_pelletSpawner != null)
        {
            Debug.Log("🔄 Calling SpawnPellets()...");
            _pelletSpawner.SpawnPellets();
        }
        else
        {
            Debug.LogError("❌ PelletSpawner is null!");
        }

        Debug.Log("✅ StartGame() completed");
    }

    public void EndGame()
    {
        Debug.Log("🏁 Game ended");
        if (_gameOverScreen != null)
        {
            _gameOverScreen.SetActive(true);
        }
    }

    public void RestartGame()
    {
        Debug.Log("🔄 Restart Game button pressed");
        StartGame();
    }

    // Alternative method names in case button uses different name
    public void PlayAgain()
    {
        Debug.Log("🔄 Play Again button pressed");
        StartGame();
    }
}