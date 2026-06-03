using UnityEngine;

public class Caixa_Enrolar : MonoBehaviour
{
    [Header("Configurações do sprite")]
    [SerializeField] private Sprite sprite_Normal;
    [SerializeField] private Sprite sprite_Mudado;

    private SpriteRenderer SP;
    private bool ja_tem_fio = false;

    void Awake()
    {
        SP = GetComponent<SpriteRenderer>();
        if (sprite_Normal != null && SP != null)
            SP.sprite = sprite_Normal;

    }

    public void Enrolar_Fio()
    {
        if(ja_tem_fio ) return;

        ja_tem_fio = true;
        if (sprite_Mudado != null) SP.sprite = sprite_Mudado;
        print("Fio enrolado na caixa {gameObject.name}");

    }

    public void DesenrolarFio()
    {
        if (!ja_tem_fio) return;

        ja_tem_fio = false;
        if (sprite_Normal != null) SP.sprite = sprite_Normal;
        Debug.Log($"Fio desenrolado da caixa: {gameObject.name}");
    }
}


