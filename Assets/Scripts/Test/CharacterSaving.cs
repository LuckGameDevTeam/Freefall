using UnityEngine;
using System.Collections;

public class CharacterSaving : MonoBehaviour 
{

	public void SaveCharacter(GameObject button)
	{
		UILabel lable = button.GetComponentInChildren<UILabel>();

		PlayerCharacter pc = new PlayerCharacter ();
		pc.characterName = lable.text;

		SaveLoadManager.SharedManager.Save (pc);

		LoadTestLevel ();
	}

	void LoadTestLevel()
	{
		Application.LoadLevel ("TestField");
	}
}
