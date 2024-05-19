using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform target; // El objetivo al que seguir� la c�mara
    [SerializeField] float speed = 5f; // La velocidad de movimiento de la c�mara
    [SerializeField] Vector2 minLimits; // Los l�mites m�nimos en X e Y
    [SerializeField] Vector2 maxLimits; // Los l�mites m�ximos en X e Y

    void Update()
    {
        // Calcular la posici�n objetivo de la c�mara
        Vector3 targetPosition = target.position;
        targetPosition.z = transform.position.z; // Mantener la misma posici�n Z de la c�mara

        // Limitar la posici�n objetivo dentro de los l�mites
        targetPosition.x = Mathf.Clamp(targetPosition.x, minLimits.x, maxLimits.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minLimits.y, maxLimits.y);

        // Dibujar l�neas de debug para visualizar los l�mites en el editor
        Debug.DrawLine(new Vector3(minLimits.x, minLimits.y, transform.position.z), new Vector3(minLimits.x, maxLimits.y, transform.position.z), Color.red);
        Debug.DrawLine(new Vector3(minLimits.x, maxLimits.y, transform.position.z), new Vector3(maxLimits.x, maxLimits.y, transform.position.z), Color.red);
        Debug.DrawLine(new Vector3(maxLimits.x, maxLimits.y, transform.position.z), new Vector3(maxLimits.x, minLimits.y, transform.position.z), Color.red);
        Debug.DrawLine(new Vector3(maxLimits.x, minLimits.y, transform.position.z), new Vector3(minLimits.x, minLimits.y, transform.position.z), Color.red);

        // Interpolar suavemente la posici�n actual de la c�mara hacia la posici�n objetivo
        transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
    }
}