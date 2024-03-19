using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingOrbs : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    void Update()
    {
        //transform.rotation += Quaternion(0, 0, 1, 1) * _rotationSpeed * Time.deltaTime;
        transform.Rotate(0, 0, _rotationSpeed * Time.deltaTime);
    }

    public void SetSpeed(int x)
    {
        _rotationSpeed = x;
    }
}
