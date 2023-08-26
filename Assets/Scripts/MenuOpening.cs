using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOpening : MonoBehaviour
{
    [SerializeField] AudioClip[] audioClips;

    [SerializeField] float timer = 3;

    bool clipPlayed = false;
    AudioSource source;
    
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0 && !clipPlayed)
        {
            source.clip = audioClips[Random.Range(0,audioClips.Length)];
            source.Play();
            clipPlayed = true;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }
}
