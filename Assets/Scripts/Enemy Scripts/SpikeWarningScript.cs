using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SpikeWarningScript : MonoBehaviour
{
    [SerializeField] private GameObject _timer;
    [SerializeField] private GameObject _spike;

    void Update()
    {
        _timer.transform.localScale += new Vector3(Time.deltaTime, Time.deltaTime, 0) * 3;

        if(_timer.transform.localScale.x >= 2.2)
        {
            Instantiate(_spike, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
