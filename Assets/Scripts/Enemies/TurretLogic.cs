using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class EnemyShoot : MonoBehaviour
{
    [SerializeField] private GameObject damageTag;
    [SerializeField] private int maxHealth;
    [SerializeField] private GameObject smokeEffect;
    public int currentHealth;
    
    private List<Collider> hittingColliders = new();

    public GameObject projectilePrefab; // Prefab del proyectil
    public AudioClip soundEffect; // Efecto de sonido al disparar
    public float detectionDistance = 10f; // Distancia de detecci�n del jugador
    [SerializeField] private float timer = 3f; // Tiempo entre disparos
    private float bulletTimer; // Temporizador para el disparo
    public Transform spawnpoint; // Punto de aparici�n del proyectil
    public float speedProjectile = 500f; // Velocidad del proyectil
    private AudioSource audioSource; // Componente AudioSource

    private Animator animator;

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Obtiene el AudioSource del objeto
        animator = GetComponent<Animator>();
        bulletTimer = timer; // Inicializa el temporizador
        currentHealth = maxHealth;
    }

    void Update()
    {
        Transform closestPlayer = GetClosestPlayer(); // Obtiene el jugador m�s cercano

        if (closestPlayer == null) return; // Si no hay jugadores, sale

        
        float distanceToPlayer = Vector3.Distance(transform.position, closestPlayer.position);

        if (distanceToPlayer <= detectionDistance && currentHealth > 0)
        {
            LookAtTarget(closestPlayer);
            animator.SetTrigger("Shoot"); // Activa la animaci�n de disparo
            DisparaJugador(closestPlayer); // Llama a la funci�n de disparo con el jugador m�s cercano
        }
    }

    void LookAtTarget(Transform target)
    {
        Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, target.position.z); // Ignora la altura
        transform.LookAt(targetPosition); // Mira al objetivo ajustado
    }

    Transform GetClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player"); // Busca todos los jugadores con el tag "Player"
        Transform closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject player in players)
        {
            if (player == null) continue; // Ignora si el jugador es nulo

            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = player.transform; // Actualiza al jugador m�s cercano
            }
        }

        return closest; // Devuelve el transform del jugador m�s cercano
    }

    void DisparaJugador(Transform target)
    {
        bulletTimer -= Time.deltaTime; // Reduce el temporizador

        if (bulletTimer > 0) return; // Si no ha pasado el tiempo, sale

        bulletTimer = timer; // Reinicia el temporizador

        GameObject Projectile = Instantiate(projectilePrefab, spawnpoint.position, spawnpoint.rotation);
        Rigidbody bullet = Projectile.GetComponent<Rigidbody>();

        if (bullet != null)
        {
            bullet.AddForce(spawnpoint.forward * speedProjectile); // Aplica fuerza al proyectil
        }

        Destroy(Projectile, 4f); 

        audioSource.PlayOneShot(soundEffect); 
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WeaponBase"))
        {
            Obj_Player_Weapon playerWeapon = other.GetComponent<Obj_Player_Weapon>();
            //Debug.Log(playerWeapon.DealDamage());
            GameObject newTag = Instantiate(damageTag, other.gameObject.transform.position, Quaternion.identity);
            newTag.GetComponent<TMP_Text>().text = playerWeapon.DealDamage().ToString();
            Destroy(newTag, 1f);

            //Debug.Log("sc");
            DamageEnemy(playerWeapon.DealDamage(), other);
        }
        else if (other.CompareTag("WeaponComplement"))
        {
            Obj_Player_Weapon playerWeapon = other.transform.parent.GetComponent<Obj_Player_Weapon>();
            //Debug.Log(playerWeapon.DealDamage(other.gameObject.transform.GetSiblingIndex()));
            playerWeapon.ActivateEffects(other.gameObject.transform.GetSiblingIndex(), transform);
            GameObject newTag = Instantiate(damageTag, other.gameObject.transform.position, Quaternion.identity);
            newTag.GetComponent<TMP_Text>().text = playerWeapon.DealDamage(other.gameObject.transform.GetSiblingIndex()).ToString();
            Destroy(newTag, 1f);

            //Debug.Log("sc2");
            DamageEnemy(playerWeapon.DealDamage(), other);
        }
    }

    private void DamageEnemy(int damage, Collider hittingCollider)
    {
        if (currentHealth > 0)
        {
            animator.SetBool("death", false);
            GameManager.Instance.increaseLevelScore(damage);


            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                GameObject smokeObj = Instantiate(smokeEffect, this.transform.position, Quaternion.identity);
                animator.SetBool("death", true);
                Destroy(gameObject, 2f);
            }
            

        }
    }
}


