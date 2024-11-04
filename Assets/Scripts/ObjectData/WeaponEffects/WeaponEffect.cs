using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Esta clase es la plantilla para crear efectos de arma y armadura.
*/

public abstract class WeaponEffect : ScriptableObject
{
    //If true, the effect will only play after hitting an enemy
    [SerializeField] bool activateOnEnemyImpact;

    //Función base para ejectuar el efecto. Cada efecto la adapta a su modo.
    public virtual void PlayEffect(Transform target){}
}