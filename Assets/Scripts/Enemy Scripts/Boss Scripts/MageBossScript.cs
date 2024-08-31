using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.UIElements.ToolbarMenu;

public class MageBossScript : MonoBehaviour
{
    [Serializable] public struct TransformSettings
    {
        public Vector3 positionChange;
        public Vector3 rotationChange;
    };

    [SerializeField] private bool _canAttack;
    [SerializeField] private int _attackCooldown;

    [Header("Wall Ability")]
    [SerializeField] private GameObject _wall;
    private GameObject _wallReference;

    [Header("Projectile Attack")]
    [SerializeField] private GameObject _projectile;
    [SerializeField] private GameObject _projectileContainer;
    [SerializeField] private float _projectileContainerSpeed;
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private float _projectileWaveCount;
    [SerializeField] private float _projectileInterval;
    [SerializeField] private GameObject[] _projectileSpawnLocations;

    [Header("Wall Opening Attack")]
    [SerializeField] private GameObject _wallOpening;
    [SerializeField] private int _wallCount;
    [SerializeField] private float _wallSpeed;
    [SerializeField] private float _wallInterval;

    [Header("Wave Attack")]
    [SerializeField] private GameObject _wave;
    [SerializeField] private int _waveCount;
    [SerializeField] private float _waveInterval;
    [SerializeField] private float _waveSpeed;
    [SerializeField] private TransformSettings[] _transformSetting;
    private int _attackDirection;

    [Header("Misc")]
    [SerializeField] private GameObject _player;
    [SerializeField] private float _hp;
    [SerializeField] private float _initialHp;

    public void Awake()
    {
        _player = GameObject.Find("Player");
        _hp = _initialHp;
    }

    public void Start()
    {
        _wallReference = Instantiate(_wall, _player.transform.position, Quaternion.identity);
        _projectileContainer = Instantiate(_projectileContainer, _player.transform.position, Quaternion.identity);

        int i = 0;
        foreach(Transform child in _projectileContainer.transform)
        {
            _projectileSpawnLocations[i] = child.gameObject;
            i++;
        }

        _projectileContainer.SetActive(false);
    }

    public void Update()
    {

        if (_player == null)
        {
            return;
        }

        if(UnityEngine.Random.Range(0f, 100f) <= 30 * Time.deltaTime)
        {
            _projectileContainerSpeed *= -1;
        }

        _projectileContainer.transform.Rotate(0, 0, _projectileContainerSpeed * Time.deltaTime);

        if (_canAttack)
        {
            _canAttack = false;

            int attack = UnityEngine.Random.Range(0, 3);
            switch (attack)
            {
                case 0:
                    StartCoroutine(WaveAttack());
                    break;
                case 1:
                    StartCoroutine(ProjectileAttack());
                    break;
                case 2:
                    StartCoroutine(WallAttack());
                    break;
                default:
                    break;
            }
        }

        Movement();
    }

    public IEnumerator ProjectileAttack()
    {
        _projectileContainer.SetActive(true);

        GameObject reference;

        for (int i = 0; i < _projectileWaveCount; i++)
        {
            foreach(GameObject x in _projectileSpawnLocations)
            {
                reference = Instantiate(_projectile, x.transform.position, Quaternion.identity);
                reference.transform.up = x.transform.up;
                reference.GetComponent<ProjectileScript>().SetFixedSpeed((int)_projectileSpeed);
            }
            yield return new WaitForSeconds(_projectileInterval);
        }

        _projectileContainer.SetActive(false);

        yield return new WaitForSeconds(_attackCooldown);

        _canAttack = true;
    }

    public IEnumerator WaveAttack()
    {
        GameObject reference;

        for(int i = 0; i < _waveCount; i++)
        {
            _attackDirection = UnityEngine.Random.Range(0, 4);

            reference = Instantiate(_wave);
            reference.GetComponent<WaveScript>().SetSpeed(_waveSpeed);
            reference.transform.position = _wallReference.transform.position + _transformSetting[_attackDirection].positionChange;
            reference.transform.Rotate(_transformSetting[_attackDirection].rotationChange);


            yield return new WaitForSeconds(_waveInterval);
        }

        yield return new WaitForSeconds(_attackCooldown);

        _canAttack = true;
    }

    public IEnumerator WallAttack()
    {
        GameObject reference;
        float variance;

        for (int i = 0; i < _wallCount; i++)
        {
            _attackDirection = UnityEngine.Random.Range(0, 4);

            reference = Instantiate(_wallOpening);
            reference.GetComponent<WaveScript>().SetSpeed(_wallSpeed);
            reference.transform.position = _wallReference.transform.position + _transformSetting[_attackDirection].positionChange;
            reference.transform.Rotate(_transformSetting[_attackDirection].rotationChange);


            variance = UnityEngine.Random.Range(-2.5f, 2.5f);

            if (_attackDirection % 2 == 0)
            {
                reference.transform.position += new Vector3(0, variance, 0);
            }
            else
            {
                reference.transform.position += new Vector3(variance, 0, 0);
            }

            yield return new WaitForSeconds(_wallInterval);
        }

        yield return new WaitForSeconds(_attackCooldown);

        _canAttack = true;
    }

    public void Movement()
    {
        float distance = Vector2.Distance(transform.position, _player.transform.position);
        float step = (distance - 6) * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, step);

        //transform.up = _player.transform.position - transform.position;
    }

    public void TakeDamage(float damage)
    {
        _hp -= damage;

        if(_hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        
        if ((collision.tag == "Bullet"))
        {
            float damage = collision.GetComponent<BulletScript>().GetDamage();
            TakeDamage(damage);
            Destroy(collision.gameObject);
        }
    }
}
