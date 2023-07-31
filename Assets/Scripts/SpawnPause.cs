using UnityEngine;

public class SpawnPause : MonoBehaviour, IBonus
{
    [SerializeField] private ParticleSystem particleTime;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float pauseDelay = 3;

    public void Hit()
    {
        GameManager.Instance.SpawnPause(pauseDelay);
        particleTime.transform.position = transform.position;
        particleTime.Play();
        audioSource.Play();
        gameObject.SetActive(false);
    }
}
