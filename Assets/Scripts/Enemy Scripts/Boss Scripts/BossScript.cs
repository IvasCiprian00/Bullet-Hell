using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BossScript : MonoBehaviour
{

    private GameObject _player;
    [SerializeField] private float _minSpeed;

    void Start()
    {
        _player = GameObject.Find("Player");
    }

    void Update()
    {

        float distance = Vector2.Distance(transform.position, _player.transform.position);
        float step = (distance - 7) * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, step);
    }
}
