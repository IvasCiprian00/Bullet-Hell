using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BossScript : MonoBehaviour
{

    private GameObject _player;
    [SerializeField] private GameObject _orbContainer;
    [SerializeField] private float _minSpeed;
    [SerializeField] private float _fireRate;
    private float _timer;

    void Start()
    {
        _player = GameObject.Find("Player");
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, _player.transform.position);
        float step = (distance - 7) * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, step);

        if(GameObject.FindGameObjectsWithTag("Orb").Length == 0)
        {
            Destroy(gameObject);
        }

        Attack();
    }

    public void Attack()
    {
        _timer += Time.deltaTime;

        if(_timer >= _fireRate)
        {
            _timer = 0;

            GameObject[] orbs = GameObject.FindGameObjectsWithTag("Orb");

            for(int i = 0; i < orbs.Length; i++)
            {
                if (orbs[i] != null)
                {
                    orbs[i].GetComponent<OrbScript>().Shoot();
                }
            }
        }
    }
}
