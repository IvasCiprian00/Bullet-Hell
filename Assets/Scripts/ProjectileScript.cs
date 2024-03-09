using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ProjectileScript : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _rotationDirection;

    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;
    private void Awake()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _player = GameObject.Find("Player");
        _speed += Random.Range(-1f, 0.5f);
        _rotationSpeed += Random.Range(-0.5f, 0.5f);
    }

    private void Start()
    {
        transform.up = _player.transform.position - transform.position;
    }

    void Update()
    {
        if (_player != null)
        {
            MoveTowardsPlayer();
            RotateTowardsPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        transform.Translate(Vector2.up * _speed * Time.deltaTime);
    }

    void RotateTowardsPlayer()
    {
        Vector3 targetDirection = _player.transform.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Obstacle")
        {
            Destroy(gameObject);
        }
        if(collision.tag == "Enemy")
        {
            _gameManager.AddScore();
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
        if(collision.tag == "Player")
        {
            _player.GetComponent<PlayerScript>().TakeDamage();
        }
    }
}
