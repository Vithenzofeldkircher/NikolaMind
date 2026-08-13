using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInitializer : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene("PauseMenu", LoadSceneMode.Additive);
    }
}