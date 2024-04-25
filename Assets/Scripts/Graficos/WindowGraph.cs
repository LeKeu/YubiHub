using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class WindowGraph : MonoBehaviour
{
    [SerializeField] private Sprite circleSprite;
    private RectTransform graphContainer;

    RectTransform labelTemplateX;
    RectTransform labelTemplateY;

    RectTransform dashTemplateX;
    RectTransform dashTemplateY;

    private void Awake()
    {
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        labelTemplateX = graphContainer.Find("LabelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("LabelTemplateY").GetComponent<RectTransform>();

        dashTemplateX = graphContainer.Find("DashTemplateX").GetComponent<RectTransform>();
        dashTemplateY = graphContainer.Find("DashTemplateY").GetComponent<RectTransform>();

        List<int> valoresInt = new List<int>() { 5, 45, 74, 23, 9, 34, 10, 8, 12, 3, 77, 2 } ;

        MostrarGraph(valoresInt);
    }

    private GameObject CriarCirculo(Vector2 anchoredPos)
    {
        GameObject gameObject = new GameObject("circulo", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite; 
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPos;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }

    private void MostrarGraph(List<int> valores)
    {
        float graphAltura = graphContainer.sizeDelta.y;
        float yMax = 100f;
        float xSize = 50f;

        GameObject ultimoCirculoGameObject = null;
        for (int i = 0; i < valores.Count; i++)
        {
            float xPos = xSize + i * xSize;
            float yPos = (valores[i] / yMax) * graphAltura;
            GameObject circuloGameObject = CriarCirculo(new Vector2(xPos, yPos));
            if(ultimoCirculoGameObject != null)
            {
                CriarPontoConnection(ultimoCirculoGameObject.GetComponent<RectTransform>().anchoredPosition, circuloGameObject.GetComponent<RectTransform>().anchoredPosition);
            } 
            ultimoCirculoGameObject = circuloGameObject;

            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(graphContainer, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPos, -7f);
            labelX.GetComponent<Text>().text = i.ToString();

            RectTransform dashX = Instantiate(dashTemplateY);
            dashX.SetParent(graphContainer, false);
            dashX.gameObject.SetActive(true);
            dashX.anchoredPosition = new Vector2(xPos, -3f);
        }

        int separador = 10;
        for(int i = 0;i <= separador;i++)
        {
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer, false);
            labelY.gameObject.SetActive(true);
            float valorNormalizado = i * 1f / separador;
            labelY.anchoredPosition = new Vector2(-7f, valorNormalizado * graphAltura);
            labelY.GetComponent<Text>().text = Mathf.RoundToInt(valorNormalizado * yMax).ToString();

            RectTransform dashY = Instantiate(dashTemplateX);
            dashY.SetParent(graphContainer, false);
            dashY.gameObject.SetActive(true);
            dashY.anchoredPosition = new Vector2(-4f, valorNormalizado * graphAltura);
        }
    }

    private void CriarPontoConnection(Vector2 pontoPosA, Vector2 pontoPosB)
    {
        GameObject gameObject2 = new GameObject("pontoConnection", typeof(Image));
        gameObject2.transform.SetParent(graphContainer, false);
        gameObject2.GetComponent<Image>().color = new Color(1, 1, 1, .5f);
        RectTransform rectTransform = gameObject2.GetComponent<RectTransform>();
        Vector2 dir = (pontoPosB - pontoPosA).normalized;
        float distancia = Vector2.Distance(pontoPosA, pontoPosB);

        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distancia, 3f);
        rectTransform.anchoredPosition = pontoPosA + dir * distancia * .5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
    }
}
