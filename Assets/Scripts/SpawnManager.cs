using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _enemies;
    [SerializeField] private int _enemyLimit;
    [SerializeField] private float _spawnInterval;
    [SerializeField] private GameObject _player;

    private float _timer;
    private float _xPos;
    private float _yPos;


    private void Awake()
    {
        _player = GameObject.Find("Player");
    }

    private void Start()
    {
        _enemyLimit = 2;
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if(_player == null) 
        {
            return;
            
        }

        if (_timer < _spawnInterval)
        {
            return;
        }

        _timer = 0;
        GetValidSpawnPosition();
        int enemyNumber = Random.Range(1, _enemyLimit + 1);
        Instantiate(_enemies[enemyNumber], new Vector3(_xPos, _yPos, 0), Quaternion.identity);
    }

    public void GetValidSpawnPosition()
    {
        bool isValid;
        float x;
        float y;

        do
        {
            isValid = true;
            x = Random.Range(-14f, 14f);
            y = Random.Range(-9f, 9f);

            if (x <= _player.transform.position.x + 9f && x >= _player.transform.position.x - 9f && y <= _player.transform.position.y + 5f && y >= _player.transform.position.y - 5f)
            {
                isValid = false;
            }
        } while (!isValid);

        _xPos = x;
        _yPos = y;
    }
}
