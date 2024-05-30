using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Mat03Main : MonoBehaviour
{
    public static string nomeMat03;

    [SerializeField] private Transform area;
    [SerializeField] Button butMemoria;

    public Sprite[] Numeros;
    public Sprite[] Pontos;

    public Sprite bgImg;

    public List<Sprite> GamePuzzle;

    [SerializeField] List<Button> buts;

    int qntdBut;
    int linhas;

    bool firstGuess, secondGuess;
    int countGuesses, countCorrectGuesses, gameGuesses;
    int firstGuessIndex, secondGuessIndex;
    string firstGuessPuzzle, secondGuessPuzzle;

    SFX_scripts sFX_Scripts;

    private void Awake()
    {
        Numeros = Resources.LoadAll<Sprite>("Sprites/Numeros");
        Pontos = Resources.LoadAll<Sprite>("Sprites/Pontos");
        AjustesBotoes();
        CriarBotoes();
    }

    private void Start()
    {
        sFX_Scripts = gameObject.GetComponent<SFX_scripts>();
        nomeMat03 = "Mat03";
        GameObject[] objects = GameObject.FindGameObjectsWithTag("ButMemoria");
        foreach (GameObject obj in objects)
        {
            buts.Add(obj.GetComponent<Button>());
        }
        AddListeners();
        AdicionarPuzzle();
        GamePuzzle = GamePuzzle.OrderBy(x => Random.value).ToList();
        gameGuesses = GamePuzzle.Count / 2;
    }

    public void AdicionarPuzzle()
    {
        int looper = buts.Count;
        int index = 0;

        List<int> indexes = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        indexes = indexes.OrderBy(x => Random.value).ToList();

        List<Sprite> auxNumList = new();
        List<Sprite> auxPontosList = new();
        foreach (int i in indexes)
        {
            auxNumList.Add(Numeros[i]);
            auxPontosList.Add(Pontos[i]);
        }

        for (int i = 0; i < looper; i++)
        {
            if (index % 2 == 0)
                GamePuzzle.Add(auxNumList[index]);
            else
                GamePuzzle.Add(auxPontosList[index - 1]);
            index++;
        }
    }


    void AddListeners()
    {
        foreach (Button button in buts)
        {
            button.onClick.AddListener(() => EscolherBut());
        }
    }

    public void EscolherBut()
    {
        if (!firstGuess)
        {
            firstGuess = true;
            firstGuessIndex = int.Parse(EventSystem.current.currentSelectedGameObject.name);
            firstGuessPuzzle = GamePuzzle[firstGuessIndex].name;

            buts[firstGuessIndex].image.sprite = GamePuzzle[firstGuessIndex];
        } else if (!secondGuess)
        {
            secondGuess = true;
            secondGuessIndex = int.Parse(EventSystem.current.currentSelectedGameObject.name);
            secondGuessPuzzle = GamePuzzle[secondGuessIndex].name;

            buts[secondGuessIndex].image.sprite = GamePuzzle[secondGuessIndex];

            countGuesses++;

            StartCoroutine(ChecarSeIguais());
        }
    }

    IEnumerator ChecarSeIguais()
    {
        yield return new WaitForSeconds(1.5f);
        if (firstGuessPuzzle == secondGuessPuzzle && firstGuessIndex != secondGuessIndex)
        {
            sFX_Scripts.SoundAcertar();
            yield return new WaitForSeconds(1f);

            buts[firstGuessIndex].interactable = false;
            buts[secondGuessIndex].interactable = false;

            buts[firstGuessIndex].image.color = new Color(0, 0, 0, 0);
            buts[secondGuessIndex].image.color = new Color(0, 0, 0, 0);
            ChecarSeAcabou();
        }
        else    // se não forem iguais
        {
            if (firstGuessIndex != secondGuessIndex)
            {
                sFX_Scripts.SoundErrar();
                yield return new WaitForSeconds(1f);
                
                buts[firstGuessIndex].image.sprite = bgImg;
                buts[secondGuessIndex].image.sprite = bgImg;
            }
        }
        yield return new WaitForSeconds(1f);

        firstGuess = secondGuess = false;
    }

    void ChecarSeAcabou()
    {
        countCorrectGuesses++;
        if (countCorrectGuesses == gameGuesses)
            StartCoroutine(Acabar());
    }

    IEnumerator Acabar()
    {
        string aux = $"{InfoJogador.nomeJogador}_{nomeMat03}_{Mat03Menu.dificuldade}";
        string aux_pontos = $"{InfoJogador.nomeJogador}_{nomeMat03}_{Mat03Menu.dificuldade}_pontos";

        if (PlayerPrefs.GetInt(aux)  == 0) { PlayerPrefs.SetInt(aux, 999); }

        PlayerPrefs.SetString(aux_pontos, PlayerPrefs.GetString(aux_pontos) + "_" + countGuesses);

        if (PlayerPrefs.GetInt(aux) > countGuesses) { PlayerPrefs.SetInt(aux, countGuesses); }

        yield return new WaitForSeconds(2); SceneManager.LoadScene("MatMenu03");
    }

    private void AjustesBotoes()
    {
        switch (Mat03Menu.dificuldade)
        {
            case 0:
                qntdBut = 8; linhas = 4; break;
            case 1:
                qntdBut = 10; linhas = 5; break;
            case 2:
                qntdBut = 12; linhas = 4; break;
        }
        area.GetComponent<GridLayoutGroup>().constraintCount = linhas;
    }

    private void CriarBotoes()
    {
        for (int i = 0; i < qntdBut; i++)
        {
            Button but = Instantiate(butMemoria);
            but.name = i.ToString();
            but.transform.SetParent(area, false);
        }
    }
}
