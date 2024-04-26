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
    
    public static List<string> DadosJogador;
    public static List<string> JogadorScores;

    private void Awake()
    {
        DadosJogador = new List<string>()
        {
            $"{nomeApagar}_Mat01_0", $"{nomeApagar}_Mat01_1", $"{nomeApagar}_Mat01_2",
            $"{nomeApagar}_Mat02",
            $"{nomeApagar}_Mat03_0", $"{nomeApagar}_Mat03_1", $"{nomeApagar}_Mat03_2"
        };

        JogadorScores = new List<string>()
        {
            $"{nomeJogador}_Mat01_0_pontos", $"{nomeJogador}_Mat01_1_pontos", $"{nomeJogador}_Mat01_2_pontos",
            $"{nomeJogador}_Mat02_pontos",
            $"{nomeJogador}_Mat03_0_pontos", $"{nomeJogador}_Mat03_1_pontos", $"{nomeJogador}_Mat03_2_pontos"
        };
    }

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
        List<string> nomes = RetornarNomes();

        for(int i = 0; i < nomes.Count; i++)
        {
            if (nomes[i] == nomeApagar) nomes.RemoveAt(i);
        }

        foreach(string dado in DadosJogador) { PlayerPrefs.DeleteKey(dado); }

        AtualizarJogadores(nomes);

        SceneManager.LoadScene("Deletar");
    }

    private List<string> RetornarNomes()
    {
        List<string> nomes = new();
        nomes.AddRange(PlayerPrefs.GetString("Jogadores").Split(';'));
        nomes.RemoveAt(nomes.Count - 1);
        return nomes;
    }

    public static List<string> RetornarPontos(string parametro)
    {
        List<string> pontos = new();
        pontos.AddRange(PlayerPrefs.GetString(parametro).Split('_'));
        pontos.RemoveAt(0);
        return pontos;
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

    public void PrintarPalyerPrefsNomes()
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

    public void PrintarPalyerPrefsScores()
    {
        Debug.Log(JogadorScores.Count);
        foreach(var p in JogadorScores)
        {
            Debug.Log("p--> "+p);
            List<string> scores = RetornarPontos(p);
            Debug.Log("scores-->"+scores);
            foreach (var aluno in scores)
            {
                Debug.Log(aluno);
            }
            Debug.Log("||||||||||||||||||||||||||||||||||||");
        }
    }
}
