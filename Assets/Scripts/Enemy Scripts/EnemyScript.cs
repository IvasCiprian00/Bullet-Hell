using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    //private SoundManager _soundManager;

    [SerializeField] private GameObject _sprite;
    [SerializeField] private GameObject _player;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private GameObject _projectile;
    [SerializeField] private float _fireRate;
    [SerializeField] private GameObject _projSpawnLocation;
    [SerializeField] private int _projectileSpeed;
    [SerializeField] private float _projectileRotationSpeed;
    [SerializeField] private float _accuracy;
    [SerializeField] private bool _isPredicting;
    [SerializeField] private Slider _fireWarning;
    private float _timer;

    [Header("Sound")]
    [SerializeField] private AudioClip _shootSound;
    [SerializeField] private AudioClip _deathSound;

    private void Start()
    {
        _player = GameObject.Find("Player");
        _fireWarning = GetComponentInChildren<Slider>();
        _fireWarning.maxValue = _fireRate;

        _projectile.GetComponent<ProjectileScript>().SetSpeed(_projectileSpeed);
        _projectile.GetComponent<ProjectileScript>().SetRotationSpeed(_projectileRotationSpeed);

        //_soundManager = GameObject.Find("Sound Manager").GetComponent<SoundManager>();
    }

    private void Update()
    {
        if (_player == null) 
        {
            return;
        }

        _fireWarning.value = _timer;

        Movement();

        ShootingController();
    }

    public void ShootingController()
    {
        _timer += Time.deltaTime;

        if (_timer >= _fireRate)
        {
            SoundManager.PlaySound(SoundType.ENEMY_SHOOT);
            if (_isPredicting)
            {
                FirePredicting();
            }
            else
            {
                FireNormal();
            }
        }
    }

    public void FirePredicting()
    {
        _timer = 0;
        Vector2 playerSpeed = _player.GetComponent<Rigidbody2D>().velocity;
        float distance = Vector2.Distance(gameObject.transform.position, _player.transform.position);
        Vector3 futurePosition = _player.transform.position + new Vector3(playerSpeed.x, playerSpeed.y, 0) * (distance / _projectileSpeed);

        GameObject reference = Instantiate(_projectile, _projSpawnLocation.transform.position, transform.rotation);
        reference.transform.up = futurePosition - reference.transform.position;
    }

    public void FireNormal()
    {
        _timer = 0;
        GameObject reference = Instantiate(_projectile, _projSpawnLocation.transform.position, transform.rotation);
        reference.transform.up = _player.transform.position - reference.transform.position;

        float deviation = Random.Range(-_accuracy, _accuracy);
        reference.transform.Rotate(0, 0, deviation);
    }

    public void Movement()
    {
        _sprite.transform.right = _player.transform.position - transform.position;

        //transform.Translate(Vector3.down * _moveSpeed * Time.deltaTime);
        /*float distance = Vector3.Distance(transform.position, _player.transform.position);
        //if (Mathf.Abs(Mathf.Abs(transform.position.x) - Mathf.Abs(_player.transform.position.x)) >= 9f || Mathf.Abs(Mathf.Abs(transform.position.y) - Mathf.Abs(_player.transform.position.y)) >= 5f)
        float x = transform.position.x;
        float y = transform.position.y;

        if(x <= _player.transform.position.x + 10f && x >= _player.transform.position.x - 10f && y <= _player.transform.position.y + 6f && y >= _player.transform.position.y - 6f)
        {
            return;
        }
        transform.Translate(Vector2.right * (distance - 3f) * Time.deltaTime);*/
    }

    public void OnDestroy()
    {
        SoundManager.PlaySound(SoundType.DEATH);
    }
}
