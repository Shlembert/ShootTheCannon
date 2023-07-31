using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HealsBar : MonoBehaviour
{
    [SerializeField] private Image hpBar;
    [SerializeField] private GameObject hpBarObj;

    private Transform _transform;
    private Transform _cameraTransform;

    private void Start()
    {
        _cameraTransform = Camera.main.transform;
        _transform = transform;
    }

    private void Update()
    {
        _transform.forward = _transform.position - _cameraTransform.position;
    }

    public void ChangeHP(int maxHP, int currentHP)
    {
        hpBar.DOFillAmount(currentHP / (float)maxHP, 0.5f);
    }

    public void ShowHPBar(bool value)
    {
        hpBarObj.SetActive(value);
    }
}
