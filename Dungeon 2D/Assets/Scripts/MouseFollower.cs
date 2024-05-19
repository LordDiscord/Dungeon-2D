using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] float threshold;
    private Transform player;

    void Update()
    {
        // Intenta encontrar el GameObject del jugador si a�n no se ha asignado
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            return; // Salir del m�todo para evitar errores si el jugador a�n no se ha encontrado
        }

        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 targetPos = player.position + mousePos;

        // Mantener la posici�n Z del objeto sin cambios
        targetPos.z = transform.position.z;

        // Aplicar l�mites en X e Y
        targetPos.x = Mathf.Clamp(targetPos.x, -threshold + player.position.x, threshold + player.position.x);
        targetPos.y = Mathf.Clamp(targetPos.y, -threshold + player.position.y, threshold + player.position.y);

        // Asignar la nueva posici�n al objeto
        transform.position = targetPos;
    }
}