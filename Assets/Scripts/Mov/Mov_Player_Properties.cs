using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mov_Player_Properties : MonoBehaviour
{
    public Sprite headSprite;
    public string name;
    public Animator spriteAnimator;
    //[SerializeField] private Inp_PlayerInstantiator playerInstantiator;
    //[SerializeField] private PlayerInput playerInput;
    //[SerializeField] private CharacterController player;
    public int maxHealth = 100;
    //[SerializeField] private int currentHealth;
    public float speed = 5;
    public float mass = 1;
    public float gravity = 25;
    public float jumpForce = 8;
    public float jumpTime = 0.25f;
    public float dodgeSpeed = 15;
    public float dodgeDuration = 0.5f;
    [Range(0f, 1f)] public float airFriction = 0.9f;
    public float floorRaycastDistance = 1;

}
