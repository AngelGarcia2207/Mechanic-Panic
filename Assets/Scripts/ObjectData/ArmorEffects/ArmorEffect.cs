using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorEffect : ScriptableObject
{
    //Función base para ejectuar el efecto. Cada efecto la adapta a su modo.
    public virtual void PlayEffect(){}
}
