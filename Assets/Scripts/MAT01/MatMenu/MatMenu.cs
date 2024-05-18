using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatMenu : MonoBehaviour
{
    public static int dificuldade;

    //[SerializeField] List<TextMeshProUGUI> dificuldadesText;

    private void Start()
    {
        //dificuldadesText[0].text = PlayerPrefs.GetInt($"{InfoJogador.nomeJogador}_Mat01_0").ToString();
        //dificuldadesText[1].text = PlayerPrefs.GetInt($"{InfoJogador.nomeJogador}_Mat01_1").ToString();
        //dificuldadesText[2].text = PlayerPrefs.GetInt($"{InfoJogador.nomeJogador}_Mat01_2").ToString();
        //dificuldadesText[3].text = PlayerPrefs.GetInt($"{InfoJogador.nomeJogador}_Mat01_3").ToString();
    }

    public void Facil() { dificuldade = 0; CriarPergunta.isModel = false; SceneManager.LoadScene("Mat01"); }

    public void Medio() {  dificuldade = 1; CriarPergunta.isModel = false; SceneManager.LoadScene("Mat01"); }

    public void Dificil() { dificuldade = 2; CriarPergunta.isModel = false; SceneManager.LoadScene("Mat01"); }

    public void Model() { dificuldade = 3; CriarPergunta.isModel = true; SceneManager.LoadScene("Mat01"); }
}
