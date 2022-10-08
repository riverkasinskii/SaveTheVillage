using UnityEngine;

public class Audio : MonoBehaviour
{
    [SerializeField] private AudioSource _sound;   
    public AudioSource Sound
    {
        get { return _sound; }        
    }
    private void Start()
    {        
        _sound.Stop();
    }

    public void OnClickAudioButton()
    {        
        _sound.Play();
    }

}
