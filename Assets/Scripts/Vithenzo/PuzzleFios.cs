using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class PuzzleFios : MonoBehaviour
{
    [Header("Configurações")]
    public string[] nomesCores = { "Vermelho", "Verde", "Azul", "Amarelo" };
    public Color[] coresSprites; // Atribua as cores correspondentes no Inspector

    [Header("Referências UI")]
    public TextMeshProUGUI[] postIts; // Arraste os 4 objetos de texto aqui
    public GameObject[] botoesFios;   // Os objetos dos fios na cena

    private List<int> sequenciaCorreta = new List<int>();
    private List<int> tentativaJogador = new List<int>();

    void Start()
    {
        GerarSequencia();
    }

    void GerarSequencia()
    {
        sequenciaCorreta.Clear();
        tentativaJogador.Clear();

        // 1. Gera 4 índices aleatórios (0 a 3)
        for (int i = 0; i < 4; i++)
        {
            int indiceSorteado = Random.Range(0, nomesCores.Length);
            sequenciaCorreta.Add(indiceSorteado);

            // 2. Atualiza o Post-it com a inicial da cor
            postIts[i].text = nomesCores[indiceSorteado][0].ToString().ToUpper();
        }

        Debug.Log("Sequência Gerada: " + string.Join(", ", sequenciaCorreta));
    }

    // Chamado pelo clique no fio
    public void ClicarNoFio(int indiceFio)
    {
        tentativaJogador.Add(indiceFio);
        int passoAtual = tentativaJogador.Count - 1;

        // Verifica se o fio clicado é o correto da sequência
        if (tentativaJogador[passoAtual] == sequenciaCorreta[passoAtual])
        {
            Debug.Log("Fio correto!");

            if (tentativaJogador.Count == sequenciaCorreta.Count)
            {
                Debug.Log("Ligação Direta Concluída!");
                FinalizarPuzzle();
            }
        }
        else
        {
            Debug.Log("Sequência errada! Reiniciando...");
            tentativaJogador.Clear();
            // Aqui você pode adicionar um efeito visual de erro ou som
        }
    }

    void FinalizarPuzzle()
    {
        // Lógica de vitória: abrir porta, ligar luz, etc.
    }
}