using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Barracuda;

public class Visualizer : MonoBehaviour
{
    public static bool newAnim;
    public Transform head;
    public Transform left;
    public Transform right;
    public Transform waist;

    public NNModel modelSource;

    private float hh;
    private float lx, ly, lz;
    private float rx, ry, rz;
    private float hrx, hry, hrz, hrw;
    private float lrx, lry, lrz, lrw;
    private float rrx, rry, rrz, rrw;

    private float standard_height;
    private Quaternion left_init_rotation;
    private Quaternion right_init_rotation;

    private IWorker worker;

    private Model model;

    // Start is called before the first frame update
    void Start()
    {   
        standard_height = head.position.y;
        left_init_rotation = Quaternion.Inverse(head.rotation) * left.rotation;
        right_init_rotation = Quaternion.Inverse(head.rotation) * right.rotation;

        model = ModelLoader.Load(modelSource);
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, model);
        newAnim = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (newAnim)
        {
            Refresh();
        }
        hh = head.position.y / standard_height;

        Vector3 L_position = head.InverseTransformDirection(left.position - head.position) / standard_height;
        lx = L_position.x;
        ly = L_position.y;
        lz = L_position.z;

        Vector3 R_position = head.InverseTransformDirection(right.position - head.position) / standard_height;
        rx = R_position.x;
        ry = R_position.y;
        rz = R_position.z;

        Quaternion H_rotation = head.rotation;
        hrx = H_rotation.x;
        hry = H_rotation.y;
        hrz = H_rotation.z;
        hrw = H_rotation.w;

        Quaternion L_rotation = left.rotation;
        lrx = L_rotation.x;
        lry = L_rotation.y;
        lrz = L_rotation.z;
        lrw = L_rotation.w;

        Quaternion R_rotation = right.rotation;
        rrx = R_rotation.x;
        rry = R_rotation.y;
        rrz = R_rotation.z;
        rrw = R_rotation.w;

        float[] intput_vector = new float[] {hh, lx, ly, lz, rx, ry, rz, hrx, hry, hrz, hrw, lrx, lry, lrz, lrw, rrx, rry, rrz, rrw};

        Tensor input = new Tensor(new TensorShape(1,1,19,1), intput_vector);
        worker.Execute(input);
        Tensor p = worker.PeekOutput("waist_position");
        Tensor r = worker.PeekOutput("waist_rotation");

        waist.position = (new Vector3(p[0], p[1], p[2])) * standard_height + head.position;
        waist.rotation = new Quaternion(r[0], r[1], r[2], r[3]) * head.rotation;

        input.Dispose();


    }

    public void Refresh()
    {
        worker.Dispose();
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.ComputePrecompiled, model);
        newAnim = false;
    }
}
