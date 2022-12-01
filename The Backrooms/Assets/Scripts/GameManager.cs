using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PostProcessVolume volume;

    private bool isVolumeActive;

    public AudioSource musicSource;

    public Transform spawnPos;

    public Toggle postProcessToggle;

    public PlayerMovement playerMovement;

    public GameObject player;

    public Transform startPos;

    private void Update()
    {
        if(postProcessToggle.isOn)
        {
            volume.enabled = true;
            isVolumeActive = true;
        }
        else
        {
            volume.enabled = false;
            isVolumeActive = false;
        }
    }

    public void Respawn()
    {
        player.transform.position = spawnPos.position;
    }

    public void Restart()
    {
        playerMovement.foundEasterEggs = 0;
        player.transform.position = startPos.position;
    }

    public void AdjustSound(float _volume)
    {
        musicSource.volume = _volume;
    }
}
