using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEditor.VersionControl;

public class BotoesGerais : MonoBehaviour
{
    string SENHA;

    public static string nomeJogoGrafico;
    public static string nomeJogadorGrafico;

    [SerializeField] GameObject PainelSenha;
    [SerializeField] GameObject PainelNormal;

    GameObject PainelApagar;
    Window_Graph windowGraph;

    public class ConfigData
    {
        public string Senha;
    }

    private void Start()
    {
        if(SceneManager.GetActiveScene().name == "MenuInicial")
            ResgatarSenha();


        if (SceneManager.GetActiveScene().name == "ProfasMain")
            windowGraph = GameObject.FindGameObjectWithTag("WindowGraph").GetComponent<Window_Graph>();
        if (SceneManager.GetActiveScene().name == "MenuJogos")
        { GameObject.Find("nomeJogador").GetComponent<TextMeshProUGUI>().text = $"Ol�, {InfoJogador.nomeJogador}!"; }
    }

    void ResgatarSenha()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "config.json");

        if (File.Exists(path))
        {
            string jsonContent = File.ReadAllText(path);
            ConfigData configSenha = JsonUtility.FromJson<ConfigData>(jsonContent);
            SENHA = configSenha.Senha.Trim();
            Debug.Log("SENHA = "+SENHA);
        }
        else
        {
            Debug.LogError("Arquivo de configura��o n�o encontrado.");
        }
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
    public void Cadastro2() { SceneManager.LoadScene("Cadastro2"); }
    public void MenuInicial() { SceneManager.LoadScene("MenuInicial"); }

    public void Mat02() { SceneManager.LoadScene("Mat02"); }
    public void Mat04() { SceneManager.LoadScene("Mat04"); }
    public void MatMenu03() { SceneManager.LoadScene("MatMenu03"); }
    public void Profs() { SceneManager.LoadScene("ProfasMain"); }
    public void Sust01() { SceneManager.LoadScene("Sust01"); }
    public void ProfsFechar() { PainelSenha.SetActive(false); }

    public void ChecarSenha(TMP_InputField senha)
    {
        if (senha.text.Trim() == SENHA) { SceneManager.LoadScene("ProfasMain"); }
    }

    public void VisualizarSenha(TMP_InputField senhaInput)
    {
        if (senhaInput.contentType == TMP_InputField.ContentType.Password)
            senhaInput.contentType = TMP_InputField.ContentType.Standard;
        else
            senhaInput.contentType = TMP_InputField.ContentType.Password;

        senhaInput.ForceLabelUpdate();
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

    public void VoltarApagar2()
    {
        InfoJogador.nomeApagar = "";
        SceneManager.LoadScene("Cadastro2");
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

    public void ApagarPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }
}
