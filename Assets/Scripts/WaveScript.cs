using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveScript : MonoBehaviour
{
    private UIManager _uiManager;
    private Rigidbody2D _rigidbody;
    private float _speed;

    private void Start()
    {
        _uiManager =  GameObject.Find("Canvas").GetComponent<UIManager>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        transform.Translate(Vector3.right * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (!collision.GetComponent<PlayerScript>().IsDodging())
            {
                return;
            }

            collision.GetComponent<PlayerScript>().RefreshDodgeCooldown();

            _uiManager.DisplayDodgeRefreshed();
        }
    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }
}
