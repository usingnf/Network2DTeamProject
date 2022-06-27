using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    public GameObject soundPrefab = null;
    private void Awake()
    {
        Instance = this;
    }

    public void PlaySound(string sound, Vector3 pos, float Volume = 1.0f, float ThreeD = 1.0f)
    {
        GameObject obj = Instantiate(soundPrefab, pos, Quaternion.identity);
        AudioSource audio = obj.GetComponent<AudioSource>();
        audio.clip = Resources.Load<AudioClip>("Sound/" + sound);
        audio.volume = Volume;

        audio.spatialBlend = ThreeD;
        audio.Play();
        Destroy(obj, 3.0f);
    }
}
