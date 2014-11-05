using UnityEngine;
using System.Collections;

public enum TypeOfGame
{
	Unknow,
	SinglePlayer,
	FB
}

public class GameType : MonoBehaviour 
{
	/// <summary>
	/// which type of game player is playing
	/// </summary>
	public TypeOfGame currentGameType = TypeOfGame.Unknow;
}
