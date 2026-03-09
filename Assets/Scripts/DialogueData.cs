using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NovoDialogo", menuName = "Dialogo/Novo Dialogo")]
public class DialogueData : ScriptableObject
{
    [Serializable]
    public class Fala
    {
        public string nomePersonagem;
        [TextArea(2, 5)]
        public string texto;
        public int IdPersonagem;
    }

    public List<Fala> falas = new List<Fala>();

    [Header("Configurações de Quest (Opcional)")]
    public bool ehFimDeQuest;
    public enum Dificuldade { Nenhum, Facil, Media, Dificil }
    public Dificuldade dificuldade;
}