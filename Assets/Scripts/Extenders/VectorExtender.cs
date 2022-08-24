using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace JuanPayan.Extenders
{
    public static class VectorExtender
    {

        public static Vector3Int ToVector(this int3 int3)
        {
            return new Vector3Int(int3.x, int3.y, int3.z);
        }//Close ToVector method

        public static int3 ToInt3(this Vector3Int vector3Int)
        {
            return new int3(vector3Int.x, vector3Int.y, vector3Int.z);
        }//Close ToInt3 method


        public static Vector3 ToVector(this float3 float3)
        {
            return new Vector3(float3.x, float3.y, float3.z);
        }//Close ToVector method

        public static float3 ToFloat3(this Vector3 vector3)
        {
            return new float3(vector3.x, vector3.y, vector3.z);
        }//Close ToFloat3 method



    }//Closes VectorExtender class
}//Closes Namespace declaration

