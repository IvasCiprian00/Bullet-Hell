using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Controls")]
    [SerializeField] private GameObject[] _controls;
    [SerializeField] private Joystick _fixedJoystick;
    [SerializeField] private Joystick _floatingJoystick;
    [SerializeField] private GameObject _shootZone;

    [SerializeField] private GameManager _gameManager;
    [SerializeField] private TextMeshProUGUI _score;
    [SerializeField] private GameObject _dodgeRefreshed;
    [SerializeField] private GameObject _restartButton;
    [SerializeField] private GameObject _quitButton;
    [SerializeField] private GameObject _pauseScreen;
    private GameObject _player;

    private void Awake()
    {
        _player = GameObject.Find("Player");
    }

    public void Start()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _gameManager.SetUIManager(this);

        CheckSettings();
    }

    private void Update()
    {
        _score.text = "Score: " + _gameManager.GetScore();
    }

    public void CheckSettings()
    {
        if (_gameManager.GetControlsFlipped())
        {
            for(int i = 0; i < _controls.Length; i++)
            {
                _controls[i].transform.position = new Vector3(-_controls[i].transform.position.x, _controls[i].transform.position.y, _controls[i].transform.position.z);
            }
            //_fixedJoystick.gameObject.transform.position = new Vector3(-Mathf.Abs(_fixedJoystick.gameObject.transform.position.x), _fixedJoystick.gameObject.transform.position.y, 0);
            //_floatingJoystick.gameObject.transform.position = new Vector3(-Mathf.Abs(_floatingJoystick.gameObject.transform.position.x), _floatingJoystick.gameObject.transform.position.y, 0);
        }

        if (_gameManager.GetFixedJoystick())
        {
            _player.GetComponent<PlayerScript>().SetJoystick(_fixedJoystick);
            _fixedJoystick.gameObject.SetActive(true);
            _floatingJoystick.gameObject.SetActive(false);
        }
        else
        {
            _player.GetComponent<PlayerScript>().SetJoystick(_floatingJoystick);
            _fixedJoystick.gameObject.SetActive(false);
            _floatingJoystick.gameObject.SetActive(true);
        }
    }

    public void DisplayDodgeRefreshed()
    {
        StartCoroutine(DestroyDodgeRefreshed());
    }

    public IEnumerator DestroyDodgeRefreshed()
    {
        GameObject reference = Instantiate(_dodgeRefreshed, _player.transform.position, Quaternion.identity, gameObject.transform);

        yield return new WaitForSeconds(3f);

        Destroy(reference);
    }

    public void DisplayPauseScreen(bool cond)
    {
        _pauseScreen.SetActive(cond);
    }

    public void AddScore(int score, Vector2 enemyPosition)
    {
        StartCoroutine(DisplayAddScoreText(score, enemyPosition));
    }

    public IEnumerator DisplayAddScoreText(int score, Vector2 enemyPosition)
    {
        GameObject reference = Instantiate(_dodgeRefreshed, enemyPosition, Quaternion.identity, gameObject.transform);
        TextMeshProUGUI addText = reference.GetComponentInChildren<TextMeshProUGUI>();
        addText.text = "+ " + score.ToString();
        addText.fontSize = 50;
        addText.color = Color.black;
        addText.fontStyle = FontStyles.Bold;

        yield return new WaitForSeconds(3f);

        Destroy(reference);
    }


    public void ExitGame()
    {
        Application.Quit();
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        _gameManager.SetGameIsStarted(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
        _gameManager.SetGameIsStarted(true);
    }
}
