using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _enemies;
    [SerializeField] private GameObject[] _bosses;
    [SerializeField] private float _spawnInterval;
    [SerializeField] private float _maxSpawnInterval;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _enemyIndicator;

    private float _xPos;
    private float _yPos;


    private void Awake()
    {
        _player = GameObject.Find("Player");

        StartCoroutine(Phase1());
    }

    public IEnumerator Phase1()
    {
        yield return new WaitForSeconds(3f);

        SpawnEnemy(_enemies[0], 1);

        yield return new WaitForSeconds(7f);

        SpawnEnemy(_enemies[0], 1);

        yield return new WaitForSeconds(7f);

        SpawnEnemy(_enemies[0], 2);

        yield return new WaitForSeconds(5f);

        SpawnEnemy(_enemies[0], 3);

        yield return new WaitForSeconds(7f);

        StartCoroutine(Phase2());
    }

    public IEnumerator Phase2()
    {
        SpawnEnemy(_enemies[1], 2);

        yield return new WaitForSeconds(3f);

        SpawnEnemy(_enemies[1], 3);

        yield return new WaitForSeconds(5f);

        SpawnEnemy(_enemies[0], 1);
        SpawnEnemy(_enemies[1], 3);

        yield return new WaitForSeconds(5f);

        SpawnEnemy(_enemies[0], 2);
        SpawnEnemy(_enemies[1], 2);

        yield return new WaitForSeconds(10f);

        StartCoroutine(EyeBossPhase());
    }

    public IEnumerator EyeBossPhase()
    {
        SpawnEnemy(_bosses[0], 1);

        yield return new WaitForSeconds(40f);
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
            x = Random.Range(-25f, 25f);
            y = Random.Range(-25f, 25f);

            if (Vector2.Distance(new Vector2(x, y), Vector2.zero) > 25 || Vector2.Distance(_player.transform.position, new Vector2(x, y)) <= 10)
            {
                isValid = false;
            }
        } while (!isValid);

        _xPos = x;
        _yPos = y;
    }
}
