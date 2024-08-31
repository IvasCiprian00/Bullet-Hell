using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private UIManager _uiManager;
    [SerializeField] private GameObject _player;
    [SerializeField] private float _score;
    [SerializeField] private int _nrOfAliveEnemies;
    [SerializeField] private int _firstWaveIndex;

    [SerializeField] private GameObject _mainMenuScreen;
    [SerializeField] private GameObject _selectWaveScreen;

    [SerializeField] private bool _gameIsPaused;
    [SerializeField] private bool _gameIsStarted;

    [SerializeField] private bool _controlsFlipped;
    [SerializeField] private bool _fixedJoystick;

    private static GameManager managerInstance;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (managerInstance == null)
        {
            managerInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Update()
    {
        if (!_gameIsStarted)
        {
            return;
        }

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

        _score += Time.deltaTime * 0.5f;
    }

    public void AddScore(int points)
    {
        _score += points;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
        _gameIsStarted = false;
    }


    public void ExitGame()
    {
        Application.Quit();
    }


    public int GetScore() { return (int) _score; }
    public void SetPlayer() { _player = GameObject.Find("Player"); }
    public bool IsGamePaused() {  return _gameIsPaused; }
    public void SetUIManager(UIManager uiManager) { _uiManager = uiManager;  }
    public void SetGameIsStarted(bool cond) {  _gameIsStarted = cond; _gameIsPaused = false; }
    public int GetFirstWaveIndex() { return _firstWaveIndex; }
    public bool GetControlsFlipped() { return _controlsFlipped; }
    public bool GetFixedJoystick() {  return _fixedJoystick; }
    public void FlipControls() {  _controlsFlipped = !_controlsFlipped; }
    public void FlipFixedJoystick() { _fixedJoystick = !_fixedJoystick; }
}
