using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbScript : MonoBehaviour
{
    [SerializeField] private GameObject _projectile;
    float timer;

    public void Shoot()
    {
        GameObject reference = Instantiate(_projectile, transform.position, Quaternion.identity);
        reference.transform.rotation = transform.rotation * new Quaternion(0, 0, -1, 1);
        reference.GetComponent<ProjectileScript>().SetFixedSpeed(5);
    }
}
