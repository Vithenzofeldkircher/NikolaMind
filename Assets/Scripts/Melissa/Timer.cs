using UnityEngine;

using UnityEngine.SceneManagement;

using TMPro;

public class Timer : MonoBehaviour

{

    public float tempo = 60f;

    public string nomeDaCena = "GameOver";

    public TextMeshProUGUI textoTimer;

    bool acabou = false;

    void Update()

    {

        if (acabou) return;

        tempo -= Time.deltaTime;

        AtualizarTexto();

        if (tempo <= 0)

        {

            acabou = true;

            tempo = 0;

            AtualizarTexto();

            Perdeu();

        }

    }

    void AtualizarTexto()

    {

        textoTimer.text = "Tempo: " + Mathf.Ceil(tempo).ToString();

    }

    void Perdeu()

    {

        ScoreManager.ResetarPontos();
        SceneManager.LoadScene(nomeDaCena);

    }

}