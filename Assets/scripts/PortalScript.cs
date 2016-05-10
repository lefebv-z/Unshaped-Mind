using UnityEngine;
using System.Collections;

public class PortalScript : MonoBehaviour {
	public ShapeType type;
	public GameObject exitPortal;
	bool _isActive = true;
	GameObject _playerShape;
	GameManager _gameManager;
	float _fadeTime = 0.25f;
	float _currentFade;
	bool _fadeIn = false;
	bool _fadeOut = false;
	Vector3 _scale;

	void Start() {
		_gameManager = GameObject.FindObjectOfType<GameManager>();
		_scale = new Vector3(0.5f, 0.5f, 0.5f);
	}
	
	public void UsePortal(GameObject playerShape)
	{
		if (_isActive == true)
		{
			Debug.Log("Portal used:" + name);
			_playerShape = playerShape;
			_gameManager.gameState = GameState.PortalAnimation;
			_currentFade = 0.0f;
			_fadeIn = true;
			exitPortal.GetComponent<PortalScript>().LockPortal();//Temporary lock the portal to prevent infinite teleportation
		}
	}

	void Update() {
		if (_isActive) {
			if (_fadeIn) {
				_currentFade += Time.deltaTime;
				_playerShape.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f) - _currentFade * 10 * _scale;
				if (_currentFade >= _fadeTime) {
					_fadeIn = false;
					_fadeOut = true;
					_currentFade = 0.0f;
					_playerShape.transform.position = exitPortal.transform.position;
				}
			} else if (_fadeOut) {
				_currentFade += Time.deltaTime;
				_playerShape.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f) + _currentFade * _scale;
				Debug.Log(_currentFade);
				if (_currentFade >= _fadeTime) {
					_fadeOut = false;
					_playerShape.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
					_gameManager.gameState = GameState.Playing;
				}
			}
		}
	}

	public void UnlockPortal()
	{
		GetComponent<SpriteRenderer>().color = Color.white;
		_isActive = true;
	}
	public void LockPortal()
	{
		SoundManager sm = (SoundManager)(GameObject.FindObjectOfType(typeof(SoundManager)));
		if (sm != null)
			sm.PlayUnlocking();
		GetComponent<SpriteRenderer>().color = Color.gray;
		_isActive = false;
	}
}
