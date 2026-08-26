using UnityEngine;

public class TocaE : MonoBehaviour
{
    public AudioSource audioSource;

    void Update()
  {
       if (Input.GetKeyDown(KeyCode.E)) 
        {
            audioSource.Play();
        }
  }
}