using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TensorFlowLite;

public class Model_Range_MAT01 : MonoBehaviour
{

    string fileName;
    Interpreter interpreter;

    private void Awake()
    {
        fileName = "Mat01Model.tflite";
    }

    void Start()
    {
        var options = new InterpreterOptions()
        {
            threads = 2,
        };

        interpreter = new Interpreter(FileUtil.LoadFile(fileName), options);
        interpreter.AllocateTensors();
    }

    public List<int> ResultModel(int streak)
    {
        int[] input = new int[1];
        input[0] = streak;
        var output = new float[2];

        interpreter.ResetVariableTensors();
        interpreter.SetInputTensorData(0, input);
        interpreter.Invoke();
        interpreter.GetOutputTensorData(0, output);

        List<int> returnValues = new List<int>
        {
            (int)Mathf.Ceil(output[0]),
            (int)Mathf.Ceil(output[1])
        };

        return returnValues;
    }

    private void OnDestroy()
    {
        interpreter.Dispose();
    }
}
