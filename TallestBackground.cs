using System;
using System.Collections.Generic;
using UnityEngine;

public class TallestBackground : AbstractBackground
{
	public TallestBackground(ZoomableCamera zoomableCamera) : base(zoomableCamera, "Background", "BackgroundCamera", "BackgroundContainer")
	{
	}

	protected override void _AddLayers()
	{
		base._AddLayer(new GradientLayer(this._camera, new Color32(102, 142, 171, 255), new Color32(221, 229, 244, 255), new Color32(177, 196, 216, 255)));
		SurvivalBackgroundIslandsLayer islandsLayer = new SurvivalBackgroundIslandsLayer(this._camera, "BackgroundLandscape", 0.25f);
		islandsLayer.ChangeColour(Color.white);
		base._AddLayer(islandsLayer);
		SurvivalForegroundIslandsLayer foregroundIslandsLayer = new SurvivalForegroundIslandsLayer(this._camera, "BackgroundLandscapeFront", 0.35f);
		foregroundIslandsLayer.ChangeColour(Color.white);
		base._AddLayer(foregroundIslandsLayer);
		string[] cloudNames3 = new string[]
		{
			"RACE_CLOUD7",
			"RACE_CLOUD8",
			"RACE_CLOUD9",
			"RACE_CLOUD10"
		};
		base._AddLayer(new CloudLayer(cloudNames3, 0.4f, 0.5f, 0.5f, 2f, this._camera, "Clouds2", 0.8f, new Color32(177, 196, 216, 255)));

		MistLayer mistLayer = new MistLayer("BackgroundMistFront", 1f, new Color32(221, 229, 244, 255), 0.1f, 0.8f);
		base._AddLayer(mistLayer);
		this._mistLayers.Add(mistLayer);
		MistLayer mistLayer2 = new MistLayer("BackgroundMistMid", 1f, new Color32(177, 196, 216, 255), 0.5f, 0.6f);
		base._AddLayer(mistLayer2);
		this._mistLayers.Add(mistLayer2);
		MistLayer mistLayer3 = new MistLayer("Clouds1", 1f, Color.white, 0.3f, 0.4f);
		base._AddLayer(mistLayer3);
		this._mistLayers.Add(mistLayer3);

	}

	public override void Update()
	{
		Vector3 bottomLeft = this._camera.ViewportToWorldPoint(new Vector3(0f, 0f, this._camera.nearClipPlane));
		for (int i = 0; i < this._mistLayers.Count; i++)
		{
			Vector3 topRight = this._camera.ViewportToWorldPoint(new Vector3(1f, 1f * (0.14f + (float)i * 0.05f), this._camera.nearClipPlane));
			this._mistLayers[i].UpdateSizeAndPosition(bottomLeft, topRight);
		}
		base.Update();
	}

	private List<MistLayer> _mistLayers = new List<MistLayer>();
}
