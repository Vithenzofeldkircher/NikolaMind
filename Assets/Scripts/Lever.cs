using UnityEngine;
using UnityEngine.Events;

public class Lever : MonoBehaviour, IInteractable
{

    [SerializeField] private UnityEvent On_Active;
    [SerializeField] private UnityEvent On_Desactive;
    [SerializeField] private bool _Once;
    [SerializeField] private bool _Is_Active;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Active()
    {
        if (_Is_Active)
        {
            On_Desactive.Invoke();
        }
        else
        {
            On_Active.Invoke();
        }
        _Is_Active = !_Is_Active;

        if (_Once)
            Destroy(this);

    }
}
