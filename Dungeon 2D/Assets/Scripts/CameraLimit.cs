using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform target; // El objetivo al que seguirá la cámara
    [SerializeField] float speed = 5f; // La velocidad de movimiento de la cámara
    [SerializeField] Vector2 minLimits; // Los límites mínimos en X e Y
    [SerializeField] Vector2 maxLimits; // Los límites máximos en X e Y

    void Update()
    {
        // Calcular la posición objetivo de la cámara
        Vector3 targetPosition = target.position;
        targetPosition.z = transform.position.z; // Mantener la misma posición Z de la cámara

        // Limitar la posición objetivo dentro de los límites
        targetPosition.x = Mathf.Clamp(targetPosition.x, minLimits.x, maxLimits.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minLimits.y, maxLimits.y);

        // Dibujar líneas de debug para visualizar los límites en el editor
        Debug.DrawLine(new Vector3(minLimits.x, minLimits.y, transform.position.z), new Vector3(minLimits.x, maxLimits.y, transform.position.z), Color.red);
        Debug.DrawLine(new Vector3(minLimits.x, maxLimits.y, transform.position.z), new Vector3(maxLimits.x, maxLimits.y, transform.position.z), Color.red);
        Debug.DrawLine(new Vector3(maxLimits.x, maxLimits.y, transform.position.z), new Vector3(maxLimits.x, minLimits.y, transform.position.z), Color.red);
        Debug.DrawLine(new Vector3(maxLimits.x, minLimits.y, transform.position.z), new Vector3(minLimits.x, minLimits.y, transform.position.z), Color.red);

        // Interpolar suavemente la posición actual de la cámara hacia la posición objetivo
        transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
    }
}