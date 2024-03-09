using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField] GameObject _player;

    [SerializeField] private GameObject _bullet;
    [SerializeField] private GameObject _aimsight;
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
        ShootController();
    }

    public void ShootController()
    {
        _cooldownTimer += Time.deltaTime;

        if (Input.GetButtonDown("Fire1") && _cooldownTimer >= _aimCooldown && !_isAiming)
        {
            _aimsight.SetActive(true);
            _isAiming = true;
            _aimTimer = 0f;
            Time.timeScale = 0.5f;
        }

        if (_isAiming)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            Vector3 direction = (mousePos - _player.transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _aimsight.transform.rotation = Quaternion.Euler(0, 0, angle);

            _aimTimer += Time.deltaTime;
            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;

            if (Input.GetButtonUp("Fire1") || _aimTimer >= _aimDuration)
            {
                Instantiate(_bullet, _player.transform.position, Quaternion.identity);
                _isAiming = false;
                _aimsight.SetActive(false);
                _cooldownTimer = 0f;
                Time.timeScale = 1.0f;
            }
        }
    }
}
