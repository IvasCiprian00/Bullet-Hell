using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBossScript : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private int _hp;

    [Header("Right Arm")]
    [SerializeField] private GameObject _rightArm;
    [SerializeField] private GameObject _aimSight;
    [SerializeField] private GameObject _laser;
    [SerializeField] private float _aimTimer;
    [SerializeField] private float _rTimer;
    [SerializeField] private float _rCooldown;
    [SerializeField] private bool _rIsFiring;
    [SerializeField] private bool _isAiming;

    [Header("Left Arm")]
    [SerializeField] private GameObject _leftArm;
    [SerializeField] private GameObject _projectile;
    [SerializeField] private float _lTimer;
    [SerializeField] private float _lCooldown;
    [SerializeField] private float _lFireSpeed;
    [SerializeField] private float _lAimDuration;
    [SerializeField] private float _alternatingAngle;
    [SerializeField] private bool _lIsFiring;
    [SerializeField] private Animator _animator;

    public float offset;
    private int _initialHp;

    public void Awake()
    {
        _player = GameObject.Find("Player");
        _initialHp = _hp;
    }

    public void Update()
    {
        if (_player == null)
        {
            return;
        }
        Movement();
        RightArmController();
        LeftArmController();
    }

    public void Movement()
    {
        float distance = Vector2.Distance(transform.position, _player.transform.position);
        float step = (distance - 6) * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, step);

        //transform.up = _player.transform.position - transform.position;
    }

    public void LeftArmController()
    {
        if (!_lIsFiring)
        {
            _lTimer += Time.deltaTime;
        }

        if (_lIsFiring)
        {
            _leftArm.transform.Rotate(0, 0, _alternatingAngle * 3f * Time.deltaTime);
            _lAimDuration += Time.deltaTime;
        }

        if(_lAimDuration >= 3f)
        {
            _lAimDuration = 0;
            _lIsFiring = false;
        }

        if(_lTimer > _lCooldown)
        {
            _lTimer = 0;
            _leftArm.transform.up = _leftArm.transform.position - _player.transform.position;
            _leftArm.transform.Rotate(0, 0, -_alternatingAngle);
            StartCoroutine(FireProjectiles());
        }
    }

    public IEnumerator FireProjectiles()
    {
        _lTimer = 0;
        _lIsFiring = true;

        while(_lIsFiring)
        {
            GameObject reference = Instantiate(_projectile, _leftArm.transform.position, _leftArm.transform.rotation);
            reference.transform.Rotate(0, 0, 180);
            yield return new WaitForSeconds(_lFireSpeed);
        }

        _leftArm.transform.rotation = Quaternion.identity;
        _alternatingAngle *= -1;
        _lIsFiring = false;
    }

    public void RightArmController()
    {
        if (!_rIsFiring)
        {
            _rTimer += Time.deltaTime;

        }

        if (_isAiming)
        {
            _rightArm.transform.up = _rightArm.transform.position - _player.transform.position;
        }

        if (_rTimer >= _rCooldown)
        {
            _rTimer = 0;
            StartCoroutine(FireLaser());
        }
    }

    public IEnumerator FireLaser()
    {
        _rIsFiring = true;
        _isAiming = true;
        _aimSight.SetActive(true);

        yield return new WaitForSeconds(_aimTimer);

        _isAiming = false;

        yield return new WaitForSeconds(0.3f);

        GameObject laserReference = Instantiate(_laser, _aimSight.transform.position, _aimSight.transform.rotation);

        _rightArm.transform.rotation = Quaternion.identity;
        _aimSight.SetActive(false);
        _rIsFiring = false;

        yield return new WaitForSeconds(0.2f);

        Destroy(laserReference);
    }

    public void TakeDamage(int dmg)
    {
        _hp -= dmg;

        CheckHpThreshold();

        if(_hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void CheckHpThreshold()
    {
        if(_hp <= 3 * _initialHp / 10)
        {
            _aimTimer = 1;
            _rCooldown = 2;

            _lCooldown = 1;
            _lFireSpeed = 0.1f;
            return;
        }

        if(_hp <= 7 * _initialHp / 10)
        {
            _aimTimer = 1.5f;
            _rCooldown = 3;

            _lCooldown = 2;
            _lFireSpeed = 0.2f;
            return;
        }
    }
}
