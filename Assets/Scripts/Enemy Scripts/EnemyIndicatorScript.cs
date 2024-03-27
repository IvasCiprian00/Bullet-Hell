using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIndicatorScript : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _enemy;

    // Update is called once per frame
    private void Awake()
    {
        _player = GameObject.Find("Player");
    }

    void Update()
    {
        if(_enemy == null || _player == null)
        {
            Destroy(gameObject);
            return;
        }

        if (Vector2.Distance(_player.transform.position, _enemy.transform.position) >= 8f)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }

        transform.up = _enemy.transform.position - transform.position;

        float x = _player.transform.position.x;
        float y = _player.transform.position.y;

        transform.position = _player.transform.position + (_enemy.transform.position - _player.transform.position).normalized * 5;
    }

    EnemyIndicatorScript(GameObject enemy)
    {
        _enemy = enemy;
    }

    public void SetEnemy(GameObject enemy)
    {
        _enemy = enemy;
    }
}
