using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data
{
    public static Data Instance; 
    public static Data Cache;
    public float H_height;
    public Vector3 H_position;
    public Vector3 L_position;
    public Vector3 R_position;
    public Vector3 W_position;
    public Quaternion H_rotation;
    public Quaternion L_rotation;
    public Quaternion R_rotation;
    public Quaternion W_rotation;
    public float timestamp = 0.0f;
    public int animation_count = 0;

    
}
