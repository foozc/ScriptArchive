﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AddLighingMapNew : MonoBehaviour {

	public Color color;
	//public Color lightingColor;
	public string shaderName = "Legacy Shaders/Lightmapped/Diffuse";
    public string bakeTextureSuffix = "VRayLightingMap";

    public TpyeShader seletionChangerType;
	public enum TpyeShader
	{
		onlyChangeShader,
		addLightMamp,
	}

	void Start()
	{
		AddLightingMap();
		Self_Illumin_Diffuse();
	}


	void AddLightingMap()
	{
		if( seletionChangerType.ToString() == "addLightMamp" )
		{
			//Transform[] childs ;
			foreach( Transform child in GetComponentsInChildren<Transform>() )
			{
				try
				{
					for (int i = 0; i < child.GetComponent<Renderer>().sharedMaterials.Length; i++)
					{
                        Shader shader = Shader.Find(shaderName);
                        child.GetComponent<Renderer>().sharedMaterials[i].shader = Shader.Find(shaderName);
						child.GetComponent<Renderer>().sharedMaterials[i].color = color;
						Texture tex =(Texture)Resources.Load("hongpei/" + child.transform.name + bakeTextureSuffix);
						child.GetComponent<Renderer>().sharedMaterials[i].SetTexture("_LightMap", tex);
                        //child.GetComponent<Renderer>().sharedMaterials[i].SetColor("_lightmap_color", lightingColor);
					}
					child.GetComponent<Renderer>().sharedMaterial.shader = Shader.Find(shaderName);
				}
				catch
				{
				}
			}
		}
	}


	void Self_Illumin_Diffuse()
	{
		if( seletionChangerType.ToString() == "onlyChangeShader" )
		{
			//Transform[] childs ;
			foreach( Transform child in GetComponentsInChildren<Transform>() )
			{
				try
				{
					child.GetComponent<Renderer>().sharedMaterial.shader = Shader.Find(shaderName);
				}
				catch
				{
				}
			}
		}
	} 
}
