using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BotoesGerais : MonoBehaviour
{
    public static string nomeJogoGrafico;
    public static string nomeJogadorGrafico;

    [SerializeField] GameObject PainelNormal;
    public void MatMenu() { SceneManager.LoadScene("MatMenu01"); }
    public void Menu(TextMeshProUGUI nome)
    {
        InfoJogador.nomeJogador = nome.text;
        InfoJogador.AtualizarNome(nome.text);
        SceneManager.LoadScene("MenuJogos");
    }

    public void Votar()
    {
        Application.OpenURL("https://www.youtube.com/watch?v=cvh0nX08nRw");
    }

    public void MenuJogos() { SceneManager.LoadScene("MenuJogos"); }

    public void Cadastro() { SceneManager.LoadScene("Cadastro"); }
    public void Cadastro2() { SceneManager.LoadScene("Cadastro2"); }
    public void MenuInicial() { SceneManager.LoadScene("MenuInicial"); }

    public void Mat02() { SceneManager.LoadScene("Mat02"); }
    public void MatMenu03() { SceneManager.LoadScene("MatMenu03"); }

    public void FecharPainel(GameObject painel)
    {
        painel.SetActive(false);
    }

    public void AbrirPainel(GameObject painel)
    {
        painel.SetActive(true);
    }

    public void ChamarApagar()
    {
        SceneManager.LoadScene("Deletar");
    }

    public void ChamarApagarAux()
    {
        InfoJogador.nomeApagar = EventSystem.current.currentSelectedGameObject.name;
        SceneManager.LoadScene("DeletarAux");
    }

    public void SairJogo()
    {
        //PlayerPrefs.DeleteAll();
        Application.Quit();
    }

    public void ApagarPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }
}
