using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButtonScript : MonoBehaviour
{
    [SerializeField] private Image _cooldownImage;
    [SerializeField] private float _cooldownTime;


    private void Update()
    {
        _cooldownImage.fillAmount -= Time.deltaTime / _cooldownTime;
    }

    public void ResetCooldown() 
    { 
        if(_cooldownImage.fillAmount > 0)
        {
            return;
        }

        _cooldownImage.fillAmount = 1; 
    }

    public void SetCooldownTime(float cooldownTime) {  _cooldownTime = cooldownTime; }
}
