using UnityEngine;

public class Portal : MonoBehaviour
{
    private PortalPool _portalPool;
    private Vector3 _origSize;

    public void Init(Vector3 position, PortalPool portalPool)
    {
        _origSize = transform.localScale;
        transform.position = position;
        _portalPool = portalPool;
    }

    public void Release()
    {
        _portalPool.Release(this);
    }

    public void SetOrigSize()
    {
        transform.localScale = _origSize;
    }
}
