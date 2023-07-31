using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] LayerMask layerMask;
    private Vector3 _target;

    private void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, float.MaxValue, layerMask))
        {
            _target = hit.point;
        }
    }

    public Vector3 GetTargetPosition()
    {
        return _target;
    }
}
