using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondBossScript : MonoBehaviour
{
    private GameObject _player;

    [Header("Laser Attack")]
    [SerializeField] private GameObject _laser;
    [SerializeField] private GameObject _laserContainer;
    [SerializeField] private float _laserCooldown;
    [SerializeField] private float _laserDuration;
    [SerializeField] private float _laserSpeed;
    [SerializeField] private float _laserMaxSpeed;
    [SerializeField] private bool _laserIsActive;
    [SerializeField] private bool _laserCanFire;

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


    public void Awake()
    {
        _player = GameObject.Find("Player");
    }

    public void Update()
    {
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

        if(_waveCanFire && !_laserIsActive)
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
        _laserCanFire = false;
        _laserIsActive = true;
        _laserSpeed = 0;

        SpawnLasers();

        yield return new WaitForSeconds(_laserDuration);

        _laserIsActive = false;

        DestroyLasers();

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

    public IEnumerator FireWaves()
    {
        _waveCanFire = false;
        _waveIsActive = true;

        for (int i = 0; i < _waveCount; i++)
        {
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
            float x = Random.Range(_player.transform.position.x - 5f, _player.transform.position.x + 5f);
            float y = Random.Range(_player.transform.position.y - 4f, _player.transform.position.y + 4f);

            //should check if position is valid(so they don't spawn outside the map or on top of the boss

            Instantiate(_spike, new Vector3(x, y, 0), Quaternion.identity);
        }

        yield return new WaitForSeconds(_spikeCooldown);

        _spikeCanFire = true;
    }
}
