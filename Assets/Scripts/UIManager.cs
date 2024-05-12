using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private TextMeshProUGUI _score;
    [SerializeField] private GameObject _dodgeRefreshed;
    [SerializeField] private GameObject _restartButton;
    [SerializeField] private GameObject _quitButton;
    [SerializeField] private GameObject _pauseScreen;
    private GameObject _player;

    private void Awake()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _player = GameObject.Find("Player");
    }

    private void Update()
    {
        _score.text = "Score: " + _gameManager.GetScore();
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
}
