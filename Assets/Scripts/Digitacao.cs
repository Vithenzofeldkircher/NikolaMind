using System.Collections;
using TMPro;
using UnityEngine;

public class Digitacao : MonoBehaviour
{
    public TMP_Text texto;
    public AudioSource som;
    public string mensagem;
    public float velocidade = 0.05f;

   void Start()
    {
        StartCoroutine(Digitar());
    }

    IEnumerator Digitar()
    {
     texto.text = "";
   foreach (char letra in mensagem)
     {
           texto.text += letra;
           som.PlayOneShot(som.clip);
            yield return new WaitForSeconds(velocidade);
     }
    }
}