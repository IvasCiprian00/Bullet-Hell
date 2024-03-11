using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _enemies;
    [SerializeField] private float _spawnInterval;
    [SerializeField] private float _maxSpawnInterval;
    [SerializeField] private GameObject _player;
    private int phase;

    private float _timer;
    private float _spawnTimer;
    private float _xPos;
    private float _yPos;


    private void Awake()
    {
        phase = 1;
        _player = GameObject.Find("Player");
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if(_player == null) 
        {
            return;

        }

        if(_timer >= 30)
        {
            phase = 2;
        }

        switch (phase) 
        {
            case 1:
                Phase1();
                break;
            case 2:
                Phase2();
                break;
            default: 
                break;
        }

    }

    public void Phase1()
    {
        _spawnTimer += Time.deltaTime;

        if (_spawnTimer < _spawnInterval)
        {
            return;
        }

        _spawnTimer = 0;
        _spawnInterval -= 0.2f;
        GetValidSpawnPosition();
        Instantiate(_enemies[0], new Vector3(_xPos, _yPos, 0), Quaternion.identity);
    }

    public void Phase2()
    {
        _spawnTimer += Time.deltaTime;

        if (_spawnTimer < _spawnInterval)
        {
            return;
        }

        _spawnTimer = 0;
        GetValidSpawnPosition();
        Instantiate(_enemies[1], new Vector3(_xPos, _yPos, 0), Quaternion.identity);
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
