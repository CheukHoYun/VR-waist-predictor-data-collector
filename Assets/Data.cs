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

    // Note that H_velocity is actually the rate of change in its height,
    // that's why it's a float
    public float H_velocity;
    public Vector3 L_velocity;
    public Vector3 R_velocity;
    public Vector3 W_velocity;
    public Quaternion H_angular_velocity;
    public Quaternion L_angular_velocity;
    public Quaternion R_angular_velocity;
    public Quaternion W_angular_velocity;


    
}
