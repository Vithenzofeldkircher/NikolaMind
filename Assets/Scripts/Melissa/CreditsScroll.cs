using UnityEngine;

public class CreditsScroll : MonoBehaviour
{
    public GameObject creditsPanel;
    public Animator creditsAnimator;

    public void AbrirCreditos()
    {
        creditsPanel.SetActive(true);

        creditsAnimator.Play("CreditsAnimation", 0, 0f);
    }

    public void FecharCreditos()
    {
        creditsPanel.SetActive(false);
    }
}