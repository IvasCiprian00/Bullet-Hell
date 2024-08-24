using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelector : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private float _cooldown;

    public void DestroyBulletsSkill()
    {
        List<GameObject> projectiles = GameObject.FindGameObjectsWithTag("Projectile").ToList<GameObject>();
        foreach (GameObject projectile in projectiles)
        {
            Destroy(projectile);
        }
    }

    public void StartCooldown()
    {
        _button.interactable = false;
        StartCoroutine(Cooldown());

    }

    public IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(_cooldown);
        _button.interactable = true;
    }
}
