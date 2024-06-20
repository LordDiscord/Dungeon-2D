using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    private static DontDestroyOnLoad instance;

    private void Awake()
    {
        // Check if there is already an instance of this object
        if (instance == null)
        {
            // If not, set this as the instance and make it persist
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If there is already an instance, destroy this duplicate
            Destroy(gameObject);
        }
    }
}