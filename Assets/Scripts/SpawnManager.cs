using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    [Serializable] public struct Waves
    {
        public string name;
        public GameObject[] enemies;
        public int[] enemyCount;
        public float[] timeGiven;
    };

    [SerializeField] private Waves[] _waves;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _enemyIndicator;

    private float _xPos;
    private float _yPos;


    private void Awake()
    {
        _player = GameObject.Find("Player");
        int firstWave = GameObject.Find("Game Manager").GetComponent<GameManager>().GetFirstWaveIndex();
        StartCoroutine(SpawnWaves(firstWave));
    }

    public IEnumerator SpawnWaves(int firstWave)
    {
        for (int i = firstWave; i < _waves.Length; i++)
        {
            for(int j = 0; j < _waves[i].enemies.Length; j++)
            {
                SpawnEnemy(_waves[i].enemies[j], _waves[i].enemyCount[j]);

                yield return new WaitForSeconds(_waves[i].timeGiven[j]);
            }
        }
    }


    public void SpawnEnemy(GameObject enemy, int enemyCount)
    {
        if(_player == null)
        {
            return;
        }

        for(int i = 0; i < enemyCount; i++)
        {
            GetValidSpawnPosition();

            GameObject enemyReference = Instantiate(enemy, new Vector3(_xPos, _yPos, 0), Quaternion.identity);
            GameObject reference = Instantiate(_enemyIndicator, Vector3.zero, Quaternion.identity, GameObject.Find("Canvas").transform);
            reference.GetComponent<EnemyIndicatorScript>().SetEnemy(enemyReference);
        }
    }

    public void GetValidSpawnPosition()
    {
        bool isValid;
        float x;
        float y;

        do
        {
            isValid = true;
            x = Random.Range(-20f, 20f);
            y = Random.Range(-20f, 20f);

            if (Vector2.Distance(new Vector2(x, y), Vector2.zero) > 20 || Vector2.Distance(_player.transform.position, new Vector2(x, y)) <= 10)
            {
                isValid = false;
            }
        } while (!isValid);

        _xPos = x;
        _yPos = y;
    }
}
