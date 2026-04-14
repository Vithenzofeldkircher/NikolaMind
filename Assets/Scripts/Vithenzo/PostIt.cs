using UnityEngine;
using TMPro;

public class PostIt : MonoBehaviour
{
    private TextMeshProUGUI texto;

    void Awake()
    {
        texto = GetComponent<TextMeshProUGUI>();
    }

    public void DefinirLetra(string nomeDaCor)
    {
        // Pega a primeira letra e exibe
        texto.text = nomeDaCor[0].ToString().ToUpper();
    }
}