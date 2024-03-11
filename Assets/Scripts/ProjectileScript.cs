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

    [SerializeField] private bool _isHoming;
    private bool _stopFollowing;
    private void Awake()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _player = GameObject.Find("Player");
        _rotationSpeed += Random.Range(-0.5f, 0.5f);
    }

    private IEnumerator Start()
    {
        transform.up = _player.transform.position - transform.position;

        yield return new WaitForSeconds(15f);
        _gameManager.AddScore(0);
        _stopFollowing = true;
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
}

    void Update()
    {
        if (_player == null)
        {
            return;
        }

        MoveTowardsPlayer();

        if (_stopFollowing)
        {
            return;
        }

        if (_isHoming)
        {
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
            _gameManager.AddScore(0);
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
        if(collision.tag == "Player")
        {
            _player.GetComponent<PlayerScript>().TakeDamage();
        }
    }

    public void SetSpeed(int x)
    {
        _speed = x;
        _speed += Random.Range(-1f, 0.5f);
    }
}
