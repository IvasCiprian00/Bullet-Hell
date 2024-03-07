using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScript : MonoBehaviour
{
    [SerializeField] private GameObject _player;

    private void Awake()
    {
        _player = GameObject.Find("Player");
    }

    private void Update()
    {
        
    }
}
