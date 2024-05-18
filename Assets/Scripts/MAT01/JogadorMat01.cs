using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JogadorMat01 : MonoBehaviour
{
    public static int vidas;
    public static int pontos;
    public static string nomeMat01;
    [SerializeField] List<Image> coracoes;

    void Start()
    {
        nomeMat01 = "Mat01";
        vidas = 2; pontos = 0;
    }

    public void PerderVida() { coracoes[vidas].enabled = false; vidas--; if (vidas < 0) { StartCoroutine("Morrer"); } }
    IEnumerator Morrer() 
    {
        string aux_pontos = $"{InfoJogador.nomeJogador}_{nomeMat01}_{MatMenu.dificuldade}_pontos";
        Debug.Log(aux_pontos);
        Debug.Log(">" + PlayerPrefs.GetString(aux_pontos));
        PlayerPrefs.SetString(aux_pontos, PlayerPrefs.GetString($"{InfoJogador.nomeJogador}_{nomeMat01}_{MatMenu.dificuldade}_pontos") + "_" + pontos.ToString());
        Debug.Log(">" + PlayerPrefs.GetString(aux_pontos));

        if (PlayerPrefs.GetInt($"{InfoJogador.nomeJogador}_{nomeMat01}_{MatMenu.dificuldade}") < pontos)
            PlayerPrefs.SetInt($"{InfoJogador.nomeJogador}_{nomeMat01}_{MatMenu.dificuldade}", pontos);
        yield return new WaitForSeconds(2); SceneManager.LoadScene("MatMenu01"); }

    public void GanharPontoMat01() { pontos++; }
}
