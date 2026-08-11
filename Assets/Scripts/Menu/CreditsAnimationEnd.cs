using UnityEngine;

public class CreditsAnimationEnd : MonoBehaviour
{
    public GameObject painelCreditos;

    public void FecharCreditos()
    {
        painelCreditos.SetActive(false);
    }
}