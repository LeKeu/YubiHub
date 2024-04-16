using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigoMat01 : MonoBehaviour
{
    public List<GameObject> inimigos;
    private Vector2 pos;

    private void Start()
    {
        pos = gameObject.transform.position;
    }
    public void Morrer() { Destroy(gameObject.transform.GetChild(0).gameObject); StartCoroutine(AparecerOutroInimigo()); }

    IEnumerator AparecerOutroInimigo() 
    {  
        yield return new WaitForSeconds(1.5f);
        var inim = Instantiate(inimigos[Random.Range(0, inimigos.Count)], pos, Quaternion.identity);
        inim.transform.parent = gameObject.transform;
    }
}
