using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VidPlayer : MonoBehaviour
{
    [SerializeField] VideoClip clip;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("VideoClipPlayer");
    }

    IEnumerator VideoClipPlayer()
    {
        yield return new WaitForSeconds((float)clip.length);
        SceneManager.LoadScene("MenuJogos");
    }
}
