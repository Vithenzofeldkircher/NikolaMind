using UnityEngine;
using UnityEngine.SceneManagement;

public class MudarCena : MonoBehaviour
{
    public string nomeDaCena;

    public void IrParaCena()
    {
        SceneManager.LoadScene("Mini_Game_1");
    }
}