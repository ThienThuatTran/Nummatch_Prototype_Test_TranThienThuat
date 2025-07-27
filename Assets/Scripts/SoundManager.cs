using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    private AudioSource audioSource;

    [SerializeField] private AudioClip chooseSoundFX;
    [SerializeField] private AudioClip rowClearSoundFX;
    [SerializeField] private AudioClip pairClearSoundFX;
    [SerializeField] private AudioClip buttonClickSoundFX;

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayChooseSoundFX()
    {
        audioSource.PlayOneShot(chooseSoundFX);
    }

    public void PlayRowClearSoundFX()
    {
        audioSource.PlayOneShot(rowClearSoundFX);
    }

    public void PlayPairClearSoundFX()
    {
        audioSource.PlayOneShot(pairClearSoundFX);
    }

    public void PlayButtonClickSoundFX()
    {
        audioSource.PlayOneShot(buttonClickSoundFX);
    }
}
