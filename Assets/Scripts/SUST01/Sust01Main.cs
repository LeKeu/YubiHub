using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sust01Main : MonoBehaviour
{
    Sprite[] lixos;

    GameObject lixoPai;
    GameObject aux;

    Vector3 posInicial;
    void Start()
    {
        lixos = Resources.LoadAll<Sprite>("Sprites/Lixos");
        lixoPai = GameObject.Find("Lixo");
        aux = GameObject.Find("aux");
        StartCoroutine("CriarLixo");
        posInicial = aux.transform.position;
    }

    void Update()
    {
        if (ArrastarObj.chamarFunc) { StartCoroutine("CriarLixo"); }
    }

    IEnumerator CriarLixo()
    {
        ArrastarObj.chamarFunc = false;
        yield return new WaitForSeconds(2);
        aux.transform.position = posInicial; 
        //var px = Instantiate(lixos[Random.Range(0, lixos.Length)], lixoPai.gameObject.transform.position, Quaternion.identity);
        //px.transform.parent = lixoPai.transform;
        Sprite lixo = lixos[Random.Range(0, lixos.Length)];
        aux.GetComponent<SpriteRenderer>().sprite = lixo;
        aux.transform.name = lixo.name;
    }


}
