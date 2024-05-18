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
using TMPro;
using Unity.VisualScripting;

public class Window_Graph : MonoBehaviour {

    [SerializeField] private Sprite dotSprite;
    [SerializeField] private GameObject TextObj;

    private RectTransform graphContainer;
    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;
    private RectTransform dashTemplateX;
    private RectTransform dashTemplateY;
    private List<GameObject> gameObjectList;

    // Cached values
    private List<int> valueList;
    private IGraphVisual graphVisual;
    private int maxVisibleValueAmount;
    private Func<int, string> getAxisLabelX;
    private Func<float, string> getAxisLabelY;

    GameObject barChartBtn;
    GameObject lineGraphBtn;
    GameObject decreaseVisibleAmountBtn;
    GameObject increaseVisibleAmountBtn;

    private void Awake()
    {
        barChartBtn = GameObject.FindGameObjectWithTag("barChartBtn");
        lineGraphBtn = GameObject.FindGameObjectWithTag("lineGraphBtn");
        decreaseVisibleAmountBtn = GameObject.FindGameObjectWithTag("decreaseVisibleAmountBtn");
        increaseVisibleAmountBtn = GameObject.FindGameObjectWithTag("increaseVisibleAmountBtn");

        DesativarBotoes();
    }

    public void CriarGrafico(string parametro)
    {
        DesativarBotoes();
        // Grab base objects references
        graphContainer = GameObject.FindGameObjectWithTag("graphContainer").GetComponent<RectTransform>();
        labelTemplateX = GameObject.FindGameObjectWithTag("LabelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = GameObject.FindGameObjectWithTag("LabelTemplateY").GetComponent<RectTransform>();
        //dashTemplateX = GameObject.FindGameObjectWithTag("dashTemplateX").GetComponent<RectTransform>();
        //dashTemplateY = GameObject.FindGameObjectWithTag("dashTemplateY").GetComponent<RectTransform>();

        gameObjectList = new List<GameObject>();

        List<int> pontos = InfoJogador.RetornarPontos(parametro);
        Debug.Log("----"+parametro);
        if (pontos.Count == 0)
        { GameObject.Find("aviso").GetComponent<TextMeshProUGUI>().text = "Ainda não há dados nesse jogo."; DesativarBotoes(); return; }
        else { GameObject.Find("aviso").GetComponent<TextMeshProUGUI>().text = parametro; AtivarBotoes(); }

        IGraphVisual lineGraphVisual = new LineGraphVisual(graphContainer, dotSprite, Color.green, new Color(1, 1, 1, .5f));
        IGraphVisual barChartVisual = new BarChartVisual(graphContainer, Color.white, .8f);
        ShowGraph(pontos, barChartVisual, -1, (int _i) => "" + ($"{pontos[_i]}"), (float _f) => "" + Mathf.RoundToInt(_f), TextObj);
        //($"{_i+1}\n{pontos[_i]}")
        GameObject.FindGameObjectWithTag("barChartBtn").GetComponent<Button_UI>().ClickFunc = () => {
            SetGraphVisual(barChartVisual);
        };
        GameObject.FindGameObjectWithTag("lineGraphBtn").GetComponent<Button_UI>().ClickFunc = () => {
            SetGraphVisual(lineGraphVisual);
        };

        GameObject.FindGameObjectWithTag("decreaseVisibleAmountBtn").GetComponent<Button_UI>().ClickFunc = () => {
            DecreaseVisibleAmount();
        };
        GameObject.FindGameObjectWithTag("increaseVisibleAmountBtn").GetComponent<Button_UI>().ClickFunc = () => {
            IncreaseVisibleAmount();
        };
    }

    public void DesativarBotoes()
    {
        barChartBtn.SetActive(false);
        lineGraphBtn.SetActive(false);
        decreaseVisibleAmountBtn.SetActive(false);
        increaseVisibleAmountBtn.SetActive(false);
    }

    public void AtivarBotoes()
    {
        barChartBtn.SetActive(true);
        lineGraphBtn.SetActive(true);
        decreaseVisibleAmountBtn.SetActive(true);
        increaseVisibleAmountBtn.SetActive(true);
    }

    private void IncreaseVisibleAmount() {
        ShowGraph(this.valueList, this.graphVisual, this.maxVisibleValueAmount + 1, this.getAxisLabelX, this.getAxisLabelY);
    }

    private void DecreaseVisibleAmount() {
        ShowGraph(this.valueList, this.graphVisual, this.maxVisibleValueAmount - 1, this.getAxisLabelX, this.getAxisLabelY);
    }

    private void SetGraphVisual(IGraphVisual graphVisual) {
        ShowGraph(this.valueList, graphVisual, this.maxVisibleValueAmount, this.getAxisLabelX, this.getAxisLabelY);
    }

    public void ClearGraph()
    {
        GameObject[] apagar = GameObject.FindGameObjectsWithTag("Temp");
        foreach(GameObject obj in apagar) { Destroy(obj); }
    }

    private void ShowGraph(List<int> valueList, IGraphVisual graphVisual, int maxVisibleValueAmount = -1, Func<int, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null, GameObject textObj = null) {
        this.valueList = valueList;
        this.graphVisual = graphVisual;
        this.getAxisLabelX = getAxisLabelX;
        this.getAxisLabelY = getAxisLabelY;

        if (maxVisibleValueAmount <= 0) {
            // Show all if no amount specified
            maxVisibleValueAmount = valueList.Count;
        }
        if (maxVisibleValueAmount > valueList.Count) {
            // Validate the amount to show the maximum
            maxVisibleValueAmount = valueList.Count;
        }

        this.maxVisibleValueAmount = maxVisibleValueAmount;

        // Test for label defaults
        if (getAxisLabelX == null) {
            getAxisLabelX = delegate (int _i) { return _i.ToString(); };
        }
        if (getAxisLabelY == null) {
            getAxisLabelY = delegate (float _f) { return Mathf.RoundToInt(_f).ToString(); };
        }

        // Clean up previous graph
        foreach (GameObject gameObject in gameObjectList) {
            Destroy(gameObject);
        }
        gameObjectList.Clear();
        
        // Grab the width and height from the container
        float graphWidth = graphContainer.sizeDelta.x;
        float graphHeight = graphContainer.sizeDelta.y;

        // Identify y Min and Max values
        float yMaximum = valueList[0];
        float yMinimum = valueList[0];
        
        for (int i = Mathf.Max(valueList.Count - maxVisibleValueAmount, 0); i < valueList.Count; i++) {
            int value = valueList[i];
            if (value > yMaximum) {
                yMaximum = value;
            }
            if (value < yMinimum) {
                yMinimum = value;
            }
        }

        float yDifference = yMaximum - yMinimum;
        if (yDifference <= 0) {
            yDifference = 5f;
        }
        yMaximum = yMaximum + (yDifference * 0.2f);
        yMinimum = yMinimum - (yDifference * 0.2f);

        yMinimum = 0f; // Start the graph at zero

        // Set the distance between each point on the graph 
        float xSize = graphWidth / (maxVisibleValueAmount + 1);

        // Cycle through all visible data points
        int xIndex = 0;
        for (int i = Mathf.Max(valueList.Count - maxVisibleValueAmount, 0); i < valueList.Count; i++) {
            float xPosition = xSize + xIndex * xSize;
            float yPosition = ((valueList[i] - yMinimum) / (yMaximum - yMinimum)) * graphHeight;

            // Add data point visual
            gameObjectList.AddRange(graphVisual.AddGraphVisual(new Vector2(xPosition, yPosition), xSize, valueList, i, textObj));

            // Duplicate the x label template
            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.tag = "Temp";
            labelX.SetParent(graphContainer, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -7f);
            labelX.GetComponent<Text>().text = getAxisLabelX(i);
            gameObjectList.Add(labelX.gameObject);
            
            // Duplicate the x dash template
            //RectTransform dashX = Instantiate(dashTemplateX);
            //dashX.SetParent(graphContainer, false);
            //dashX.gameObject.SetActive(true);
            //dashX.anchoredPosition = new Vector2(xPosition, -3f);
            //gameObjectList.Add(dashX.gameObject);

            xIndex++;
        }

        // Set up separators on the y axis
        int separatorCount = 10;
        for (int i = 0; i <= separatorCount; i++) {
            // Duplicate the label template
            //RectTransform labelY = Instantiate(labelTemplateY);
            //labelY.tag = "Temp";
            //labelY.SetParent(graphContainer, false);
            //labelY.gameObject.SetActive(true);
            //float normalizedValue = i * 1f / separatorCount;
            //labelY.anchoredPosition = new Vector2(-7f, normalizedValue * graphHeight);
            //labelY.GetComponent<Text>().text = getAxisLabelY(yMinimum + (normalizedValue * (yMaximum - yMinimum)));
            //gameObjectList.Add(labelY.gameObject);

            // Duplicate the dash template
            //RectTransform dashY = Instantiate(dashTemplateY);
            //dashY.SetParent(graphContainer, false);
            //dashY.gameObject.SetActive(true);
            //dashY.anchoredPosition = new Vector2(-4f, normalizedValue * graphHeight);
            //gameObjectList.Add(dashY.gameObject);
        }
    }



    /*
     * Interface definition for showing visual for a data point
     * */
    private interface IGraphVisual {

        List<GameObject> AddGraphVisual(Vector2 graphPosition, float graphPositionWidth, List<int> valores, int posicao, GameObject textObj);

    }


    /*
     * Displays data points as a Bar Chart
     * */
    private class BarChartVisual : IGraphVisual {

        private RectTransform graphContainer;
        private Color barColor;
        private float barWidthMultiplier;

        public BarChartVisual(RectTransform graphContainer, Color barColor, float barWidthMultiplier) {
            this.graphContainer = graphContainer;
            this.barColor = barColor;
            this.barWidthMultiplier = barWidthMultiplier;
        }

        public List<GameObject> AddGraphVisual(Vector2 graphPosition, float graphPositionWidth, List<int> valores, int posicao, GameObject textObj) {
            GameObject barGameObject = CreateBar(graphPosition, graphPositionWidth, valores, posicao, textObj);
            return new List<GameObject>() { barGameObject };
        }

        private GameObject CreateBar(Vector2 graphPosition, float barWidth, List<int> valores, int i, GameObject textObj) {
            GameObject gameObject_z = new GameObject("bar", typeof(Image));
            gameObject_z.tag = "Temp";
            gameObject_z.transform.SetParent(graphContainer, false);
            gameObject_z.GetComponent<Image>().color = barColor;
            RectTransform rectTransform = gameObject_z.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(graphPosition.x, 0f);
            rectTransform.sizeDelta = new Vector2(barWidth * barWidthMultiplier, graphPosition.y);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            rectTransform.pivot = new Vector2(.5f, 0f);
            Debug.Log("enbtrou barra");
            GameObject textValor = new GameObject("textoNum", typeof(TextMeshProUGUI));
            textValor.tag = "Temp";
            textValor.transform.SetParent(gameObject_z.transform);
            textValor.GetComponent<TextMeshProUGUI>().text = valores[i].ToString();
            textValor.transform.position = new Vector3(0, 0, 0);
            //rectTransformText.anchoredPosition = new Vector2(graphPosition.x, 0f);
            //rectTransformText.sizeDelta = new Vector2(barWidth * barWidthMultiplier, graphPosition.y);
            //rectTransformText.anchorMin = new Vector2(0, 0);
            //rectTransformText.anchorMax = new Vector2(0, 0);
            //rectTransformText.pivot = new Vector2(.5f, 0f);

            //GameObject textoValor = Instantiate(textObj, gameObject_z.transform.position, Quaternion.identity);
            //textoValor.transform.SetParent(gameObject_z.transform, false);
            //textoValor.transform.position = new Vector3(0, 0, 0);
            //textoValor.GetComponent<TextMeshProUGUI>().text = valores[i].ToString();
            //RectTransform rectTransformText = textoValor.GetComponent<RectTransform>();
            //rectTransformText.anchoredPosition = new Vector2(gameObject_z.transform.position.x, 0f);
            //rectTransformText.anchoredPosition = new Vector2(gameObject_z.transform.position.y, 0f);
            return gameObject_z;
        }
    }


    /*
     * Displays data points as a Line Graph
     * */
    private class LineGraphVisual : IGraphVisual {

        private RectTransform graphContainer;
        private Sprite dotSprite;
        private GameObject lastDotGameObject;
        private Color dotColor;
        private Color dotConnectionColor;

        public LineGraphVisual(RectTransform graphContainer, Sprite dotSprite, Color dotColor, Color dotConnectionColor) {
            this.graphContainer = graphContainer;
            this.dotSprite = dotSprite;
            this.dotColor = dotColor;
            this.dotConnectionColor = dotConnectionColor;
            lastDotGameObject = null;
        }


        public List<GameObject> AddGraphVisual(Vector2 graphPosition, float graphPositionWidth, List<int> valores, int posicao, GameObject textObj) {
            List<GameObject> gameObjectList = new List<GameObject>();
            GameObject dotGameObject = CreateDot(graphPosition);
            gameObjectList.Add(dotGameObject);
            if (lastDotGameObject != null) {
                GameObject dotConnectionGameObject = CreateDotConnection(lastDotGameObject.GetComponent<RectTransform>().anchoredPosition, dotGameObject.GetComponent<RectTransform>().anchoredPosition);
                gameObjectList.Add(dotConnectionGameObject);
            }
            lastDotGameObject = dotGameObject;
            return gameObjectList;
        }

        private GameObject CreateDot(Vector2 anchoredPosition) {
            GameObject gameObject = new GameObject("dot", typeof(Image));
            gameObject.transform.SetParent(graphContainer, false);
            gameObject.GetComponent<Image>().sprite = dotSprite;
            gameObject.GetComponent<Image>().color = dotColor;
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = anchoredPosition;
            rectTransform.sizeDelta = new Vector2(11, 11);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            return gameObject;
        }

        private GameObject CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB) {
            GameObject gameObject = new GameObject("dotConnection", typeof(Image));
            gameObject.transform.SetParent(graphContainer, false);
            gameObject.GetComponent<Image>().color = dotConnectionColor;
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            Vector2 dir = (dotPositionB - dotPositionA).normalized;
            float distance = Vector2.Distance(dotPositionA, dotPositionB);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            rectTransform.sizeDelta = new Vector2(distance, 3f);
            rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
            rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
            return gameObject;
        }
    }

}
