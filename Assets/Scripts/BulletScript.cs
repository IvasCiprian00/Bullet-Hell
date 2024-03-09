using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private void Start()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector3 direction = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    private void Update()
    {
        transform.Translate(Vector3.up * 30 * Time.deltaTime);

        if(Mathf.Abs(transform.position.x) >= 30f  || Mathf.Abs(transform.position.y) >= 15f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            GameObject.Find("Game Manager").GetComponent<GameManager>().AddScore();
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
