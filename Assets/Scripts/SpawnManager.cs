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

        yield return new WaitForSeconds(5f);

        SpawnEnemy(_enemies[0], 1);

        yield return new WaitForSeconds(5f);

        SpawnEnemy(_enemies[0], 2);

        yield return new WaitForSeconds(2f);

        SpawnEnemy(_enemies[0], 1);

        yield return new WaitForSeconds(2f);

        //StartCorountine(Phase2());
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

            Instantiate(enemy, new Vector3(_xPos, _yPos, 0), Quaternion.identity);
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
