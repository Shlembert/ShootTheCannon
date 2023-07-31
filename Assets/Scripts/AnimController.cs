using UnityEngine;

public class AnimController : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip attackSound;

    public void PlaySoundAttack()
    {
        audioSource.PlayOneShot(attackSound);
    }

    public void Dead()
    {
        enemy.Release();
    }
}
