using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InfoJogador : MonoBehaviour
{
    [SerializeField] Button botao;

    public static string nomeJogador;
    public static string nomeApagar;
    
    private static List<string> DadosJogador;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "MenuInicial" || SceneManager.GetActiveScene().name == "Deletar")
            CriarBotoes();
    }

    public static void AtualizarNome(string nome)
    {
        nomeJogador = nome;
    }

    private void AtualizarJogadores(List<string> nomes)
    {
        string nomesNovos = "";
        foreach (string nome in nomes) { nomesNovos += nome + ";"; }
        PlayerPrefs.SetString("Jogadores", nomesNovos);
    }

    public void AdicionarJogador(TextMeshProUGUI nome)
    {
        string jogadores = PlayerPrefs.GetString("Jogadores");
        PlayerPrefs.SetString("Jogadores", jogadores + nome.text + ";");
    }

    public void DeletarJogador()
    {
        DadosJogador = new List<string>()
        {
            $"{nomeApagar}_Mat01_0", $"{nomeApagar}_Mat01_1", $"{nomeApagar}_Mat01_2",
            $"{nomeApagar}_Mat02"
        };

        List<string> nomes = RetornarNomes();

        for(int i = 0; i < nomes.Count; i++)
        {
            if (nomes[i] == nomeApagar) nomes.RemoveAt(i);
        }

        foreach(string dado in DadosJogador) { PlayerPrefs.DeleteKey(dado); }

        PrintarPalyerPrefs();
        AtualizarJogadores(nomes);
        PrintarPalyerPrefs();

        SceneManager.LoadScene("Deletar");
    }

    private List<string> RetornarNomes()
    {
        List<string> nomes = new();
        nomes.AddRange(PlayerPrefs.GetString("Jogadores").Split(';'));
        nomes.RemoveAt(nomes.Count - 1);
        return nomes;
    }

    public void CriarBotoes()
    {
        GameObject painelScroll = GameObject.FindGameObjectWithTag("PainelBotoes");

        var nomes = RetornarNomes();
        foreach (var aluno in nomes)
        {
            var novoBut = Instantiate(
                botao,
                painelScroll.transform.position,
                Quaternion.identity);
            novoBut.transform.SetParent(painelScroll.transform);

            novoBut.GetComponentInChildren<TextMeshProUGUI>().text = aluno;
            novoBut.name = aluno;
        }
    }

    public void PrintarPalyerPrefs()
    {
        List<string> nomes = RetornarNomes();
        foreach(var aluno in nomes) 
        { 
            Debug.Log(aluno); 
        }
        Debug.Log("||||||||||||||||||||||||||||||||||||");

        foreach(var dado in DadosJogador)
        {
            Debug.Log(dado);
            Debug.Log(PlayerPrefs.GetInt(dado));
            Debug.Log("=============");
        }
    }
}
