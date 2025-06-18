using UnityEngine;
using TMPro;

public class PelletCollector : MonoBehaviour
{
    public static PelletCollector Instance;

    [Header("UI Connection - Drag Your Existing Counter Here")]
    [SerializeField] private TextMeshProUGUI _counterText;

    [Header("Components")]
    private GameController _gameController;
    private AudioSource _audioSource;

    [Header("Game State")]
    private int _pelletsCollected = 0;
    private int _totalPelletsToCollect = 10;

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _gameController = GetComponent<GameController>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        // Get total from spawner
        PelletSpawner spawner = GetComponent<PelletSpawner>();
        if (spawner != null)
        {
            _totalPelletsToCollect = spawner.NumberToSpawn;
        }

        Debug.Log($"PelletCollector initialized - Need to collect {_totalPelletsToCollect} pellets to win");
    }

    public void ResetCounter()
    {
        _pelletsCollected = 0;

        // Reset counter text to normal color and text
        if (_counterText != null)
        {
            _counterText.text = "0";
            _counterText.color = Color.white; // Reset to white from green "WIN!"
        }

        Debug.Log("Counter reset to 0");
    }

    public void PelletCollected()
    {
        // Play sound
        if (_audioSource != null && _audioSource.clip != null)
        {
            _audioSource.PlayOneShot(_audioSource.clip);
        }

        // Increase count
        _pelletsCollected++;

        Debug.Log($"Pellet collected! Count: {_pelletsCollected}/{_totalPelletsToCollect}");

        // Update UI
        UpdateCounter();

        // Check win
        if (_pelletsCollected >= _totalPelletsToCollect)
        {
            PlayerWins();
        }
    }

    private void UpdateCounter()
    {
        if (_counterText != null)
        {
            _counterText.text = _pelletsCollected.ToString();
            // Keep normal color during gameplay
            _counterText.color = Color.white;
        }
        else
        {
            Debug.LogWarning("Counter Text not connected! Please drag your TextMeshPro counter into the Counter Text field.");
        }
    }

    private void PlayerWins()
    {
        Debug.Log("🎉 PLAYER WINS! All pellets collected!");

        // Update counter to show WIN
        if (_counterText != null)
        {
            _counterText.text = "WIN!";
            _counterText.color = Color.green;
        }

        // End game
        if (_gameController != null)
        {
            _gameController.EndGame();
        }
    }

    // Public method to manually set total (useful for dynamic spawning)
    public void SetTotalPellets(int total)
    {
        _totalPelletsToCollect = total;
        Debug.Log($"Total pellets to collect set to: {total}");
    }

    // Getters for other scripts
    public int GetCollectedCount() => _pelletsCollected;
    public int GetTotalPellets() => _totalPelletsToCollect;
}