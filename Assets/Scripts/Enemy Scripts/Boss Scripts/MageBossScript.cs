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
    [SerializeField] private int _hp;
    [SerializeField] private int _initialHp;

    public void Awake()
    {
        _player = GameObject.Find("Player");
        _hp = _initialHp;
    }

    public void Start()
    {
        _wallReference = Instantiate(_wall, _player.transform.position, Quaternion.identity);
    }

    public void Update()
    {

        if (_player == null)
        {
            return;
        }

        if (_canAttack)
        {
            _canAttack = false;
            int attack = UnityEngine.Random.Range(0, 2);
            switch (attack)
            {
                case 0: Debug.Log("Dodge waves");
                    //StartCoroutine(WaveAttack());
                    StartCoroutine(WallAttack());
                    break;
                case 1:
                    Debug.Log("Projectiles");
                    StartCoroutine(WallAttack());
                    break;
                case 2:
                    Debug.Log("Wall openenings");
                    break;
                default:
                    break;
            }
        }

        Movement();
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
                Debug.Log("YEY");
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
}
