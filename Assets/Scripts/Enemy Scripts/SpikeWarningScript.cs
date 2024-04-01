using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SpikeWarningScript : MonoBehaviour
{
    [SerializeField] private GameObject _timer;
    [SerializeField] private GameObject _spike;
    [SerializeField] private float _tickSpeed;

    [SerializeField] private GameObject _player;
    [SerializeField] private bool _homing;

    private void Awake()
    {
        _player = GameObject.Find("Player");
    }

    void Update()
    {
        _timer.transform.localScale += new Vector3(Time.deltaTime, Time.deltaTime, 0) * _tickSpeed;

        if(_timer.transform.localScale.x >= 2.2)
        {
            Instantiate(_spike, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }

        if(_player == null)
        {
            return;
        }

        if (_homing)
        {
            transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, Vector2.Distance(transform.position, _player.transform.position) * 2 * Time.deltaTime);
        }
    }
}
