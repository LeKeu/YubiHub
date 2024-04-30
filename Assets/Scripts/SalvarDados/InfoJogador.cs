﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Linq;
using Unity.VisualScripting;
using System;

public class InfoJogador : MonoBehaviour
{
    [SerializeField] Button botao;
    [SerializeField] Button botaoJogos;

    public static string nomeJogador;
    public static string nomeApagar;
    public static string nomeGrafico;
    
    public static List<string> DadosJogador;
    public static List<string> JogadorScores;
    public static List<string> NomesJogos;

    private void Awake()
    {
        nomeGrafico = PlayerPrefs.GetString("Jogadores").Split(';')[0];
        //nomeGrafico = "sees";
        DadosJogador = new List<string>()
        {
            $"{nomeApagar}_Mat01_0", $"{nomeApagar}_Mat01_1", $"{nomeApagar}_Mat01_2",
            $"{nomeApagar}_Mat02",
            $"{nomeApagar}_Mat03_0", $"{nomeApagar}_Mat03_1", $"{nomeApagar}_Mat03_2"
        };

        JogadorScores = new List<string>()
        {
            $"{nomeGrafico}_Mat01_0_pontos", $"{nomeGrafico}_Mat01_1_pontos", $"{nomeGrafico}_Mat01_2_pontos",
            $"{nomeGrafico}_Mat02_pontos",
            $"{nomeGrafico}_Mat03_0_pontos", $"{nomeGrafico}_Mat03_1_pontos", $"{nomeGrafico}_Mat03_2_pontos"
        };

        NomesJogos = new List<string>()
        {
            $"Mat01_0", $"Mat01_1", $"Mat01_2",
            $"Mat02",
            $"Mat03_0", $"Mat03_1", $"Mat03_2"
        };

        BotoesGerais.nomeJogoGrafico = NomesJogos[0];
        BotoesGerais.nomeJogadorGrafico = nomeGrafico;
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "MenuInicial" || SceneManager.GetActiveScene().name == "Deletar")
            CriarBotoesNomesJogadores();

        if(SceneManager.GetActiveScene().name == "ProfasMain") 
        { CriarBotoesNomesJogadores(); CriarBotoesNomesJogoes(); }
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

    public static List<int> RetornarPontos(string parametro)
    {
        GameObject.Find("aviso").GetComponent<TextMeshProUGUI>().text = "";
        parametro += "_pontos";
        List<string> pontos_aux = new();
        List<int> pontos = new();

        string pontosPlayerPrefs = PlayerPrefs.GetString(parametro);

        pontos_aux.AddRange(pontosPlayerPrefs.Split('_'));
        pontos_aux.RemoveAt(0);

        for (int i = 0; i < pontos_aux.Count; i++)
        { pontos.Add(int.Parse(pontos_aux[i])); }
        

        return pontos;
    }

    
    public void CriarBotoesNomesJogadores()
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

    public void CriarBotoesNomesJogoes()
    {
        GameObject painelScroll = GameObject.FindGameObjectWithTag("PainelBotoesJogos");

        foreach (var aluno in NomesJogos)
        {
            var novoBut = Instantiate(
                botaoJogos,
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
    
}
