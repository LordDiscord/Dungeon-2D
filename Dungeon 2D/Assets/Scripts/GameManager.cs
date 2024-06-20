using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float fadeDuration = 1f; // Duración del desvanecimiento
    public int level = 0;
    private BattleSystem battleSystem;
    private MainCharacter mainCharacter;
    public string musicObjectName = "Music";
    private string playerTarget = "PlayerMovePoint";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        if (sceneName == "Sala Combate niv1")
        {
            if (level != 0) {
                if (battleSystem == null)
                {
                    battleSystem = GameObject.FindObjectOfType<BattleSystem>();
                    battleSystem.canMove = false;
                }
                if (mainCharacter == null)
                {
                    mainCharacter = GameObject.FindObjectOfType<MainCharacter>();
                }
                mainCharacter.anim.SetBool("Moving", false);
            }
            level++; // cada vez que cargue la escena aumentara el nivel
        }
        StartCoroutine(FadeAndSwitchScenes(sceneName));
    }

    public void ReloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("This is the last scene.");
        }
    }

    public void LoadPreviousScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int previousSceneIndex = currentSceneIndex - 1;

        if (previousSceneIndex >= 0)
        {
            SceneManager.LoadScene(previousSceneIndex);
        }
        else
        {
            Debug.Log("This is the first scene.");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator FadeAndSwitchScenes(string sceneName)
    {
        FadeTransition.instance.FadeToBlack();
        yield return new WaitForSeconds(FadeTransition.instance.fadeDuration); // Esperar a que la imagen se desvanezca completamente
        SceneManager.LoadScene(sceneName);
        if (sceneName == "Main Menu")
        {
            level = 0;
            mainCharacter = GameObject.FindObjectOfType<MainCharacter>();
            if (mainCharacter != null)
            {
                Destroy(mainCharacter.gameObject);
            }
            GameObject musicObject = GameObject.Find(musicObjectName);
            if (musicObject != null)
            {
                Destroy(musicObject);
            }
            GameObject targetObject = GameObject.Find(playerTarget);
            if (targetObject != null)
            {
                Destroy(targetObject);
            }
        }
        FadeTransition.instance.FadeFromBlack();
    }
}