using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _projectile;
    [SerializeField] private float _fireRate;
    [SerializeField] private GameObject _projSpawnLocation;
    private float _timer;

    private void Start()
    {
        _player = GameObject.Find("Player");
    }

    private void Update()
    {
        if (_player == null) 
        {
            return;
        }

        Movement();

        _timer += Time.deltaTime;

        if(_timer >= _fireRate)
        {
            _timer = 0;
            Instantiate(_projectile, _projSpawnLocation.transform.position, transform.rotation);
        }
    }

    public void Movement()
    {
        transform.right = _player.transform.position - transform.position;

        float distance = Vector3.Distance(transform.position, _player.transform.position);
        //if (Mathf.Abs(Mathf.Abs(transform.position.x) - Mathf.Abs(_player.transform.position.x)) >= 9f || Mathf.Abs(Mathf.Abs(transform.position.y) - Mathf.Abs(_player.transform.position.y)) >= 5f)
        float x = transform.position.x;
        float y = transform.position.y;
        if(x <= _player.transform.position.x + 10f && x >= _player.transform.position.x - 10f && y <= _player.transform.position.y + 6f && y >= _player.transform.position.y - 6f)
        {
            return;
        }
        transform.Translate(Vector2.right * (distance - 3f) * Time.deltaTime);
    }
}
