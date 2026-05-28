using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class controlSom : MonoBehaviour
{   private bool estadoSomLigado = true;
    [SerializeField]private AudioSource fundoMusical;

    [SerializeField] private Sprite somLigadoSprite;
    [SerializeField] private Sprite somDesligadoSprite;

    [SerializeField] private Image muteImagem;
    public void LigardesligarSom()
    {
       estadoSomLigado = !estadoSomLigado;
        fundoMusical.enabled = estadoSomLigado;

        if (estadoSomLigado)
        {
            muteImagem.sprite = somLigadoSprite;
        }
        else
        {
            muteImagem.sprite = somDesligadoSprite;
        }
    }
    public void VolumeMusical(float value)
    {
        fundoMusical.volume = value;
    }
}
