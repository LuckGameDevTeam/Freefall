using UnityEngine;
using System.Collections;

/// <summary>
/// Main menu controller.
/// 
/// This class control main menu
/// Mainly it just load character
/// </summary>
public class MainMenuController : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		LoadCharacter ().GetComponent<CharacterControl>().Evt_CharacterStart += OnCharacterStart;

		//we tell level selection to present main level selection on presenting
		GameObject.FindGameObjectWithTag (Tags.levelSelection).GetComponent<LevelSelection> ().SetMainLevelSelected (0);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	GameObject LoadCharacter()
	{
		string[] characters = StoreAssets.GetAllCharactersId ();

		string pickedId = characters[Random.Range (0, characters.Length)];

		Object[] characterAssets = Resources.LoadAll("Characters", typeof(GameObject));

		if(characterAssets.Length > 0)
		{
			for(int i = 0; i<characterAssets.Length; i++)
			{
				if(pickedId == characterAssets[i].name)
				{
					GameObject cPrefab = (GameObject)characterAssets[i];
					GameObject retCharacter = Instantiate(cPrefab) as GameObject;
					
					retCharacter.name = cPrefab.name;

					//tell character that it is not in game
					retCharacter.GetComponent<CharacterControl>().isInGame = false;
					
					return retCharacter;
				}
			}
		}
		else
		{
			Debug.LogError("No character assets in Resources folder");
			
			return null;
		
		}

		return null;
	}

	void OnCharacterStart(CharacterControl chaControl)
	{
		(GameObject.FindObjectOfType (typeof(MusicManager)) as MusicManager).PlayMusic ();
	}
}
