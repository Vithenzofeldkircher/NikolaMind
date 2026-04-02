using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    [SerializeField]
    private Transform _Target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    private void LateUpdate()
    {
        transform.position = new Vector3(_Target.position.x, _Target.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
