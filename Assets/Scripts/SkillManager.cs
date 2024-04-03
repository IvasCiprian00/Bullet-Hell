using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SkillManager : MonoBehaviour
{
    [SerializeField] GameObject _player;
    [SerializeField] GameManager _gameManager;

    [SerializeField] private GameObject _bullet;
    [SerializeField] private GameObject _aimsight;
    private bool _isAiming;
    private float _cooldownTimer;
    [SerializeField] private float _aimCooldown;
    private float _aimTimer;
    [SerializeField] private float _aimDuration;
    private float fixedDeltaTime;


    private Vector3 _initialPosition;
    private void Awake()
    {
        _player = GameObject.Find("Player");
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        this.fixedDeltaTime = Time.fixedDeltaTime;
    }


    private void Update()
    {
        _cooldownTimer += Time.deltaTime;

        ShootController();

        if(_gameManager.GetScore() >= 50)
        {
            _aimCooldown = 8f;
        }
    }

    public void ShootController()
    {
        if (Input.GetButtonDown("Fire1") && _cooldownTimer >= _aimCooldown && !_isAiming)
        {
            if (CheckMousePosition())
            {
                _initialPosition = Input.mousePosition;
                Debug.Log(Input.mousePosition);
                _aimsight.SetActive(true);
                _isAiming = true;
                _aimTimer = 0f;
                Time.timeScale = 0.5f;
            }
        }

        if (_isAiming)
        {
            TouchInput();

            _aimTimer += Time.deltaTime;
            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;

            if (Input.GetButtonUp("Fire1") || _aimTimer >= _aimDuration)
            {
                GameObject reference = Instantiate(_bullet, _player.transform.position, Quaternion.identity);
                reference.transform.rotation = _aimsight.transform.rotation;
                reference.transform.Rotate(0, 0, -90);
                _isAiming = false;
                _aimsight.SetActive(false);
                _cooldownTimer = 0f;
                Time.timeScale = 1.0f;
            }
        }
    }

    public bool CheckMousePosition()
    {
        float x = Input.mousePosition.x;
        float y = Input.mousePosition.y;

        if(x <= 200 && y <= 200)
        {
            return false;
        }

        if(x >= 960)
        {
            return false;
        }

        return true;
    }

    public void MouseInput()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0;
        Vector3 direction = (mousePos - _player.transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _aimsight.transform.rotation = Quaternion.Euler(0, 0, angle);
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
