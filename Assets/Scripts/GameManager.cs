using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private UIManager _uiManager;
    [SerializeField] private GameObject _player;
    [SerializeField] private float _score;
    [SerializeField] private int _nrOfAliveEnemies;
    [SerializeField] private float _aliveEnemiesMultiplier;

    private bool _gameIsPaused;

    private void Awake()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    public void Start()
    {
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _gameIsPaused = !_gameIsPaused;

            Time.timeScale = _gameIsPaused ? 0.0f : 1.0f;
            _uiManager.DisplayPauseScreen(_gameIsPaused);

        }
        if(_player == null)
        {
            return;
        }

        if(Vector2.Distance(_player.transform.position, Vector2.zero) >= 25)
        {
            _player.GetComponent<PlayerScript>().TakeDamage();
        }

        GetScoreMultiplier();
        _score += Time.deltaTime * 0.5f;
    }

    public void GetScoreMultiplier()
    {
        _nrOfAliveEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        _aliveEnemiesMultiplier = _nrOfAliveEnemies * 1.3f;
    }

    public void AddScore(int points)
    {
        _score += points;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public int GetScore() { return (int) _score; }

    public bool IsGamePaused() {  return _gameIsPaused; }
}
