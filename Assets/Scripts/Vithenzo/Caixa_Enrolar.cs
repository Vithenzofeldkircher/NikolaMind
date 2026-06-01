using UnityEngine;

public class Caixa_Enrolar : MonoBehaviour
{
    [Header("Configurações do sprite")]
    [SerializeField] private Sprite sprite_Normal;
    [SerializeField] private Sprite sprite_Mudado;

    private SpriteRenderer Sprite_Render;
    private bool ja_tem_fio = false;

    void awake()
    {
        Sprite_Render = GetComponent<SpriteRenderer>();
        if(sprite_Normal != null ) Sprite_Render.sprite = sprite_Normal;
    }

    public void Enrolar_Fio()
    {
        if(ja_tem_fio ) return;

        ja_tem_fio = true;
        if(sprite_Mudado != null)
        {
            //spriteRenderer.sprite = sprite_Mudado;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
