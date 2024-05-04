using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public abstract class Wave : MonoBehaviour
{
    private GameObject _player;
    [SerializeField] private GameObject _enemyIndicator;

    [SerializeField] private GameObject[] _enemies;
    [SerializeField] private int[] _enemyCount;
    [SerializeField] private float[] _timeGiven;

    float _xPos;
    float _yPos;

    public void Start()
    {
        _player = GameObject.Find("Player");
    }

    public IEnumerator SpawnWave()
    {
        for(int i = 0; i < _enemies.Length; i++)
        {
            SpawnEnemy(_enemies[i], _enemyCount[i]);

            yield return new WaitForSeconds(_timeGiven[i]);
        }
    }

    public void SpawnEnemy(GameObject enemy, int enemyCount)
    {
        if (_player == null)
        {
            return;
        }

        for (int i = 0; i < enemyCount; i++)
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

            if (Vector2.Distance(new Vector2(x, y), Vector2.zero) > 25 || Vector2.Distance(_player.transform.position, new Vector2(x, y)) <= 10)
            {
                isValid = false;
            }
        } while (!isValid);

        _xPos = x;
        _yPos = y;
    }
}
