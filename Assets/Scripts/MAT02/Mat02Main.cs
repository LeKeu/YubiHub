using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Mat02Main : MonoBehaviour
{
    [SerializeField] List<GameObject> peixes;
    [SerializeField] List<GameObject> posicoes;

    [SerializeField] List<Button> botoes;
    SFX_scripts sFX_Scripts;

    JogadorMat02 jogador;
    int numeroPeixes;
    // Start is called before the first frame update
    void Start()
    {
        jogador = gameObject.GetComponent<JogadorMat02>();
        sFX_Scripts = gameObject.GetComponent<SFX_scripts>();
        StartCoroutine("CriarPeixes");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CriarPeixes()
    {
        foreach( var botao in botoes )
        {
            botao.enabled = false;
            botao.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
        numeroPeixes = Random.Range(1, 11);
        yield return new WaitForSeconds(2.5f);
        RespostaBotoes(numeroPeixes);
        List<GameObject> posicoesNovas = Misturar(posicoes);

        for (int i = 0; i < numeroPeixes; i++)
        {
            var px = Instantiate(peixes[Random.Range(0, peixes.Count)], posicoesNovas[i].gameObject.transform.position, Quaternion.identity);
            px.transform.parent = posicoesNovas[i].transform;
        }
        foreach (var botao in botoes)
        {
            botao.enabled = true;
        }
    }

    void ApagarPeixes()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Peixes");
        foreach (GameObject go in gos)
            Destroy(go);
    }

    private void RespostaBotoes(int numCerto)
    {
        List<Button> botoesNovos = botoes.OrderBy(x => Random.value).ToList();
        for (int i = 0;i < botoesNovos.Count-1;i++)
        {
            botoesNovos[i].GetComponentInChildren<TextMeshProUGUI>().text = NumRedor(numCerto, i).ToString();
            botoesNovos[i].gameObject.tag = "Errada";
        }
        botoesNovos[botoesNovos.Count - 1].GetComponentInChildren<TextMeshProUGUI>().text = numCerto.ToString();
        botoesNovos[botoesNovos.Count - 1].gameObject.tag = "Certa";
    }

    private int NumRedor(int num, int i)
    {
        int numNovo = Random.Range(1, 10);
        if(num != 1) 
        {
            while (numNovo == num)
            {
                numNovo = Random.Range(1, num);
            }
        }

        //if(i == 1)
        //{
        //    int n1 = int.Parse(GameObject.Find("but01").GetComponent<TextMeshProUGUI>().text);
        //    int n2 = int.Parse(GameObject.Find("but02").GetComponent<TextMeshProUGUI>().text);
        //}
        
        return numNovo;
    }

    private List<GameObject> Misturar(List<GameObject> elementos)
    {
        return elementos.OrderBy(x => Random.value).ToList();
    }

    public void ChecarResposta() 
    {
        if (EventSystem.current.currentSelectedGameObject.tag != "Certa")
        { jogador.PerderVida(); sFX_Scripts.SoundErrar(); }
        else { jogador.GanharPontoMat01(); sFX_Scripts.SoundAcertar(); }

        if (JogadorMat02.vidas >= 0) { ApagarPeixes(); StartCoroutine("CriarPeixes"); }
    }




}
