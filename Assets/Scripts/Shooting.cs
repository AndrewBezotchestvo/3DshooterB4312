using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private Transform _startPointRay;

    [SerializeField] private float _bulletDelay = 0.1f;

    private float _bulletTime = 0;

    private RaycastHit _raycastHit;
    private Ray _ray;
    private Camera _camera;

    private int _countBullets;
    private LineRenderer lineRenderer;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        _camera = GetComponentInParent<Camera>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;
    }

    private void Update()
    {
            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
            }
    }

    private void Shoot()
    {
        audioSource.PlayOneShot(audioSource.clip);
        _ray = new Ray(_camera.transform.position, _camera.transform.forward);

        if (Physics.Raycast(_ray, out _raycastHit))
        {
            GameObject targetObject = _raycastHit.collider.gameObject;
            
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, _startPointRay.position);
            lineRenderer.SetPosition(1, _raycastHit.point);

            if (targetObject.TryGetComponent<EnemyController>(out EnemyController enemyController))
            {
                enemyController.GetDamage(30);
            }
            //lineRenderer.enabled = false;
            Invoke("HideLine", 0.1f);
        }

        Debug.DrawRay(_ray.origin, _ray.direction * 1000);
    }

    private Vector3 GetRandomVector()
    {
        return Vector3.zero;
    }

    private void HideLine()
    {
        lineRenderer.enabled = false;
    }
}
