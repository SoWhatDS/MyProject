using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShader : MonoBehaviour
{
    Material material;
    private void Start()
    {
        material.SetColor("_Color", Color.white);
        float height = material.GetFloat("_MixValue");
        height = 0.5f;
    }
}
