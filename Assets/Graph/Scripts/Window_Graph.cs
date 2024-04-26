/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class Window_Graph : MonoBehaviour {

    [SerializeField] private Sprite circleSprite;
    private GameObject[] graphContainer;
    private GameObject[] labelTemplateX;
    private GameObject[] labelTemplateY;
    //private RectTransform dashTemplateX;
    //private RectTransform dashTemplateY;
    //private RectTransform dashes;

    private void Awake() {
        graphContainer = GameObject.FindGameObjectsWithTag("graphContainer");
        labelTemplateX = GameObject.FindGameObjectsWithTag("LabelTemplateX");
        labelTemplateY = GameObject.FindGameObjectsWithTag("LabelTemplateY");

        EachGraph();
    }

    private void EachGraph()
    {
         
        for(int i = 0; i < graphContainer.Length; i++)
        {
            RectTransform container = graphContainer[i].GetComponent<RectTransform>();
            RectTransform labelX = labelTemplateX[i].GetComponent<RectTransform>();
            RectTransform labelY = labelTemplateY[i].GetComponent<RectTransform>();

            ShowGraph(InfoJogador.RetornarPontos(InfoJogador.JogadorScores[i]), container, labelX, labelY, (int _i) => "Vez " + (_i + 1), (float _f) => "$" + Mathf.RoundToInt(_f));
        }
    }

    private void ShowGraph
        (List<string> valueList, RectTransform graphContainer0, RectTransform labelTemplateX0, RectTransform labelTemplateY0,
        Func<int, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null) {
        if (getAxisLabelX == null) {
            getAxisLabelX = delegate (int _i) { return _i.ToString(); };
        }
        if (getAxisLabelY == null) {
            getAxisLabelY = delegate (float _f) { return Mathf.RoundToInt(_f).ToString(); };
        }

        float graphHeight = graphContainer0.sizeDelta.y;
        float yMaximum = 30f;
        float xSize = 50f;

        GameObject lastCircleGameObject = null;
        for (int i = 0; i < valueList.Count; i++) {
            float xPosition = xSize + i * xSize;
            float yPosition = (int.Parse(valueList[i]) / yMaximum) * graphHeight;
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition), graphContainer0);
            if (lastCircleGameObject != null) {
                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition, graphContainer0);
            }
            lastCircleGameObject = circleGameObject;

            RectTransform labelX = Instantiate(labelTemplateX0);
            labelX.SetParent(graphContainer0, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -7f);
            labelX.GetComponent<Text>().text = getAxisLabelX(i);
        }

        int separatorCount = 10;
        for (int i = 0; i <= separatorCount; i++) {
            RectTransform labelY = Instantiate(labelTemplateY0);
            labelY.SetParent(graphContainer0, false);
            labelY.gameObject.SetActive(true);
            float normalizedValue = i * 1f / separatorCount;
            labelY.anchoredPosition = new Vector2(-7f, normalizedValue * graphHeight);
            labelY.GetComponent<Text>().text = getAxisLabelY(normalizedValue * yMaximum);
        }
    }

    private GameObject CreateCircle(Vector2 anchoredPosition, RectTransform graphContainer0)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer0, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }

    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB, RectTransform container) {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(container, false);
        gameObject.GetComponent<Image>().color = new Color(1,1,1, .5f);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
    }

}
