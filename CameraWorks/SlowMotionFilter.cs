using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotionFilter : MonoBehaviour
{
	[SerializeField] private Shader _shader;
	[SerializeField] private Material _material;
	[SerializeField] private CameraUnitAttacher _attacher;
	private Unit _attachedUnit;
	private Color _assigned;
	private bool _isActive = false;
	
	private void Awake(){
		if(ServiceLocator.TryGetService<SlowMotionNotification>(out var notificator)){
			notificator.OnSlowmotionStarted += (float f) => _isActive = true;
			notificator.OnSlowmotionEnded +=  (float f) => _isActive = false;
		}
		else{
			ServiceLocator.AddService(new SlowMotionNotification());
			if(ServiceLocator.TryGetService<SlowMotionNotification>(out var notificator2)){
				notificator2.OnSlowmotionStarted += (float f) => _isActive = true;
				notificator2.OnSlowmotionEnded +=  (float f) => _isActive = false;
			}
			
		}
		if(_material == null){
		_material = new Material(_shader);
		}
		
		_attachedUnit = _attacher.Attached;
		_attacher.OnUnitChange += (Unit unit) => _attachedUnit = unit;
	}
	
	private void OnRenderImage(RenderTexture src, RenderTexture dst){
		if(_isActive){
			_material.SetFloat("_Strength", -0.5f);
		}
		else{
			_material.SetFloat("_Strength", 0f);
		}
		if(_attachedUnit != null){
		_material.SetColor("_Color", TeamColorData.GetTeamColor(_attachedUnit.teamNumber));
		}
		Graphics.Blit(src, dst, _material);
	}
}
