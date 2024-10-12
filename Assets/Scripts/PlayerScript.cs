using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private SoundManager _soundManager;

    [SerializeField] private Joystick _moveJoystick;
    [SerializeField] private float _speed;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private Rigidbody2D _rigidbody;

    [Header("Dodge Section")]
    [SerializeField] private float _dodgeCooldown;
    [SerializeField] private float _dodgeForce;
    [SerializeField] private bool _canDodge;
    private bool _isDodging;

    [Header("Sound")]
    [SerializeField] private AudioClip _dodgeSound;

    private void Awake()
    {
        _canDodge = true;
        _rigidbody = GetComponent<Rigidbody2D>();
        GameObject.Find("Game Manager").GetComponent<GameManager>().SetPlayer();
        _soundManager = GameObject.Find("Sound Manager").GetComponent<SoundManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _canDodge)
        {
            Dodge();
        }

        if (_isDodging)
        {
            if(_rigidbody.velocity.magnitude >= 0.5f)
            {
                return;
            }

            _isDodging = false;
        }
    }

    void FixedUpdate()
    {
        if (_isDodging)
        {
            return;
        }

        Movement();
        JoystickMovement();
    }

    public void JoystickMovement()
    {
        //Debug.Log(_joystick.Horizontal);
        _rigidbody.AddForce(new Vector2(_moveJoystick.Horizontal, _moveJoystick.Vertical) * _speed, ForceMode2D.Impulse);

        if (_rigidbody.velocity.magnitude > _maxSpeed)
        {
            _rigidbody.velocity = _rigidbody.velocity.normalized * _maxSpeed;
        }
    }

    public void Movement()
    {
        _rigidbody.AddForce(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * _speed, ForceMode2D.Impulse);

        if (_rigidbody.velocity.magnitude > _maxSpeed)
        {
            _rigidbody.velocity = _rigidbody.velocity.normalized * _maxSpeed;
        }
    }

    public void Dodge()
    {
        if (!_canDodge)
        {
            return;
        }

        _isDodging = true;
        _canDodge = false;

        float x = _rigidbody.velocity.x;
        float y = _rigidbody.velocity.y;
        //float x = Input.GetAxisRaw("Horizontal");
        //float y = Input.GetAxisRaw("Vertical");

        _rigidbody.velocity = Vector2.zero;
        float dodgeForce = _dodgeForce / Mathf.Sqrt(x * x + y * y);
        _rigidbody.AddForce (new Vector2(x, y) * dodgeForce, ForceMode2D.Impulse);

        SoundManager.PlaySound(SoundType.DODGE);

        StartCoroutine(RefreshDodge());
    }
    
    public void JoystickDodge()
    {
        if (!_canDodge)
        {
            return;
        }

        _isDodging = true;
        _canDodge = false;

        float x = _moveJoystick.Horizontal;
        float y = _moveJoystick.Vertical;

        _rigidbody.velocity = Vector2.zero;
        float dodgeForce = _dodgeForce / Mathf.Sqrt(x * x + y * y);
        _rigidbody.AddForce (new Vector2(x, y) * dodgeForce, ForceMode2D.Impulse);

        StartCoroutine(RefreshDodge());
    }

    public IEnumerator RefreshDodge()
    {
        yield return new WaitForSeconds(_dodgeCooldown);

        _canDodge = true;
    }

    public void RefreshDodgeCooldown()
    {
        _canDodge = true;
    }

    public void TakeDamage()
    {
        if(_isDodging)
        {
            return;
        }

        Camera.main.transform.parent = null;
        SoundManager.PlaySound(SoundType.PLAYER_DEATH);
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy" || collision.tag == "Hazard" || collision.tag == "Boss")
        {
            TakeDamage();
        }
    }

    public bool IsDodging() { return _isDodging; }
    public void SetJoystick(Joystick joystick) { _moveJoystick = joystick; }
}
