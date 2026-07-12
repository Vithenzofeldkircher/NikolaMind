using UnityEngine;
public interface IInteragivelFio
{
    void AoTocarFio();
    void AoSoltarFio();
    bool EstaComFio { get; } // Nova propriedade para o SOLID
}