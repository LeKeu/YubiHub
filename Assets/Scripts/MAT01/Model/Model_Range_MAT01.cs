using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TensorFlowLite;
using System.IO;

public class Model_Range_MAT01 : MonoBehaviour
{

    [SerializeField] string fileName = "Mat01Model_lite.tflite";
    Interpreter interpreter;

    private void Awake()
    {
        // Construct the file path
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        if (!File.Exists(filePath))
        {
            Debug.LogError($"Model file not found at: {filePath}");
            return;
        }
        
    }

    private void OnDestroy()
    {
        if(CriarPergunta.isModel)
            interpreter.Dispose();
    }

    public List<float> ResultModel(float streak)
    {
        var options = new InterpreterOptions()
        {
            threads = 2,
        };

        try
        {
            byte[] modelData = File.ReadAllBytes(fileName);
            interpreter = new Interpreter(modelData, options);
            interpreter.AllocateTensors();
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to load model: {ex}");
        }

        Debug.Log("MODELMODEL");

        interpreter = new Interpreter(FileUtil.LoadFile(fileName), options);
        interpreter.AllocateTensors();

        var input = new float[1];
        input[0] = (float)streak;
        var output = new float[2];

        //interpreter.ResetVariableTensors();
        interpreter.SetInputTensorData(0, input);
        interpreter.Invoke();
        interpreter.GetOutputTensorData(0, output);

        Debug.Log(output[0]);
        Debug.Log(output[1]);

        List<float> returnValues = new List<float>
        {
            Mathf.Ceil(output[0]),
            Mathf.Ceil(output[1])
        };

        return returnValues;
    }
}
