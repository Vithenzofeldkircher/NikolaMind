using UnityEngine;
public class PlayerAnimation : MonoBehaviour
{
   public SpriteRenderer spriteRenderer;
   public Sprite[] frente;
   public Sprite[] costas;
   public Sprite[] esquerda;
   public Sprite[] direita;
   public float frameRate = 0.15f;
   private float timer;
   private int frameIndex;
   private Sprite[] currentAnimation;
   void Update()
   {
       Vector2 move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
       // Escolhe direção
       if (move.y > 0)
           currentAnimation = costas;
       else if (move.y < 0)
           currentAnimation = frente;
       else if (move.x > 0)
           currentAnimation = direita;
       else if (move.x < 0)
           currentAnimation = esquerda;
       // Se não estiver se movendo, trava no primeiro frame
       if (move == Vector2.zero)
       {
           frameIndex = 0;
           if (currentAnimation != null && currentAnimation.Length > 0)
               spriteRenderer.sprite = currentAnimation[0];
           return;
       }
       // Animação
       timer += Time.deltaTime;
       if (timer >= frameRate)
       {
           timer = 0f;
           frameIndex++;
           if (frameIndex >= currentAnimation.Length)
               frameIndex = 0;
           spriteRenderer.sprite = currentAnimation[frameIndex];
       }
   }
}