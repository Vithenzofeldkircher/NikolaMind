using UnityEngine;
using UnityEngine.SceneManagement;

public class MudarCena : MonoBehaviour
{

    public void Mini_Game_1()
    {
        SceneManager.LoadScene("Mini_Game_1");
    }

    public void Simple_Scene()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Title()
    {
        SceneManager.LoadScene("Tela Inicial");
    }

}