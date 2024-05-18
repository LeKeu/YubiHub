using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TensorFlowLite;
using System.IO;

public class Model_Range_MAT01 : MonoBehaviour
{

    [SerializeField] string filePath = "Assets/StreamingAssets/Mat01Model_lite.tflite";
    Interpreter interpreter;

    private void Awake()
    {
        
    }

    private void OnDestroy()
    {
        interpreter.Dispose();
    }

    public List<float> ResultModel(float streak)
    {
        var options = new InterpreterOptions()
        {
            threads = 2,
        };

        interpreter = new Interpreter(File.ReadAllBytes(filePath), options);
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
