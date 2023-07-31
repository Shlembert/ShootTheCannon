using UnityEngine;

public class BonusDrop : MonoBehaviour
{
    public static BonusDrop instance;

    [SerializeField] private Transform bomb, clock;
    [SerializeField] private int chance;
    [SerializeField] private float offsetY = 1.5f;

    private void Start()
    {
        instance = this;
    }

    public void RandomDrop(Vector3 position)
    {
        if(Random.Range(0,100) < chance)
        {
            if (Random.Range(0, 2) == 0)
            {
                if (bomb.gameObject.activeSelf) return;

                bomb.position = position + Vector3.up * offsetY;
                bomb.gameObject.SetActive(true);
            }
            else
            {
                if (clock.gameObject.activeSelf) return;

                clock.position = position + Vector3.up * offsetY;
                clock.gameObject.SetActive(true);
            }
        }
    }
}
