using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscillatingScript : MonoBehaviour
{
    private float _timer;
    private float a;
    private float b;
    private float c;

    private void Awake()
    {
        b = gameObject.transform.parent.GetComponent<ProjectileScript>().GetSpeed();
        Debug.Log(b);
    }
    void Update()
    {
        _timer += Time.deltaTime;
        a = Mathf.Sin(_timer * Mathf.PI);
        c = Mathf.Sqrt(a * a + b * b);

        transform.localPosition = new Vector3(a, 0, 0);

        float angle = Mathf.Asin(a / c);
        Debug.Log(angle);
        transform.rotation = new Quaternion(0, 0, angle, 1);
    }
}
