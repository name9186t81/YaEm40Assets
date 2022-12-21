using UnityEngine.UI;
using UnityEngine;

public class ZombieModInfoDisplayer : MonoBehaviour
{
	[SerializeField] private Text _waveDisplayer;
	[SerializeField] private Text _zombieCountDisplayer;
	[SerializeField] private ZombieModeCycle _mod;
	[SerializeField] private GameObject _activateOnLoose;
	[SerializeField] private GameObject _activateOnWin;
	private void Awake(){
		_waveDisplayer.text = "1/" + _mod.MaxWaves;
		_mod.OnWaveChange += UpdateWaveText;
		_mod.OnWin += () => _activateOnWin?.SetActive(true);
		_mod.OnLoose += () => _activateOnLoose?.SetActive(true);
	}
	
	private void Update(){
		_zombieCountDisplayer.text = $"{_mod.SpawnedCount}/{_mod.SpawnedInThisWave}";
	}
	
	private void UpdateWaveText((int zombiesCount, int waveIndex) arg){
		_waveDisplayer.text = arg.waveIndex + 1 + "/" + (_mod.MaxWaves);
	}
}
