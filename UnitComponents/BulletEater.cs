using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEater : UnitComponent
{
	[SerializeField] private int _maxBullets;
	[SerializeField] private int _damagePerBullet;
	[SerializeField] private Projectile _bullet;
	private int _bulletsCount;
	
    protected override void AddToComponentSystem()
    {
        Owner.ComponentSystem.AddComponent(this);
		Owner.Health.OnDamage += Damaged;
		Owner.Health.OnDeath += Death;
    }
	
	private void Death(DamageArgs args){
		_bulletsCount += args.Damage % _damagePerBullet;
		
		_bulletsCount = Mathf.Clamp(_bulletsCount, 0, _maxBullets);
		
		for(int i = 0; i < _bulletsCount; i++){
			float randomAngle = Random.Range(0, 360);
			
			var bullet = Instantiate(_bullet, Position2D, Quaternion.Euler(0, 0, randomAngle), null);
			bullet.Init(null, new DamageArgs(_damagePerBullet, Owner), Position2D, 1f);
		}
	}
	
	private void Damaged(DamageArgs args){
		_bulletsCount += args.Damage % _damagePerBullet;
	}
}
