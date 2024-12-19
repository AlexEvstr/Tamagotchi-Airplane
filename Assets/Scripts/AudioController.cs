using System.Collections;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioClip _click;
    [SerializeField] private AudioClip _tap;
    [SerializeField] private AudioClip _win;
    [SerializeField] private AudioClip _lose;
    [SerializeField] private AudioClip _error;
    private AudioSource _audioSource;
    [SerializeField] private GameObject _gameBgAudio;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_gameBgAudio != null) StartCoroutine(StartBgMusic());
    }

    private IEnumerator StartBgMusic()
    {
        yield return new WaitForSeconds(2.0f);
        _gameBgAudio.SetActive(true);
    }

    public void SoundClick()
    {
        _audioSource.PlayOneShot(_click);
    }

    public void SoundTap()
    {
        _audioSource.PlayOneShot(_tap);
    }

    public void SoundWin()
    {
        _gameBgAudio.GetComponent<AudioSource>().Stop();
        _audioSource.PlayOneShot(_win);
    }

    public void SoundLose()
    {
        _gameBgAudio.GetComponent<AudioSource>().Stop();
        _audioSource.PlayOneShot(_lose);
    }

    public void SoundError()
    {
        _audioSource.PlayOneShot(_error);
    }
}