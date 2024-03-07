using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private GameObject _player;

    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;

    private void Awake()
    {
        _player = GameObject.Find("Player");
    }

    void Update()
    {
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector2(0, _speed);
    }
}
