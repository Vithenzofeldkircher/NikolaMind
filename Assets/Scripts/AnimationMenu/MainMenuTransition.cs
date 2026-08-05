using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuTransition : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string nomeDaCena;
    [SerializeField] private float tempoDaAnimacao = 0.7f;

    public void StartGame()
    {
        StartCoroutine(PlayTransition());
    }

    IEnumerator PlayTransition()
    {
        animator.SetTrigger("PlayTransition");

        yield return new WaitForSeconds(tempoDaAnimacao);

        SceneManager.LoadScene(nomeDaCena);
    }
}