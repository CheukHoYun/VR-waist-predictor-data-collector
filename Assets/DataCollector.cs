using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;


public class DataCollector : MonoBehaviour
{
    // References to the four tracked objects
    public Transform head;
    public Transform left;

    public Transform right;

    public Transform waist;

    // Constant values
    private float standard_height = 0;
    private Quaternion left_init_rotation;
    private Quaternion right_init_rotation;
    public static StringBuilder output;

    // Stopping signal
    public static bool stopped;



    void Awake(){
        Data.Instance = new Data();
    }

    void Start()
    {
        // Initiating the stopping signal
        stopped = false;

        // Set up a standard height
        standard_height = head.position.y;

        // Set up the initial rotation of the two controllers
        left_init_rotation = Quaternion.Inverse(head.rotation) * left.rotation;
        right_init_rotation = Quaternion.Inverse(head.rotation) * right.rotation;

        // Add header to the output and initialize it
        // head height:
        output = new StringBuilder("h_height,");
        // position of hands and waist:
        output.Append("l_px, l_py, l_pz,");
        output.Append("r_px, r_py, r_pz,");
        output.Append("w_px, w_py, w_pz,");
        // rotation (in quaternion) of head, hands and waist:
        output.Append("h_rx, h_ry, h_rz, h_rw,");
        output.Append("l_rx, l_ry, l_rz, l_rw,");
        output.Append("r_rx, r_ry, r_rz, r_rw,");
        output.Append("w_rx, w_ry, w_rz, w_rw,");
        // Time stamp for each frame:
        output.Append("timestamp,");
        // Current animation #: 
        output.Append("Curr_animation");

    }



    // Update is called once per frame
    void Update()
    {
        if (!stopped)
        {
            Data.Instance.timestamp += Time.deltaTime;
            // Compute the height of the head as a float between 0 and 1.
            // Proportional to the height of the humanoid.
            Data.Instance.H_height = head.position.y / standard_height;

            // Compute the left hand, right hand, waist positions w.r.t the head.
            // Note that they're vectors in the head's space.
            // Normalized with the height of the character. 
            Data.Instance.L_position = head.InverseTransformDirection(left.position - head.position) / standard_height;
            Data.Instance.R_position = head.InverseTransformDirection(right.position - head.position) / standard_height; 
            Data.Instance.W_position = head.InverseTransformDirection(waist.position - head.position) / standard_height;

            // Getting the rotation of the head in quaternion. 
            Data.Instance.H_rotation = head.rotation;        

            // Getting the rotation of left, right hand and waist. 
            // They are all relative to the head's rotation. 
            Data.Instance.L_rotation = Quaternion.Inverse(left_init_rotation) * Quaternion.Inverse(head.rotation) *  left.rotation;
            Data.Instance.R_rotation = Quaternion.Inverse(right_init_rotation) * Quaternion.Inverse(head.rotation) *  right.rotation;
            Data.Instance.W_rotation = Quaternion.Inverse(head.rotation) * waist.rotation;


            
            // Write the currently collected data to output:
            output.Append("\n");
            output.Append(Data.Instance.H_height.ToString() + ",");
            output.Append(Data.Instance.L_position.x.ToString() + "," + Data.Instance.L_position.y.ToString() + "," + Data.Instance.L_position.z.ToString() + ",");
            output.Append(Data.Instance.R_position.x.ToString() + "," + Data.Instance.R_position.y.ToString() + "," + Data.Instance.R_position.z.ToString() + ",");
            output.Append(Data.Instance.W_position.x.ToString() + "," + Data.Instance.W_position.y.ToString() + "," + Data.Instance.W_position.z.ToString() + ",");
            output.Append(Data.Instance.H_rotation.x.ToString() + "," + Data.Instance.H_rotation.y.ToString() + "," + Data.Instance.H_rotation.z.ToString() + "," + Data.Instance.H_rotation.w.ToString() + ",");
            output.Append(Data.Instance.L_rotation.x.ToString() + "," + Data.Instance.L_rotation.y.ToString() + "," + Data.Instance.L_rotation.z.ToString() + "," + Data.Instance.L_rotation.w.ToString() + ",");
            output.Append(Data.Instance.R_rotation.x.ToString() + "," + Data.Instance.R_rotation.y.ToString() + "," + Data.Instance.R_rotation.z.ToString() + "," + Data.Instance.R_rotation.w.ToString() + ",");
            output.Append(Data.Instance.W_rotation.x.ToString() + "," + Data.Instance.W_rotation.y.ToString() + "," + Data.Instance.W_rotation.z.ToString() + "," + Data.Instance.W_rotation.w.ToString() + ",");
            output.Append(Data.Instance.timestamp.ToString() + ",");
            output.Append(Data.Instance.animation_count.ToString());

            var filePath = "data.csv";

            using(var writer = new StreamWriter(filePath, false)){
                writer.Write(output);
            }
        }
    }

    public static void WriteToFile()
    {
        var filePath = "data.csv";

        using(var writer = new StreamWriter(filePath, false)){
            writer.Write(output);
        }
    }


}
