using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicSlot : MonoBehaviour
{
    MusicController musicController;

    Text songName;

    private AudioClip song;
    private Slider progress;
    private Image buttonIcon;

    Sprite playIcon, stopIcon, pauseIcon;

    int songID;
    bool active;

    public AudioClip Song { get => song; }
    public int SongID { get => songID; }

    public delegate void onEndEvent();
    public static event onEndEvent OnEnd;
    public static event onEndEvent OnPlay;

    void OnEnable()
    {
        musicController = MusicController.instance;

        playIcon = Resources.Load<Sprite>("Graphics/Sprites/Button/Play");
        stopIcon = Resources.Load<Sprite>("Graphics/Sprites/Button/Stop");
        pauseIcon = Resources.Load<Sprite>("Graphics/Sprites/Button/Pause");

        buttonIcon = transform.Find("ButtonIcon").GetComponent<Image>();
        buttonIcon.sprite = playIcon;

        progress = transform.Find("ProgressBar").GetComponent<Slider>();
        progress.value = 0;

        songName = transform.Find("Name").GetComponent<Text>();

        active = false;
        OnPlay += Stop;
        //OnEnd += Stop;
    }

    public void Update()
    {
        if (!active)
            return;

        progress.value += Time.deltaTime;
        if (progress.value >= progress.maxValue)
            OnEnd.Invoke();
    }

    private void OnDisable()
    {
        OnPlay -= Stop;
    }

    public void Init(AudioClip songClip, int ID)
    {
        song = songClip;
        songID = ID;
        songName.text = songClip.name;
        progress.maxValue = songClip.length;
    }

    public void Play()
    {
        OnPlay.Invoke();
        musicController.Play(this);

        active = true;
        buttonIcon.sprite = stopIcon;
    }

    public void Stop()
    {
        if (!active)
            return;

        active = false;
        progress.value = 0;
        buttonIcon.sprite = playIcon;

        musicController.Stop();
    }

    // [nonsense] should add a button, not the point
    void Pause()
    {
        active = false;
        buttonIcon.sprite = pauseIcon;

        musicController.Stop();
    }

    public void Select()
    {
        if (active)
            Stop();
        else
            Play();
    }

    public void ContinuePlaying()
    {
        buttonIcon.sprite = stopIcon;
    }
}
