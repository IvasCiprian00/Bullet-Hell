using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeWarningScript : MonoBehaviour
{
    [SerializeField] private GameObject _timer;
    [SerializeField] private GameObject _spike;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(2.2f);

        Instantiate(_spike, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    void Update()
    {
        _timer.transform.localScale += new Vector3(Time.deltaTime, Time.deltaTime, 0);

    }
}
