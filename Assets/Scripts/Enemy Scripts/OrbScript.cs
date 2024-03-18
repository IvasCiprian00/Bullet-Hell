using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbScript : MonoBehaviour
{
    [SerializeField] private GameObject _projectile;
    float timer;

    public void Update()
    {
        timer += Time.deltaTime;

        if(timer >= 1)
        {
            Shoot();
            timer = 0;
        }
    }

    public void Shoot()
    {
        for(float i = 0.65f; i <= 1.4f; i += 0.35f)
        {
            GameObject reference = Instantiate(_projectile, transform.position, Quaternion.identity);
            reference.transform.rotation = transform.rotation * new Quaternion(0, 0, -i, 1);
       }
    }
}
