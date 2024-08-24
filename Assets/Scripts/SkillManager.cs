using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SkillManager : MonoBehaviour
{
    [Serializable] public struct UpgradeInfo
    {
        public float damage;
        public float cooldown;
        public int projCount;
    };

    [SerializeField] private GameObject _player;
    [SerializeField] private Slider _shootCooldownSlider;
    [SerializeField] private GameManager _gameManager;

    [Header("Shooting Section")]
    [SerializeField] private UpgradeInfo[] _upgradeInfo;
    [SerializeField] private int _upgradeLevel;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private GameObject _aimsight;
    private bool _isAiming;
    private float _aimTimer;
    [SerializeField] private float _aimDuration;
    [SerializeField] private bool _canShoot;
    private float fixedDeltaTime;


    private Vector3 _initialPosition;
    private void Awake()
    {
        _canShoot = true;
        _player = GameObject.Find("Player");
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        this.fixedDeltaTime = Time.fixedDeltaTime;
    }

    private void Update()
    {
        _upgradeLevel = _gameManager.GetScore() / 30;
        if (_upgradeLevel >= 3)
        {
            _upgradeLevel = 3;
        }
        _shootCooldownSlider.maxValue = _upgradeInfo[_upgradeLevel].cooldown;

        if (_gameManager.IsGamePaused())
        {
            return;
        }

        if(_player == null)
        {
            return;
        }

        ShootController();
    }


    public void ShootController()
    {
        if (!_canShoot)
        {
            _shootCooldownSlider.value += Time.deltaTime;

            if(_shootCooldownSlider.value >= _shootCooldownSlider.maxValue)
            {
                _shootCooldownSlider.gameObject.SetActive(false);
            }

            return;
        }

        if (Input.GetButtonDown("Fire1") && !_isAiming)
        {
            _shootCooldownSlider.gameObject.SetActive(false);
            if (CheckMousePosition())
            {
                _initialPosition = Input.mousePosition;
                _aimsight.SetActive(true);
                _isAiming = true;
                _aimTimer = 0f;
                Time.timeScale = 0.5f;
            }
        }

        if (_isAiming)
        {
            //TouchInput();
            MouseInput();

            _aimTimer += Time.deltaTime;
            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;

            if (Input.GetButtonUp("Fire1") || _aimTimer >= _aimDuration)
            {
                FireProjectile();
                _shootCooldownSlider.gameObject.SetActive(true);

                _shootCooldownSlider.value = 0;

                StartCoroutine(StartAimCooldown());
            }
        }
    }

    public void FireProjectile()
    {
        InstantiateProjectiles();

        _isAiming = false;
        _aimsight.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void InstantiateProjectiles()
    {
        GameObject reference;
        reference = InstantiateSingleProjectile(0);
        reference.GetComponent<BulletScript>().SetDamage(_upgradeInfo[_upgradeLevel].damage);

        for (int i = 1; i < _upgradeInfo[_upgradeLevel].projCount; i++) 
        {
            float deviation = Random.Range(-15f, 15f);

            reference = InstantiateSingleProjectile(deviation);
            reference.GetComponent<BulletScript>().SetDamage(_upgradeInfo[_upgradeLevel].damage / 2);
        }
    }

    public GameObject InstantiateSingleProjectile(float deviation)
    {
        GameObject reference = Instantiate(_bullet, _player.transform.position, Quaternion.identity);
        reference.transform.rotation = _aimsight.transform.rotation;
        reference.transform.Rotate(0, 0, -90 + deviation);

        return reference;
    }

    IEnumerator StartAimCooldown()
    {
        _canShoot = false;

        yield return new WaitForSeconds(_upgradeInfo[_upgradeLevel].cooldown);

        _canShoot = true;
    }

    public bool CheckMousePosition()
    {
        return true;
        /*float x = Input.mousePosition.x;
        float y = Input.mousePosition.y;

        if(x <= 200 && y <= 200)
        {
            return false;
        }

        if(x <= 960)
        {
            return false;
        }

        return true;*/
    }

    public void MouseInput()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z = 0;
        Vector3 direction = (mousePos - _player.transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _aimsight.transform.right = mousePos - _player.transform.position ;
        //_aimsight.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void TouchInput()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0;
        Vector3 direction = (mousePos - _initialPosition).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _aimsight.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void IsClicked()
    {
        Debug.Log("YEY");
    }
}
