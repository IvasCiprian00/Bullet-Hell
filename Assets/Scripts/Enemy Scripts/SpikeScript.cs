using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    IEnumerator Start()
    {
        if(Random.Range(1, 3) == 1)
        {
            transform.Rotate(0, 180, 0);
        }
        yield return new WaitForSeconds(4f);

        Destroy(gameObject);
    }

}
