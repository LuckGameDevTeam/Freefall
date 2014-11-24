using UnityEngine;
using System.Collections;

/// <summary>
/// UI health display.
/// 
/// This health display control all health blocks
/// </summary>
public class UIHealthDisplay : MonoBehaviour 
{
	/// <summary>
	/// The health blocks.
	/// </summary>
	public Transform[] healthBlocks;

	// Use this for initialization
	void Start () 
	{
		//sort health block by x position...left to right
		if(healthBlocks.Length > 0)
		{
			//sort
			System.Array.Sort(healthBlocks, (block1, block2) => {
				
				Transform b1 = block1;
				Transform b2 = block2;
				
				if(b1.position.x > b2.position.x)
				{
					return 1;
				}
				else if(b1.position.x < b2.position.x)
				{
					return -1;
				}
				else
				{
					return 0;
				}
			});
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	/// <summary>
	/// Updates the health dispaly.
	/// </summary>
	/// <param name="health">Health.</param>
	public void UpdateHealthDispaly(float health)
	{
		//clear each health block
		for(int i=0; i<healthBlocks.Length; i++)
		{
			UIHPDisplay display = healthBlocks[i].GetComponent<UIHPDisplay>();

			display.UpdateHP(0f);
		}

		//get decemal digital number
		int digital = (int)health;

		//get number after float point
		float floatPoint = health - (float)digital;

		if(digital>healthBlocks.Length)
		{
			return;
		}

		//update health block with digital number
		for(int j =0; j<digital; j++)
		{
			UIHPDisplay display = healthBlocks[j].GetComponent<UIHPDisplay>();

			display.UpdateHP(1);
		}

		//update health block with float number
		if(floatPoint > 0f)
		{
			UIHPDisplay display = healthBlocks[digital].GetComponent<UIHPDisplay>();

			display.UpdateHP(floatPoint);
		}
	}

	/// <summary>
	/// Reset.
	/// </summary>
	public void Reset()
	{
		for(int i=0; i<healthBlocks.Length; i++)
		{
			UIHPDisplay display = healthBlocks[i].GetComponent<UIHPDisplay>();
			
			display.UpdateHP(1f);
		}
	}
}
