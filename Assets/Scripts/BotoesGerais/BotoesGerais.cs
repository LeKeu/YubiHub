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

    [SerializeField] GameObject PainelSenha;
    [SerializeField] GameObject PainelNormal;
    GameObject PainelApagar;
    Window_Graph windowGraph;

    private void Start()
    {
        
        if (SceneManager.GetActiveScene().name == "ProfasMain")
            windowGraph = GameObject.FindGameObjectWithTag("WindowGraph").GetComponent<Window_Graph>();
        //if (SceneManager.GetActiveScene().name == "MenuJogos")
        //{ GameObject.FindGameObjectWithTag("TextNomeJogador").GetComponent<TextMeshProUGUI>().text = $"{InfoJogador.nomeJogador} escolha seu jogo:"; }
    }
    public void MatMenu() { SceneManager.LoadScene("MatMenu01"); }
    public void Menu(TextMeshProUGUI nome)
    {
        InfoJogador.nomeJogador = nome.text;
        InfoJogador.AtualizarNome(nome.text);
        SceneManager.LoadScene("MenuJogos");
    }

    public void MenuJogos() { SceneManager.LoadScene("MenuJogos"); }

    public void Cadastro() { SceneManager.LoadScene("Cadastro"); }
    public void MenuInicial() { SceneManager.LoadScene("MenuInicial"); }

    public void Mat02() { SceneManager.LoadScene("Mat02"); }
    public void MatMenu03() { SceneManager.LoadScene("MatMenu03"); }
    public void Profs() { SceneManager.LoadScene("ProfasMain"); }
    public void ProfsFechar() { PainelSenha.SetActive(false); }

    public void ChecarSenha(TMP_InputField senha)
    {
        if (senha.text == "") { SceneManager.LoadScene("ProfasMain"); }
    }

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

    public void VoltarApagar()
    {
        InfoJogador.nomeApagar = "";
        SceneManager.LoadScene("Deletar");
    }

    public void SelecionarNomeGrafico() // nome do jogo
    {
        nomeJogoGrafico = EventSystem.current.currentSelectedGameObject.name;
        windowGraph.ClearGraph();
        windowGraph.CriarGrafico($"{nomeJogadorGrafico}_{nomeJogoGrafico}");
    }

    public void SelecionarNomeJogador() // nome do jogador
    {
        nomeJogadorGrafico = EventSystem.current.currentSelectedGameObject.name;
        windowGraph.ClearGraph();
        windowGraph.CriarGrafico($"{nomeJogadorGrafico}_{nomeJogoGrafico}");
    }

    public void SairJogo()
    {
        //PlayerPrefs.DeleteAll();
        Application.Quit();
    }
}
