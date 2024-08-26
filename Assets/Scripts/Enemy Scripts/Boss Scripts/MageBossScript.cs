using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageBossScript : MonoBehaviour
{
    [Header("Wall Ability")]
    [SerializeField] private int _wallCooldown;
    [SerializeField] private GameObject _wall;
    private GameObject _wallReference;
    [SerializeField] private int _wallDuration;
    [SerializeField] private bool _canCastWall;

    [Header("Misc")]
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

        if (_canCastWall)
        {
            _wallReference = Instantiate(_wall, _player.transform.position, Quaternion.identity);
            StartCoroutine(StartWallAttack());
        }

        Movement();
    }

    public IEnumerator StartWallAttack()
    {
        _canCastWall = false;
        
        yield return new WaitForSeconds(_wallDuration);

        Destroy(_wallReference);

        yield return new WaitForSeconds(_wallCooldown);

        _canCastWall = true;
    }

    public IEnumerator StartWallCooldown()
    {
        yield return new WaitForSeconds(_wallCooldown);

        _canCastWall = true;
    }

    public void Movement()
    {
        float distance = Vector2.Distance(transform.position, _player.transform.position);
        float step = (distance - 6) * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, step);

        //transform.up = _player.transform.position - transform.position;
    }
}
