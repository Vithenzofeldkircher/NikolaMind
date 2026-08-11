using UnityEngine;

[System.Serializable]
public struct ColorCombination
{
    [Header("Resultado da Mistura")]
    public string nomeResultado;   // Ex: "Verde", "Laranja", "Roxo"
    public Color corResultado;     // Cor para o texto/UI

    [Header("Cores dos Componentes")]
    public WireColorData corSuperior; // Ex: Azul (Fusível de cima)
    public WireColorData corInferior; // Ex: Amarelo (Botão de baixo)
}