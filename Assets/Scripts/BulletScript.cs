using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private int _damage; 

    private void Update()
    {
        transform.Translate(Vector3.up * 30 * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy" || collision.tag == "Orb")
        {
            GameObject.Find("Game Manager").GetComponent<GameManager>().AddScore(0);
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        if(collision.tag == "Obstacle")
        {
            Destroy(gameObject);
        }
        if(collision.tag == "BossPart")
        {
            collision.transform.parent.GetComponent<RobotBossScript>().TakeDamage(1);
            Destroy(gameObject);
        }
    }

    public void SetDamage(int damage)
    {
        _damage = damage;
    }
}
