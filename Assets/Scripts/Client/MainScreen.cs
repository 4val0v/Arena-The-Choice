using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class MainScreen : MonoBehaviour 
{
	void Awake()
	{
		_names = new List<string> (new string[]{"Heird",
			"Astaydda",
			"Somatram",
			"Oleallan",
			"Etohaldan",
			"Ulelallan",
			"Eloreand",
			"Chumwen",
			"Adorewan",
			"Wigode",
			"Yalisean",
			"Soemeth",
			"Onirat",
			"Galaun",
			"Kedaejar",
			"Eowoania",
			"Toaseth",
			"Maodda",
			"Laerd",
			"Dwoidda",
			"Qyc",
			"Gworedia",
			"Cadeliwyr",
			"Kedaeniel",
			"Adrien",
			"Qirelia",
			"Adayr",
			"Aralimeth",
			"Vadan",
			"Adwiedda",
			"Etiragan",
			"Ulardolle",
			"Gwohavudd",
			"Aroewia",
			"Powyr",
			"Olewia",
			"Frilich",
			"Faondra",
			"Wohash",
			"Grassa",
			"Unomadon",
			"Mirendra",
			"Onoanydd",
			"Crelarien",
			"Crelith",
			"Grendavia",
			"Firahar",
			"Afauclya",
			"Piwyr",
			"Brucan",
			"Cadoreswen",
			"Qatram",
			"Thondra",
			"Ibilip",
			"Uliladia",
			"Weil",
			"Wicewen",
			"Asoreron",
			"Crathien",
			"Arendar",
			"Crilameth",
			"Abeseth",
			"Adreranna"});
	}

	void OnEnable()
	{
		if(!_isChangeName)
		{
			_inputField.text = _names[Random.Range(0, _names.Count)];
		}
	}

	public void OnSelectField()
	{
		_isChangeName = true;
	}

	public string NameOfPlayer 
	{
		get 
		{
			return _textInField.text;
		}
	}

	private bool _isChangeName;
	private List<string> _names;

	[SerializeField]
	private Text _textInField;
	[SerializeField]
	private InputField _inputField;
}
