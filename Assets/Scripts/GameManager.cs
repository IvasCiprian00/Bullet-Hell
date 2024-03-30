using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private float _score;
    [SerializeField] private int _nrOfAliveEnemies;
    [SerializeField] private float _aliveEnemiesMultiplier;

    private void Update()
    {
        if(Vector2.Distance(_player.transform.position, Vector2.zero) >= 40)
        {
            _player.GetComponent<PlayerScript>().TakeDamage();
        }

        GetScoreMultiplier();
        _score += Time.deltaTime * 0.2f * _aliveEnemiesMultiplier;
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

    public int GetScore() { return (int) _score; }
}
