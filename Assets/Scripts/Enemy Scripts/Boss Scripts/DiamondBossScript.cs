using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DiamondBossScript : MonoBehaviour
{
    private GameObject _player;
    [SerializeField] private int _hp;
    [SerializeField] private Animator _animator;

    [Header("Laser Attack")]
    [SerializeField] private GameObject _laser;
    [SerializeField] private GameObject _laserContainer;
    [SerializeField] private GameObject _laserWarning;
    [SerializeField] private GameObject _laserWarningContainer;
    [SerializeField] private float _laserCooldown;
    [SerializeField] private float _laserDuration;
    [SerializeField] private float _laserSpeed;
    [SerializeField] private float _laserMaxSpeed;
    [SerializeField] private bool _laserIsActive;
    [SerializeField] private bool _laserCanFire;
    private bool _laserAttackStarted;

    [Header("Wave Attack")]
    [SerializeField] private GameObject _wave;
    [SerializeField] private float _waveCooldown;
    [SerializeField] private int _waveCount;
    [SerializeField] private float _waveFireSpeed;
    [SerializeField] private float _waveSpeed;
    [SerializeField] private bool _waveIsActive;
    [SerializeField] private bool _waveCanFire;

    [Header("Spike Attack")]
    [SerializeField] private GameObject _spike;
    [SerializeField] private float _spikeCooldown;
    [SerializeField] private float _spikeCount;
    [SerializeField] private bool _spikeCanFire;

    private bool _hasLanded;


    public void Awake()
    {
        _player = GameObject.Find("Player");
    }
    public void Update()
    {
        if (_player == null)
        {
            return;
        }
        if (!_hasLanded)
        {
            transform.position = Vector2.MoveTowards(transform.position, Vector2.zero, 5f * Time.deltaTime);
            if(Vector2.Distance(transform.position, Vector2.zero) <= 0.01)
            {
                _hasLanded = true;
            }

            return;
        }

        if (_laserCanFire && !_waveIsActive)
        {
            StartCoroutine(FireLaser());
        }
        if (_laserIsActive)
        {
            _laserSpeed += Time.deltaTime;
            _laserSpeed = Mathf.Clamp(_laserSpeed, 0, _laserMaxSpeed);
            RotateLasers();
        }

        if(_waveCanFire && !_laserAttackStarted)
        {
            StartCoroutine(FireWaves());
        }

        if (_spikeCanFire)
        {
            StartCoroutine(FireSpikes());
        }
    }

    public IEnumerator FireLaser()
    {
        _laserAttackStarted = true;
        _laserCanFire = false;

        SpawnLaserWarning();

        yield return new WaitForSeconds(1f);

        _laserIsActive = true;
        _laserSpeed = 0;

        DestroyLaserWarning();
        SpawnLasers();

        yield return new WaitForSeconds(_laserDuration);

        _laserIsActive = false;

        DestroyLasers();

        yield return new WaitForSeconds(1f);
        _laserAttackStarted = false;

        yield return new WaitForSeconds(_laserCooldown);

        _laserCanFire = true;
    }

    public void SpawnLasers()
    {
        for(int i = 0; i < 360; i += 60)
        {
            Instantiate(_laser, transform.position, Quaternion.Euler(0, 0, i), _laserContainer.transform);
        }
    }
    public void SpawnLaserWarning()
    {
        for (int i = 0; i < 360; i += 60)
        {
            GameObject reference = Instantiate(_laserWarning, transform.position, Quaternion.Euler(0, 0, i), _laserWarningContainer.transform);
            reference.SetActive(true);
        }
    }

    public void RotateLasers()
    {
        _laserContainer.transform.Rotate(0, 0, _laserSpeed * 20 * Time.deltaTime);
    }

    public void DestroyLasers()
    {
        foreach (Transform child in _laserContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void DestroyLaserWarning()
    {
        foreach (Transform child in _laserWarningContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public IEnumerator FireWaves()
    {
        _waveCanFire = false;
        _waveIsActive = true;

        for (int i = 0; i < _waveCount; i++)
        {
            if(_player == null)
            {
                continue;
            }
            GameObject reference = Instantiate(_wave, transform.position, Quaternion.identity);
            reference.transform.right = _player.transform.position - transform.position;
            reference.GetComponent<WaveScript>().SetSpeed(_waveSpeed);

            yield return new WaitForSeconds(_waveFireSpeed);
        }

        _waveIsActive = false;

        yield return new WaitForSeconds(_waveCooldown);

        _waveCanFire = true;
    }

    public IEnumerator FireSpikes()
    {
        _spikeCanFire = false;

        for(int i = 0; i < _spikeCount; i++) 
        {
            //Instantiate spikes around player 
            int xLimit = 6;
            int yLimit = 5;
            float x = Random.Range(_player.transform.position.x - xLimit, _player.transform.position.x + xLimit);
            float y = Random.Range(_player.transform.position.y - yLimit, _player.transform.position.y + yLimit);

            //should check if position is valid(so they don't spawn outside the map or on top of the boss

            Instantiate(_spike, new Vector3(x, y, 0), Quaternion.identity);
        }

        yield return new WaitForSeconds(_spikeCooldown);

        _spikeCanFire = true;
    }

    public void TakeDamage(int dmg)
    {
        _hp -= dmg;

        if(_hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Bullet")
        {
            Destroy(collision.gameObject);
            TakeDamage(1);
        }
    }
}
