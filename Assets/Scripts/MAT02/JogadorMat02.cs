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
        string aux = $"{InfoJogador.nomeJogador}_{nomeMat02}";
        string aux_pontos = $"{InfoJogador.nomeJogador}_{nomeMat02}_pontos";

        PlayerPrefs.SetString(aux_pontos, PlayerPrefs.GetString(aux_pontos) + "_" + pontos.ToString());

        if (PlayerPrefs.GetInt(aux) < pontos)
            PlayerPrefs.SetInt(aux, pontos);
        yield return new WaitForSeconds(2); SceneManager.LoadScene("MenuJogos");
    }

    public void GanharPontoMat01() { pontos++; }
}
