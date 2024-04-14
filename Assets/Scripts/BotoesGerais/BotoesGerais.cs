using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BotoesGerais : MonoBehaviour
{
    [SerializeField] GameObject PainelSenha;
    [SerializeField] GameObject PainelNormal;
    GameObject PainelApagar;
    

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "MenuJogos")
        { GameObject.FindGameObjectWithTag("TextNomeJogador").GetComponent<TextMeshProUGUI>().text = $"{InfoJogador.nomeJogador} escolha seu jogo:"; }
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
    public void Profs() { SceneManager.LoadScene("ProfasMain"); }
    public void ProfsFechar() { PainelSenha.SetActive(false); }

    public void ChecarSenha(TMP_InputField senha)
    {
        Debug.Log("-" + senha.text + "-");
        if (senha.text == "let") { SceneManager.LoadScene("ProfasMain"); }
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
}
