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
    public static bool is_first_frame = true;

    // Constant values
    private float standard_height = 0;
    private Quaternion left_init_rotation;
    private Quaternion right_init_rotation;
    private StringBuilder output;

    private int counter = 0;

    void Awake(){
        Data.Instance = new Data();
        Data.Cache = new Data();
    }

    void Start()
    {
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
        // velocities of the four parts:
        output.Append("h_v,");
        output.Append("l_vx, l_vy, l_vz,");
        output.Append("r_vx, r_vy, r_vz,");
        output.Append("w_vx, w_vy, w_vz,");

    }



    // Update is called once per frame
    void Update()
    {
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
        Data.Instance.L_rotation = Quaternion.Inverse(head.rotation) * Quaternion.Inverse(left_init_rotation) * left.rotation;
        Data.Instance.R_rotation = Quaternion.Inverse(head.rotation) * Quaternion.Inverse(right_init_rotation) * right.rotation;
        Data.Instance.W_rotation = Quaternion.Inverse(head.rotation) * waist.rotation;


        if (!is_first_frame){
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

            // Collect velocities: 
            Data.Instance.H_velocity = (Data.Instance.H_height - Data.Cache.H_height) / Time.deltaTime;
            Data.Instance.L_velocity = (Data.Instance.L_position - Data.Cache.L_position) / Time.deltaTime;
            Data.Instance.R_velocity = (Data.Instance.R_position - Data.Cache.R_position) / Time.deltaTime;
            Data.Instance.W_velocity = (Data.Instance.W_position - Data.Cache.W_position) / Time.deltaTime;
            output.Append(Data.Instance.H_velocity.ToString() + ",");
            output.Append(Data.Instance.L_velocity.x.ToString() + "," + Data.Instance.L_velocity.y.ToString() + "," + Data.Instance.L_velocity.z.ToString() + ",");
            output.Append(Data.Instance.R_velocity.x.ToString() + "," + Data.Instance.R_velocity.y.ToString() + "," + Data.Instance.R_velocity.z.ToString() + ",");
            output.Append(Data.Instance.W_velocity.x.ToString() + "," + Data.Instance.W_velocity.y.ToString() + "," + Data.Instance.W_velocity.z.ToString() + ",");

            Data.Cache.H_velocity = Data.Instance.H_velocity;
            Data.Cache.L_velocity = Data.Instance.L_velocity;
            Data.Cache.R_velocity = Data.Instance.R_velocity;
            Data.Cache.W_velocity = Data.Instance.W_velocity;
        }

        // Saving data to cache (for using in the next frame)
        Data.Cache.H_height = Data.Instance.H_height;
        Data.Cache.L_position = Data.Instance.L_position;
        Data.Cache.R_position = Data.Instance.R_position;
        Data.Cache.W_position = Data.Instance.W_position;
        Data.Cache.H_rotation = Data.Instance.H_rotation;
        Data.Cache.L_rotation = Data.Instance.L_rotation;
        Data.Cache.R_rotation = Data.Instance.R_rotation;
        Data.Cache.W_rotation = Data.Instance.W_rotation;
        
        // counter ++;
        int k = Random.Range(1, 100);
        if (k == 99){
            Debug.Log("Collected a frame");
            Debug.Log(counter);
            counter++;

            // var filePath = "data.csv";

            // using(var writer = new StreamWriter(filePath, false)){
            //     writer.Write(output);
            // }

            using(var writer = new StreamWriter("counting.txt", false)){
                writer.Write(counter.ToString());
            }
        }

        is_first_frame = false;


    }



}
