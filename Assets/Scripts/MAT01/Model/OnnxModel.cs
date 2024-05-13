using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Barracuda;

public class OnnxModel : MonoBehaviour
{
    [SerializeField] NNModel kerasModel;
    Model runTimeModel;
    IWorker worker;
    string outputLayer;
    // Start is called before the first frame update
    void Start()
    {
        runTimeModel = ModelLoader.Load(kerasModel);
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.Auto, runTimeModel);
        outputLayer = runTimeModel.outputs[runTimeModel.outputs.Count - 1];
    }

    public void ResultModel(int streak)
    {
        using Tensor inputTensor = new Tensor(1, 1);

        inputTensor[0] = streak;
        worker.Execute(inputTensor);

        Tensor outputTensor = worker.PeekOutput(outputLayer);
        Debug.Log(outputTensor);
        Debug.Log(outputTensor.data);
        Debug.Log(outputTensor.ToString());
    }

    private void OnDestroy()
    {
        worker?.Dispose();
    }
}
