using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return new WaitForSeconds(4f);

        Destroy(gameObject);
    }

}
