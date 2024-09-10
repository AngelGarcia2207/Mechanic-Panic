using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Esta clase es la plantilla para crear efectos de arma y armadura.
*/

public abstract class WeaponEffect : ScriptableObject
{
    //Función base para ejectuar el efecto. Cada efecto la adapta a su modo.
    public virtual void PlayEffect(){}
}