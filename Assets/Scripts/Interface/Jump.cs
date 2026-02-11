using UnityEngine;

public interface Jump
{
    public bool Can_Jump();
    public void Jumps();
    public void OnCollisionEnter2D(Collision2D collision);
}
