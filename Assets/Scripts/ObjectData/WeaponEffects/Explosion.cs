using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExplosionEffect", menuName = "Item Effects/Explosion Effect", order = 0)]

public class Explosion : WeaponEffect
{
    [SerializeField] GameObject explosionParticles;
    [SerializeField] float size;
    [SerializeField] float duration;
    [SerializeField] int chance;

    public override void PlayEffect(Transform target)
    {
        if(Random.Range(0, 100) < chance)
        {
            GameObject newExplosion = Object.Instantiate(explosionParticles, target.position, Quaternion.identity);
            Destroy(newExplosion, duration);
        }
    }
}
