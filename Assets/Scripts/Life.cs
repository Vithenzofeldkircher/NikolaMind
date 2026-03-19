using UnityEngine;
using UnityEngine.SceneManagement;
public class Life : MonoBehaviour
{
    [Header("Configurações da vida")]
    public float Max_Life = 5f;
    public float Count_Life;
    public float Min_Life = 0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
}
  /*  private void Death()
    {
        Count_Life <= Min_Life;
        Debug.Log("Voce morreu");
        SceneManager.LoadScene("SampleScene");
    }

    // Update is called once per frame
    void Update()
    {
        Death();
    }
}/*
