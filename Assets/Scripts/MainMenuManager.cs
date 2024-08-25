using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private GameManager _gameManager;
    private SoundManager _soundManager;
    [SerializeField] private Animator _animator;

    public void Awake()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _soundManager = GameObject.Find("Sound Manager").GetComponent<SoundManager>();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
        _gameManager.SetGameIsStarted(true);
    }

    public void ToggleSettingsMenu()
    {
        _animator.SetBool("open_menu", !_animator.GetBool("open_menu"));
    }

    public void ClickButton(GameObject overlay)
    {
        overlay.SetActive(!overlay.activeSelf);
    }

    public void ToggleMusic()
    {
        _soundManager.musicSource.enabled = !_soundManager.musicSource.isActiveAndEnabled;
    }

    public void ToggleSFX()
    {
        _soundManager.SFXSource.enabled = !_soundManager.SFXSource.isActiveAndEnabled;
    }
}
