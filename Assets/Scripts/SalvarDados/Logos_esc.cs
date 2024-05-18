using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Logos_esc : MonoBehaviour
{
    [SerializeField] Button butsLogo;
    public static Sprite[] Logos;
    static Sprite perfilInicial;
    public static string nomeLogo;

    public static bool escolheu;

    static List<Button> logosList = new List<Button>();

    private void Awake()
    {
        escolheu = false;
        Logos = Resources.LoadAll<Sprite>("UI/Logos");
        
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Cadastro")
        {
            CriarBotoesLogos();
            AddListeners();
            perfilInicial = GameObject.Find("perfil").GetComponent<Image>().sprite;
        }
    }

    public static void ResetarLogo()
    {
        GameObject.Find("perfil").GetComponent<Image>().sprite = perfilInicial;
    }

    public void CriarBotoesLogos()
    {
        GameObject painelScroll = GameObject.Find("PainelBotoesLogos");
        Debug.Log(Logos.Length);
        for(int i = 0; i < Logos.Length; i++)
        {
            var novoBut = Instantiate(
                butsLogo,
                painelScroll.transform.position,
                Quaternion.identity);
            novoBut.transform.SetParent(painelScroll.transform);

            novoBut.name = Logos[i].name;
            novoBut.GetComponent<Image>().sprite = Logos[i];

            logosList.Add(novoBut);
        }
    }

    void AddListeners()
    {
        foreach (Button button in logosList)
        {
            button.onClick.AddListener(() => SelecionarNome());
        }
    }

    public static void SalvarLogo(string nomeJogador)
    {
        PlayerPrefs.SetInt($"{nomeJogador}_logo", int.Parse(nomeLogo)); // ex: leticia_logo = 2
        escolheu = false;
        ResetarLogo();
    }

    void SelecionarNome()
    {
        escolheu = true;
        nomeLogo = EventSystem.current.currentSelectedGameObject.name;
        Debug.Log(nomeLogo);
        GameObject.Find("perfil").GetComponent<Image>().sprite = Logos[int.Parse(nomeLogo)];
        //encontro a imagem de perfil e substituo com icone selecionado (pelo nome int)

    }
}
