using UnityEngine;
using System.Collections.Generic;

public class Gerenciador_Puzzle : MonoBehaviour
{
    [Header("Configurações")]
    public string[] nomesDasCores = { "Vermelho", "Verde", "Azul", "Amarelo" };

    [Header("Componentes da Cena")]
    public PostIt[] scriptsPostIts; // Arraste os scripts dos Post-its aqui

    private List<int> sequenciaCorreta = new List<int>();
    private List<int> tentativaJogador = new List<int>();

    void Start()
    {
        IniciarNovoPuzzle();
    }

    public void IniciarNovoPuzzle()
    {
        sequenciaCorreta.Clear();
        tentativaJogador.Clear();

        for (int i = 0; i < 4; i++)
        {
            int corSorteada = Random.Range(0, nomesDasCores.Length);
            sequenciaCorreta.Add(corSorteada);

            // Manda o script do Post-it mostrar a letra
            scriptsPostIts[i].DefinirLetra(nomesDasCores[corSorteada]);
        }
    }

    public void RegistrarClique(int idCorFio)
    {
        tentativaJogador.Add(idCorFio);
        int indexAtual = tentativaJogador.Count - 1;

        if (tentativaJogador[indexAtual] == sequenciaCorreta[indexAtual])
        {
            Debug.Log("<color=green>Acertou o fio!</color>");
            if (tentativaJogador.Count == 4) ResolverPuzzle();
        }
        else
        {
            Debug.Log("<color=red>Errou! Reiniciando tentativa...</color>");
            tentativaJogador.Clear();
        }
    }

    void ResolverPuzzle()
    {
        Debug.Log("LIGAÇÃO DIRETA FEITA!");
        // Coloque aqui o que acontece quando vence
    }
}