using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ControlesSigns : MonoBehaviour
{
    
    [SerializeField] private TMP_Text textMeshPro;

    public AudioSource textSound;
    // Texto que se mostrará al colisionar, editable desde el Inspector
    [SerializeField]
    private string messageOnCollision = "¡Has colisionado con el jugador!";

    // Método para inicializar el componente al inicio
    private void Start()
    {
        // Obtiene el componente TextMeshPro del objeto actual
        textMeshPro = GetComponentInChildren<TMP_Text>();

        // Asegúrate de que el texto esté inicialmente desactivado
        if (textMeshPro != null)
        {
            textMeshPro.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        // Verifica si el objeto que colisiona tiene el tag "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // Activa el texto
            if (textMeshPro != null)
            {
                textMeshPro.gameObject.SetActive(true);
                textMeshPro.text = messageOnCollision; // Usa el mensaje definido en el Inspector

                if (textSound != null && !textSound.isPlaying)
                {
                    textSound.Play();
                }
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Desactiva el texto si es necesario
            if (textMeshPro != null)
            {
                textMeshPro.gameObject.SetActive(false);
            }
        }
    }
}
