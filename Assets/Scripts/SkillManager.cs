using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField] GameObject _player;

    [SerializeField] private GameObject _bullet;
    private bool _isAiming;
    private float _cooldownTimer;
    [SerializeField] private float _aimCooldown;
    private float _aimTimer;
    [SerializeField] private float _aimDuration;
    private float fixedDeltaTime;

    private void Awake()
    {
        _player = GameObject.Find("Player");
        this.fixedDeltaTime = Time.fixedDeltaTime;
    }


    private void Update()
    {
        _cooldownTimer += Time.deltaTime;

        /*if (Input.GetButtonDown("Fire1"))
        {
            if (Time.timeScale == 1.0f)
                Time.timeScale = 0.5f;
            else
                Time.timeScale = 1.0f;
            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
        }*/

        if (Input.GetButtonDown("Fire1") && _cooldownTimer >= _aimCooldown && !_isAiming)
        {
            _isAiming = true;
            _aimTimer = 0f;
            Time.timeScale = 0.5f;
        }

        if (_isAiming)
        {
            _aimTimer += Time.deltaTime;
            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;

            if(Input.GetButtonUp("Fire1") || _aimTimer >= _aimDuration)
            {
                Instantiate(_bullet, _player.transform.position, Quaternion.identity);
                _isAiming = false;
                Time.timeScale = 1.0f;
            }
        }
    }
}
