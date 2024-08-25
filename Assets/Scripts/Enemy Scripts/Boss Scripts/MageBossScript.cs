using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class MageBossScript : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private int _hp;
    [SerializeField] private int _initialHp;

    public void Awake()
    {
        _player = GameObject.Find("Player");
        _hp = _initialHp;
    }

    public void Update()
    {
        if (_player == null)
        {
            return;
        }
        Movement();

    }

    public void Movement()
    {
        float distance = Vector2.Distance(transform.position, _player.transform.position);
        float step = (distance - 6) * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, step);

        //transform.up = _player.transform.position - transform.position;
    }
}
