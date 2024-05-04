using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private TextMeshProUGUI _score;
    [SerializeField] private GameObject _dodgeRefreshed;
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
        GameObject reference = Instantiate(_dodgeRefreshed, new Vector3(0, 1, 0), Quaternion.identity, gameObject.transform);

        yield return new WaitForSeconds(3f);

        Destroy(reference);
    }
}
