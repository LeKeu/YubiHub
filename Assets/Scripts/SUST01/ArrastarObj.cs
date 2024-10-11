using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ArrastarObj : MonoBehaviour
{
    

    Vector3 offset;
    Collider2D collider2d;

    [SerializeField] List<Image> coracoes;

    int vidas;
    public static bool chamarFunc;

    SFX_scripts sFX_Scripts;

    void Awake()
    {
        chamarFunc= false;
        collider2d = GetComponent<Collider2D>();
        vidas = 2;
        sFX_Scripts = GameObject.Find("Geral").GetComponent<SFX_scripts>();
    }

    

    void OnMouseDown()
    {
        offset = transform.position - MouseWorldPosition();
    }

    void OnMouseDrag()
    {
        transform.position = MouseWorldPosition() + offset;
    }

    void OnMouseUp()
    {
        Debug.Log("oiii");
        collider2d.enabled = false;
        var rayOrigin = Camera.main.transform.position;
        var rayDirection = MouseWorldPosition() - Camera.main.transform.position;
        RaycastHit2D hitInfo;
        if (hitInfo = Physics2D.Raycast(rayOrigin, rayDirection))
        {
            Debug.Log($"hit {hitInfo.transform.name}, lixo {gameObject.name}");
            if (hitInfo.transform.name == gameObject.name)
            {
                sFX_Scripts.SoundAcertar();
                chamarFunc = true;
                Debug.Log("acertou");
            }
            else
            {
                sFX_Scripts.SoundErrar();
                PerderVida();
            }
        }
        collider2d.enabled = true;
    }

    public void PerderVida() { coracoes[vidas].enabled = false; vidas--; if (vidas < 0) { StartCoroutine("Morrer"); } }
    IEnumerator Morrer()
    {
        //string aux = $"{InfoJogador.nomeJogador}_{nomeMat02}";
        //string aux_pontos = $"{InfoJogador.nomeJogador}_{nomeMat02}_pontos";

        //Debug.Log(aux_pontos);
        //if (PlayerPrefs.GetString(aux_pontos) == "_")
        //    PlayerPrefs.SetString(aux_pontos, "_");

        //Debug.Log(">" + PlayerPrefs.GetString(aux_pontos));
        //PlayerPrefs.SetString(aux_pontos, PlayerPrefs.GetString(aux_pontos) + "_" + pontos.ToString());
        //Debug.Log(">" + PlayerPrefs.GetString(aux_pontos));

        //if (PlayerPrefs.GetInt(aux) < pontos)
        //    PlayerPrefs.SetInt(aux, pontos);
        yield return new WaitForSeconds(2); SceneManager.LoadScene("MenuJogos");
    }

    Vector3 MouseWorldPosition()
    {
        var mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }
}
