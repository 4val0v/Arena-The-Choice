using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class SelectCharacter : MonoBehaviour {

	void OnEnable()
	{
		_characterIndex = -1;
	}

	public int CharacterIndex 
	{
		set
		{
			if (_characterIndex == -1)
			{
				_getBtn.interactable = true;
			}
			_characterIndex = value;
			for (int i = 0; i < _buttons.Count; i++) 
			{
				if (i == value)
				{
					_buttons[i].image.sprite = _spriteOfSelectedItem;
				}
				else
				{
					_buttons[i].image.sprite = _spriteOfstandartPanel;
				}
			}
		}
		get
		{
			return _characterIndex;
		}
	}

	public void GetIt()
	{
		if (OnCharacterSelected != null)
		{
			OnCharacterSelected.Invoke(CharacterIndex);
		}
	}

	[SerializeField]
	private List<Button> _buttons;
	private int _characterIndex = -1;
	public Action<int> OnCharacterSelected;

	[SerializeField]
	private Button _getBtn;
	[SerializeField]
	private Sprite _spriteOfSelectedItem;
	[SerializeField]
	private Sprite _spriteOfstandartPanel;
}
