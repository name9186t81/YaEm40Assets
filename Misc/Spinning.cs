using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinning : MonoBehaviour
{
	[SerializeField] private Transform _transform;
	[SerializeField] private float _speed;
	
	private void Update(){
		_transform.rotation = Quaternion.Euler(0, 0, _transform.eulerAngles.z + _speed);
	}
}
