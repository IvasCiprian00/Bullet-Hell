using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private Rigidbody2D _rigidbody;

    [Header("Dodge Section")]
    [SerializeField] private float _dodgeCooldown;
    [SerializeField] private bool _canDodge;
    private bool _isDodging;

    private void Awake()
    {
        _canDodge = true;
        _rigidbody = GetComponent<Rigidbody2D>();
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

            GetComponent<Collider2D>().enabled = true;
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
        _isDodging = true;
        _canDodge = false;
        GetComponent<Collider2D>().enabled = false;

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        _rigidbody.velocity = Vector2.zero;
        float dodgeForce = 100 / Mathf.Sqrt(x * x + y * y);
        _rigidbody.AddForce (new Vector2(x, y) * dodgeForce, ForceMode2D.Impulse);

        StartCoroutine(RefreshDodge());
    }

    public IEnumerator RefreshDodge()
    {
        yield return new WaitForSeconds(_dodgeCooldown);

        _canDodge = true;
    }

    public void TakeDamage()
    {
        Camera.main.transform.parent = null;
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            TakeDamage();
        }
        if(collision.tag == "Hazard")
        {
            TakeDamage();
        }
    }
}
