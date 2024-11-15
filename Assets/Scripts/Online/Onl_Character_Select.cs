using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Onl_Character_Select : NetworkBehaviour
{
    [HideInInspector] public Onl_Player_Controller onlController;

    //private Loc_Character_Select locCharSelect;

    void Start()
    {
        //locCharSelect = GetComponent<Loc_Character_Select>();
    }


    public void CharacterCrossSelect(int character, int previous)
    {
        CrossSelectNormal(character, previous); CrossSelectClientRPC(character, previous);
        if (!IsServer)
        {
            CrossSelectServerRPC(character, previous);
        }
    }

    [ServerRpc (RequireOwnership = false)]
    public void CrossSelectServerRPC(int character, int previous)
    {
        //CrossSelectNormal(character);
        CrossSelectClientRPC(character, previous);
    }

    [ClientRpc]
    public void CrossSelectClientRPC(int character, int previous)
    {
        CrossSelectNormal(character, previous);
    }

    public void CrossSelectNormal(int character, int previous)
    {
        GameObject.Find("selected_id_" + previous).GetComponent<Image>().enabled = false;
        GameObject.Find("selected_id_" + character).GetComponent<Image>().enabled = true;
    }
}
