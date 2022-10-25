using System;
using UnityEngine;

internal class WindZoneLayerDuplicate : BackgroundLayer
{
	// This class recreated due to visibility
	public WindZoneLayerDuplicate(string sortingLayer, float parallaxMultiplier) : base(sortingLayer, parallaxMultiplier)
	{
		this._wind = Singleton<ResourceManager>.instance.InstantiateByName("WIND", new Vector3(-41f, 0f, 0f), this._layerContainer);
		this._wind.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
		this.AddGameObject(this._wind, -1);
	}

	public void SetWindDirection(int direction)
	{
		if (direction == -1)
		{
			this._wind.transform.rotation = Quaternion.Euler(0f, 270f, 0f);
		}
		else
		{
			this._wind.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
		}
	}

	private GameObject _wind;
}
