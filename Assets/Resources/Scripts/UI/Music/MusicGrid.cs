using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicGrid : MonoBehaviour
{
    public GameObject slotPrefab;

    MusicController musicController;

    void Start()
    {
        musicController = MusicController.instance;
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Music");

        int index = 0;
        foreach (AudioClip clip in clips)
        {
            musicController.AddSong(clip.name);
            GameObject newSlot = Instantiate(slotPrefab, transform);
            newSlot.GetComponent<MusicSlot>().Init(clip, index++);
        }
    }

    
}
