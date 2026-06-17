using UnityEngine;

// Segregação de Interfaces: Cria um contrato exclusivo para interações, baseadas no cabo, sem misturar com o inventário padrão de itens.
public interface IInteragivelFioDiscreto
{
    // Método disparado quando o jogador interage manualmente com o fio esticado
    void InteragirComFio();
}