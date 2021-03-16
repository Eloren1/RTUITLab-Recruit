using UnityEngine;

public class UISoundManager : MonoBehaviour
{
    public static UISoundManager Instance;

    [SerializeField] private AudioSource clickSound;

    private void Start()
    {
        Instance = this;
    }

    public void PlayClickSound()
    {
        clickSound.pitch = Random.Range(0.95f, 1.05f);
        clickSound.Play();
    }
}
