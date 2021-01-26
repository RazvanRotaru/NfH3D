using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    public static MusicController instance;
    public List<string> songNames;
    readonly int noSongID = -1;

    private void Awake()
    {
        songNames = new List<string>();
        instance = this;
    }

    AudioSource audioSource;
    GameObject playlist;
    Transform grid;

    int activeSongID = -1;
    
    ScrollRect scrollBar;


    public delegate void OnSongChangeEvent(int songID);
    public event OnSongChangeEvent OnSongChange;

    void Start()
    {
        audioSource = GameObject.FindGameObjectWithTag("Player")
                                        .GetComponent<AudioSource>();
        playlist = transform.Find("Scroll View").gameObject;
        scrollBar = playlist.GetComponent<ScrollRect>();
        grid = playlist.transform.Find("Viewport").Find("Content");

        ShowPlaylist();

        MusicSlot.OnEnd += PlayNextSong;
    }

    public void ShowPlaylist()
    {
        //playlist.SetActive(!playlist.activeSelf && doShow);
        if (playlist.transform.localScale.x == 1)
            playlist.transform.localScale = Vector3.zero;

        else
        {
            playlist.transform.localScale = Vector3.one;
            scrollBar.verticalNormalizedPosition = 1;
        }

        //if (activeSongID >= 0)
        //    grid.transform.GetChild(activeSongID)
        //                    .GetComponent<MusicSlot>().ContinuePlaying();
    }

    void SetLayer(GameObject obj, int layer)
    {
        obj.layer = layer;

        foreach (Transform child in obj.transform)
            SetLayer(child.gameObject, layer);
    }

    public void Play(MusicSlot slot)
    {
        audioSource.Stop();

        audioSource.clip = slot.Song;
        activeSongID = slot.SongID;
        audioSource.Play();

        OnSongChange.Invoke(activeSongID);
    }

    public void Stop()
    {
        audioSource.Stop();
        OnSongChange.Invoke(noSongID);
    }

    void PlayNextSong()
    {
        int songID = ++activeSongID;
        if (songID > songNames.Count)
            return;
        if (activeSongID == songNames.Count)
            songID--;

        MusicSlot nextSong = grid.GetChild(songID)
                                .gameObject.GetComponent<MusicSlot>();
        if (activeSongID == songNames.Count)
            nextSong.Stop();
        else
            nextSong.Play();
    }

    public int GetNextSong()
    {
        return ++activeSongID;
    }

    public void AddSong(string songName)
    {
        songNames.Add(songName);
    }
}
