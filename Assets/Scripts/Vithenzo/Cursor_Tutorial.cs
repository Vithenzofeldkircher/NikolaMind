using UnityEngine;

public class Cursor_Tutorial : MonoBehaviour
{
    public static bool podeTravar = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        if (podeTravar)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

}
