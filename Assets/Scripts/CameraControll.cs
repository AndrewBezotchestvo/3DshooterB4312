using UnityEngine;

public class CameraControll : MonoBehaviour
{
    private float rotation = 0;
    [SerializeField] private float verical_sens = 0.5f;
    void Update()
    {
        rotation -= Input.GetAxis("Mouse Y") * verical_sens;
        rotation = Mathf.Clamp(rotation, -45, 45);

        transform.localEulerAngles = new Vector3(rotation, 0, 0);
    }
}
