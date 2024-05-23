using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Linq;
using Unity.Barracuda;

public class CriarPergunta : MonoBehaviour
{
    [SerializeField] GameObject ModeloScript;
    public static bool isModel;
    int streak;
    SFX_scripts sFX_Scripts;
    [SerializeField] TextMeshProUGUI pergunta;
    [SerializeField] Button opc1;
    [SerializeField] Button opc2;
    [SerializeField] Button opc3;

    [SerializeField] int rangeFacil = 30;
    [SerializeField] int rangeMedio = 60;
    [SerializeField] int rangeDificil = 150;

    JogadorMat01 jogador;
    InimigoMat01 inimigo;

    [SerializeField] List<Sprite> botoes;
    List<string> operacoes = new List<string>() { "+", "-", "x", "/" };

    //public string pergunta;

    void Start()
    {
        streak = 0;
        sFX_Scripts = gameObject.GetComponent<SFX_scripts>();
        jogador = GameObject.FindGameObjectWithTag("Player").GetComponent<JogadorMat01>();
        inimigo = GameObject.FindGameObjectWithTag("Inimigo").GetComponent<InimigoMat01>();
        StartCoroutine("GerarPergunta");
    }

    IEnumerator GerarPergunta()
    {
        AtrasarPergutas();
        yield return new WaitForSeconds(1.5f);
        CorBotaoPadrao();
        AtrasarPergutas();
        List<Button> buttons = new List<Button>() { opc1, opc2, opc3 };

        List<int> op_ret = Operacoes();
        int resp = op_ret[0]; int num1 = op_ret[1]; int num2 = op_ret[2]; int op = op_ret[3];

        pergunta.text = num1.ToString() + $" {operacoes[op]} " + num2.ToString();

        List<int> posicoes = Misturar(new List<int>() {0, 1, 2});


        for (int i = 0; i < posicoes.Count; i++)
        {
            buttons[posicoes[i]].GetComponentInChildren<TextMeshProUGUI>().text = NumRedor(resp, i).ToString();
            buttons[posicoes[i]].gameObject.tag = "Errada";
        }
        buttons[posicoes[2]].GetComponentInChildren<TextMeshProUGUI>().text = resp.ToString();
        buttons[posicoes[2]].gameObject.tag = "Certa";

    }

    private List<int> Soma(int limMin, int limMax)
    {
        int num1 = Random.Range(limMin, limMax);
        int num2 = Random.Range(limMin, limMax);
        return new List<int>() { num1 + num2, num1, num2 };
    }

    private List<int> Subtracao(int limMin, int limMax)
    {
        int num1 = Random.Range(limMin, limMax), num2 = Random.Range(limMin, limMax);
        while (num1 < num2)
        {
            num1 = Random.Range(limMin, limMax); num2 = Random.Range(limMin, limMax);
        } return new List<int>() { num1 - num2, num1, num2 };
    }

    private List<int> Divisao(int limMin, int limMax)
    { // diminuindo o limMax do num2 para nao ficar numeros absurdos na divisao
        int num1 = Random.Range(limMin, limMax);
        int num2 = isModel? Random.Range(limMin, limMax) : Random.Range(1, 11);
        while (num1 % num2 != 0)
        {
            num1 = Random.Range(limMin, limMax); 
            num2 = isModel? Random.Range(limMin, limMax) : Random.Range(limMin, num1);
        }
        Debug.Log($"{num1}/{num2}");
        return new List<int>() { num1 / num2, num1, num2 };
    }

    private List<int> Multiplicacao(int limMin, int limMax)
    { // diminuindo o limMax do num2 para nao ficar numeros absurdos na multiplicacao
        int num1 = Random.Range(limMin, limMax); 
        int num2 = isModel ? Random.Range(limMin, limMax) : Random.Range(1, 10);
        return new List<int>() { num1 * num2, num1, num2};
    }

    private List<int> Operacoes()
    {
        //Debug.Log("entrou operações");
        List<int> dfc = new List<int>() { rangeFacil, rangeMedio, rangeDificil };
        int limiteMax; int limiteMin;

        if (isModel)
        {
            //Debug.Log("streak --> " + streak);
            List<float> valoresModel = gameObject.GetComponent<Model_Range_MAT01>().ResultModel((float)streak);
            limiteMax = (int)valoresModel[0];
            limiteMin = (int)valoresModel[1];
            //Debug.Log("limite max --> "+limiteMax);
            //Debug.Log("limite min --> " + limiteMin);
        }
        else
        {
            limiteMax = dfc[MatMenu.dificuldade];
            limiteMin = MatMenu.dificuldade == 0 ? 1 : dfc[MatMenu.dificuldade - 1];
        }
        
        int operac = Random.Range(0, operacoes.Count);
        List<int> resp = new List<int>();

        switch (operac)
        {
            case 0: resp = Soma(limiteMin, limiteMax); break;
            case 1: resp = Subtracao(limiteMin, limiteMax); break;
            case 2: resp = Multiplicacao(limiteMin, limiteMax); break;
            case 3: resp = Divisao(limiteMin, limiteMax); break;
        }
        return new List<int>() { resp[0], resp[1], resp[2], operac };
    }   // resp0 = resposta da op, resp1 num1, resp2 num2, operac op usada na conta

    int aux = 0;
    private int NumRedor(int num, int i)
    { // 0 menor, 1 maior
        int numRedor = 0;

        while (true)
        {
            int esc = Random.Range(0, 2);

            if (esc == 0) { numRedor = Random.Range(num - 6, num - 1); }
            else { numRedor = Random.Range(num + 1, num + 6); }
            if (numRedor >= 0 && numRedor != num) { break; }
        }
        //Debug.Log($"i = {i}, numnovo = {numRedor}, aux = {aux}");
        //Debug.Log("=======================");
        if (i == 0) { aux = numRedor; }
        if (i == 1 && numRedor == aux) { numRedor = numRedor + 1 == num ? numRedor + 2 : numRedor + 1; Debug.Log("inguaiss"); }

        return numRedor;
        
    }

    private List<int> Misturar(List<int> elementos)
    {
        return elementos.OrderBy(x => Random.value).ToList();
    }

    public void ChecarResposta()
    {
        if (EventSystem.current.currentSelectedGameObject.tag != "Certa")
        { jogador.PerderVida(); streak--; sFX_Scripts.SoundErrar(); }
        else { jogador.GanharPontoMat01(); inimigo.Morrer(); streak++; sFX_Scripts.SoundAcertar(); }
        CorBotaoCertoErrado(EventSystem.current.currentSelectedGameObject);
        if(JogadorMat01.vidas >= 0)
            StartCoroutine("GerarPergunta");
        else { Limpar(); }
    }

    private void AtrasarPergutas()
    {
        pergunta.enabled = !pergunta.isActiveAndEnabled;
        Limpar();
        opc1.enabled = !opc1.isActiveAndEnabled; opc2.enabled = !opc2.isActiveAndEnabled; opc3.enabled = !opc3.isActiveAndEnabled;
    }

    private void Limpar()
    {
        pergunta.text = "";
        //opc1.GetComponentInChildren<TextMeshProUGUI>().text = ""; opc2.GetComponentInChildren<TextMeshProUGUI>().text = ""; opc3.GetComponentInChildren<TextMeshProUGUI>().text = "";
    }

    private void CorBotaoCertoErrado(GameObject butCerto) // 0 - verde, 1 - vermelho, 2 - padrao (laranja)
    {
        Debug.Log("coresbuts!!!");
        GameObject.Find("opc1").GetComponent<Image>().sprite = opc1.transform.tag == "Certo" ? botoes[0] : botoes[1];
        GameObject.Find("opc2").GetComponent<Image>().sprite = opc2.transform.tag == "Certo" ? botoes[0] : botoes[1];
        GameObject.Find("opc3").GetComponent<Image>().sprite = opc3.transform.tag == "Certo" ? botoes[0] : botoes[1];

        butCerto.GetComponent<Image>().sprite = botoes[0];
    }

    private void CorBotaoPadrao() // 0 - verde, 1 - vermelho, 2 - padrao (laranja)
    {
        GameObject.Find("opc1").GetComponent<Image>().sprite = botoes[2];
        GameObject.Find("opc2").GetComponent<Image>().sprite = botoes[2];
        GameObject.Find("opc3").GetComponent<Image>().sprite = botoes[2];
    }
}
