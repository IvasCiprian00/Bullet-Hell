using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotBossScript : MonoBehaviour
{
    [SerializeField] private GameObject _leftArm;

    [SerializeField] private GameObject _player;

    private float _timer;
    [Header("Right Arm")]
    [SerializeField] private GameObject _rightArm;
    [SerializeField] private GameObject _aimSight;
    [SerializeField] private float _aimTimer;
    [SerializeField] private float _rTimer;
    [SerializeField] private float _rCooldown;
    [SerializeField] private bool _isFiring;
    [SerializeField] private bool _isAiming;

    public void Awake()
    {
        _aimTimer = 5f;
        _player = GameObject.Find("Player");
    }

    public void Update()
    {
        RightArmController();
    }

    public void RightArmController()
    {
        if (!_isFiring)
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
        _isFiring = true;
        _isAiming = true;
        _aimSight.SetActive(true);

        yield return new WaitForSeconds(_aimTimer);

        _isAiming = false;

        yield return new WaitForSeconds(1f);

        Debug.Log("PEW");

        _rightArm.transform.rotation = Quaternion.identity;
        _aimSight.SetActive(false);
        _isFiring = false;
    }
}
