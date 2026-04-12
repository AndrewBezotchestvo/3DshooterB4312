using UnityEngine;

public class UIController : MonoBehaviour
{
    private bool isPause;

    void Start()
    {
        isPause = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPause) Cursor.lockState = CursorLockMode.Locked;
        else Cursor.lockState = CursorLockMode.None;
        Cursor.visible = isPause;
    }
}
