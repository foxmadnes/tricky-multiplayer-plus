using System;
using UnityEngine;

public class MistyWindForeground : AbstractBackground
{
	public MistyWindForeground(ZoomableCamera zoomableCamera) : base(zoomableCamera, "Foreground", "ForegroundCamera", "ForegroundContainer")
	{
		this._camera.clearFlags = CameraClearFlags.Depth;
		this._camera.depth = 90f;
		this._topRight = this._camera.ViewportToWorldPoint(new Vector3(1f, 0.1f, this._camera.nearClipPlane));
	}

	public void SetGameModel(GameModel gameModel)
	{
		this._windDirection = gameModel.GetDataModel<DataModelInt>("RANDOM_WIND_DIR").value;
		if (this._rainLayer != null && this._windZoneLayer != null)
		{
			this._rainLayer.SetWindDirection(this._windDirection);
			this._windZoneLayer.SetWindDirection(this._windDirection);
		}

		this._lowestTowerModel = gameModel.GetDataModel<DataModelFloat>("LOWEST_TOWER");
	}

	public override void Update()
	{
		float num = float.MaxValue;
		if (base.zoomableCamera != null && this._lowestTowerModel != null)
		{
			num = this._camera.ViewportToWorldPoint(base.zoomableCamera.camera.WorldToViewportPoint(new Vector3(0f, this._lowestTowerModel.value - 8f, 0f))).y;
		}
		Vector3 bottomLeft = this._camera.ViewportToWorldPoint(new Vector3(0f, 0f, this._camera.nearClipPlane));
		Vector3 vector = this._camera.ViewportToWorldPoint(new Vector3(1f, 0.1f, this._camera.nearClipPlane));
		float num2 = vector.y;
		if (num2 > num && num > bottomLeft.y)
		{
			num2 = num;
		}
		this._topRight.y = Mathf.Lerp(this._topRight.y, num2, 1.5f * Time.deltaTime);
		this._topRight.x = vector.x;
		this._mistLayer.UpdateSizeAndPosition(bottomLeft, this._topRight);
		base.Update();
	}

	protected override void _AddLayers() {
		this._rainLayer = new RainLayer("RAIN_PARTICLES", "RainForeground", 0f);
		base._AddLayer(this._rainLayer);
		this._windZoneLayer = new WindZoneLayerDuplicate("RainForeground", 0f);
		base._AddLayer(this._windZoneLayer);
		this._mistLayer = new MistLayer("Magic", 1f, new Color(0.8901961f, 0.8392157f, 0.9647059f), 0f, 1f);
		base._AddLayer(this._mistLayer);
	}

	private WindZoneLayerDuplicate _windZoneLayer;

	private RainLayer _rainLayer;

	private MistLayer _mistLayer;

	private int _windDirection = 1;

	private DataModelFloat _lowestTowerModel;

	private Vector3 _topRight;
}
