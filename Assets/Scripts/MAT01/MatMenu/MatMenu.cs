using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatMenu : MonoBehaviour
{
    public static int dificuldade;

    [SerializeField] List<TextMeshProUGUI> dificuldadesText;

    private void Start()
    {
        dificuldadesText[0].text = PlayerPrefs.GetInt($"{InfoJogador.nomeJogador}_Mat01_0").ToString();
        dificuldadesText[1].text = PlayerPrefs.GetInt($"{InfoJogador.nomeJogador}_Mat01_1").ToString();
        dificuldadesText[2].text = PlayerPrefs.GetInt($"{InfoJogador.nomeJogador}_Mat01_2").ToString();
    }

    public void Facil() { dificuldade = 0; SceneManager.LoadScene("Mat01"); }

    public void Medio() {  dificuldade = 1; SceneManager.LoadScene("Mat01"); }

    public void Dificil() { dificuldade = 2; SceneManager.LoadScene("Mat01"); }
}
