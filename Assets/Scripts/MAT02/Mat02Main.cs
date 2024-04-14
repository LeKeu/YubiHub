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

    JogadorMat02 jogador;
    int numeroPeixes;
    // Start is called before the first frame update
    void Start()
    {
        jogador = gameObject.GetComponent<JogadorMat02>();
        
        StartCoroutine("CriarPeixes");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CriarPeixes()
    {
        numeroPeixes = Random.Range(1, 11);
        yield return new WaitForSeconds(1.5f);
        RespostaBotoes(numeroPeixes);
        Debug.Log(numeroPeixes);
        List<GameObject> posicoesNovas = Misturar(posicoes);

        for (int i = 0; i < numeroPeixes; i++)
        {
            var px = Instantiate(peixes[Random.Range(0, peixes.Count)], posicoesNovas[i].gameObject.transform.position, Quaternion.identity);
            px.transform.parent = posicoesNovas[i].transform;
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
            botoesNovos[i].GetComponentInChildren<TextMeshProUGUI>().text = NumRedor(numCerto).ToString();
            botoesNovos[i].gameObject.tag = "Errada";
        }
        botoesNovos[botoesNovos.Count - 1].GetComponentInChildren<TextMeshProUGUI>().text = numCerto.ToString();
        botoesNovos[botoesNovos.Count - 1].gameObject.tag = "Certa";
    }

    private int NumRedor(int num)
    {
        int numNovo = Random.Range(1, num);
        if(num != 1) 
        {
            Debug.Log(num);
            while (numNovo == num)
            {
                numNovo = Random.Range(1, num);
            }
        }
        
        return numNovo;
    }

    private List<GameObject> Misturar(List<GameObject> elementos)
    {
        return elementos.OrderBy(x => Random.value).ToList();
    }

    public void ChecarResposta() 
    {
        if (EventSystem.current.currentSelectedGameObject.tag != "Certa")
        { jogador.PerderVida(); }
        else { jogador.GanharPontoMat01(); }

        if (JogadorMat02.vidas >= 0) { ApagarPeixes(); StartCoroutine("CriarPeixes"); }
    }




}
