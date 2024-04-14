using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JogadorMat02 : MonoBehaviour
{
    public static int vidas;
    public static int pontos;
    public static string nomeMat02;
    [SerializeField] List<Image> coracoes;

    // Start is called before the first frame update
    void Start()
    {
        nomeMat02 = "Mat02";
        vidas = 2; pontos = 0;
    }

    public void PerderVida() { coracoes[vidas].enabled = false; vidas--; if (vidas < 0) { StartCoroutine("Morrer"); } }
    IEnumerator Morrer()
    {
        if (PlayerPrefs.GetInt($"{InfoJogador.nomeJogador}_{nomeMat02}") < pontos)
            PlayerPrefs.SetInt($"{InfoJogador.nomeJogador}_{nomeMat02}", pontos);
        yield return new WaitForSeconds(2); SceneManager.LoadScene("MenuJogos");
    }

    public void GanharPontoMat01() { pontos++; }
}
