using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigoMat01 : MonoBehaviour
{
    public List<GameObject> inimigos;
    public List<GameObject> carrinhos;
    private Vector2 pos;

    private void Start()
    {
        pos = gameObject.transform.position;
    }
    public void Morrer() 
    {
        for (var i = gameObject.transform.childCount - 1; i >= 0; i--)
        {
            Object.Destroy(gameObject.transform.GetChild(i).gameObject);
        }
        StartCoroutine(AparecerOutroInimigo()); 
    }

    IEnumerator AparecerOutroInimigo() 
    {  
        yield return new WaitForSeconds(1.5f);
        var inim = Instantiate(inimigos[Random.Range(0, inimigos.Count)], pos, Quaternion.identity);
        inim.transform.parent = gameObject.transform;

        //var carrin = Instantiate(carrinhos[Random.Range(0, carrinhos.Count)], new Vector2(2.83f, -1.31f), Quaternion.identity);
        //carrin.transform.parent = gameObject.transform;
    }
}
