using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Mat04Main : MonoBehaviour
{
    [SerializeField] TMP_Text textJ1;
    [SerializeField] TMP_Text textJ2;
    [SerializeField] TMP_Text textPartidas;
    int partidas; int totalPartidas;

    [SerializeField] List<Sprite> botoesSprites;
    [SerializeField] List<Button> p1But;
    [SerializeField] List<Button> p2But;

    [SerializeField] List<GameObject> lugaresN1;
    [SerializeField] List<GameObject> lugaresN2;

    [SerializeField] List<Sprite> operacoes;
    [SerializeField] GameObject operacaoImg;

    //[SerializeField] Sprite[] Bolas;
    //[SerializeField] List<Sprite> bolasPrefab;
    [SerializeField] GameObject bolaObj;
    [SerializeField] int maximoBolas = 10;
    Sprite[] Bolas; 

    int n1, n2;

    int jogador1, jogador2;

    bool continuar;

    // Start is called before the first frame update
    void Start()
    {
        partidas = 0; totalPartidas = 10;
        textPartidas.text = $"{partidas+1}/{totalPartidas}";
        Bolas = Resources.LoadAll<Sprite>("Sprites/Bolas");
        StartCoroutine("GerarPergunta");
        jogador1 = 0; jogador2 = 0;
        continuar = true;
    }

    IEnumerator GerarPergunta()
    {
        DesativarBotoes();
        List<int> numeros = GerarNumeros();
        yield return new WaitForSeconds(1.5f);
        DesativarBotoes();
        CorBotaoPadrao();

        n1 = numeros[0]; n2 = numeros[1];
         
        GerarBolas(n1, lugaresN1);
        GerarBolas(n2, lugaresN2);
        //int oper = Random.Range(0, 2); // 0, mais; 1, menos
        int oper = 0;

        // descomentar a linha abaixo quando a operação de subtração, pois alterna entre elas
        //operacaoImg.GetComponent<Image>().sprite = oper == 0 ? operacoes[0] : operacoes[1];
        operacaoImg.GetComponent<Image>().sprite = operacoes[0];

        int resposta = oper == 0 ? n1 + n2 : n1 - n2;

        RespostaBotoes(resposta, p1But);
        RespostaBotoes(resposta, p2But, "2");
    }
    void DesativarBotoes()
    {
        foreach(Button but in p1But) { but.enabled = !but.IsActive(); }
        foreach(Button but in p2But) { but.enabled = !but.IsActive(); }
    }

    List<int> GerarNumeros()
    {
        n1 = Random.Range(1, maximoBolas + 1); 
        n2 = Random.Range(1, maximoBolas + 1);
        // descomentar linha abaixo para caso a operação de subtração volte (assim, não tera números negativos no resultado)
        //while (n2 > n1) { n2 = Random.Range(1, maximoBolas + 1); } 
        return new List<int> { n1, n2 };
    }

    private void RespostaBotoes(int numCerto, List<Button> botoes, string auxButTag = "")
    {
        List<Button> botoesNovos1 = botoes.OrderBy(x => Random.value).ToList();
        for (int i = 0; i < botoesNovos1.Count - 1; i++)
        {
            botoesNovos1[i].GetComponentInChildren<TextMeshProUGUI>().text = NumRedor(numCerto, i).ToString();
            botoesNovos1[i].gameObject.tag = "Errada";
        }
        botoesNovos1[botoesNovos1.Count - 1].GetComponentInChildren<TextMeshProUGUI>().text = numCerto.ToString();
        botoesNovos1[botoesNovos1.Count - 1].gameObject.tag = $"Certa{auxButTag}";
    }

    int aux = 0;
    private int NumRedor(int num, int i)
    {
        int numNovo = Random.Range(1, 10);
        if (num != 1)
        {
            while (numNovo == num)
            {
                numNovo = Random.Range(1, num);
            }
        }

        if (i == 0) 
        { aux = numNovo; }

        if (i == 1 && numNovo == aux) 
        { numNovo = numNovo + 1 == num ? numNovo + 2 : numNovo + 1; Debug.Log("inguaiss"); }

        return numNovo;
    }

    void GerarBolas(int qntd, List<GameObject> posicoes)
    {
        for (int i = 0; i < qntd; i++)
        {
            var bola = Instantiate(bolaObj, posicoes[i].gameObject.transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));
            bola.transform.SetParent(posicoes[i].transform);

            bola.GetComponent<Image>().sprite = Bolas[Random.Range(0, Bolas.Length)];
            bola.tag = "Bolas";
        }
    }

    void ApagarBolas()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Bolas");
        foreach (GameObject go in gos)
            Destroy(go);
    }

    public void ChecarResposta()
    {
        if (EventSystem.current.currentSelectedGameObject.tag == "Certa")   // se for a certa do jogador 1
        { 
            jogador1++; CorBotaoCertoErrado(p1: true); 
            textJ1.text = $"Jogador 1: {jogador1}"; 
        }
        else if(EventSystem.current.currentSelectedGameObject.tag == "Certa2") 
        { 
            jogador2++; 
            CorBotaoCertoErrado(p2: true); 
            textJ2.text = $"Jogador 2: {jogador2}"; 
        }
        else 
        { 
            CorBotaoCertoErrado(); 
        }

        if (ChecarFinalPartida()) 
            return; 

        if (continuar) 
        { ApagarBolas(); StartCoroutine("GerarPergunta"); }
    }

    bool ChecarFinalPartida()
    {
        string empate = "Empate!"; string j1 = "Jogador 1 venceu!"; string j2 = "Jogador 2 venceu!";
        partidas++;
        textPartidas.text = $"{partidas+1}/{totalPartidas}";
        if (partidas == totalPartidas)
        {
            GameObject.Find("Final").GetComponent<TextMeshProUGUI>().text = $"{(jogador1 == jogador2? empate : jogador1 > jogador2 ? j1 : j2)}";
            continuar = false;
            StartCoroutine("SairJogo");
        } return false;
    }

    private void CorBotaoCertoErrado(bool p1 = false, bool p2 = false) // 0 - verde, 1 - vermelho, 2 - padrao (laranja)
    {
        foreach (Button but in p1But)
        {
            if (but.transform.tag == "Certa" && p1) { but.GetComponent<Image>().sprite = botoesSprites[0]; }
            else { but.GetComponent<Image>().sprite = botoesSprites[1]; }
        }

        foreach (Button but in p2But)
        {
            if (but.transform.tag == "Certa2" && p2) { but.GetComponent<Image>().sprite = botoesSprites[0]; }
            else { but.GetComponent<Image>().sprite = botoesSprites[1]; }
        }
    }

    private void CorBotaoPadrao() // 0 - verde, 1 - vermelho, 2 - padrao (laranja)
    {
        GameObject.Find("op11").GetComponent<Image>().sprite = botoesSprites[2];
        GameObject.Find("op21").GetComponent<Image>().sprite = botoesSprites[2];
        GameObject.Find("op31").GetComponent<Image>().sprite = botoesSprites[2];

        GameObject.Find("op12").GetComponent<Image>().sprite = botoesSprites[2];
        GameObject.Find("op22").GetComponent<Image>().sprite = botoesSprites[2];
        GameObject.Find("op32").GetComponent<Image>().sprite = botoesSprites[2];
    }

    IEnumerator SairJogo()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("MenuJogos");
    }
}
