using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] float threshold;
    private Transform player;

    void Update()
    {
        // Intenta encontrar el GameObject del jugador si aún no se ha asignado
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            return; // Salir del método para evitar errores si el jugador aún no se ha encontrado
        }

        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 targetPos = player.position + mousePos;

        // Mantener la posición Z del objeto sin cambios
        targetPos.z = transform.position.z;

        // Aplicar límites en X e Y
        targetPos.x = Mathf.Clamp(targetPos.x, -threshold + player.position.x, threshold + player.position.x);
        targetPos.y = Mathf.Clamp(targetPos.y, -threshold + player.position.y, threshold + player.position.y);

        // Asignar la nueva posición al objeto
        transform.position = targetPos;
    }
}