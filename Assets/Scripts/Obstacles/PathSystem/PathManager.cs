using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathManager : MonoBehaviour 
{
	/// <summary>
	/// The paths.
	/// 
	/// This contain each begin of path;
	/// 
	/// Each path has unique prefix that will be used to distinguish each path.
	/// 
	/// Each path contain one to more PathPoint and 
	/// each PathPoints chain together like linklist
	/// 
	/// Manager only hold the beginning of PathPoint for each path
	/// </summary>
	Dictionary<string, PathPoint> paths;

	List<string> pathPrefixs;


	void Awake()
	{
		if(transform.childCount <= 0)
		{
			Debug.LogError("You must attached at least one child PathPoint to PathManager");
			
			return;
		}
		
		paths = new Dictionary<string, PathPoint> ();

		//find all path prefix
		pathPrefixs = FindAllPathPrefix ();

		//Check if paths has any error
		if(AnalyzePaths() == false)
		{
			Debug.LogError("There is an error in path");
		}
		
		//find out first PathPoint in each path
		for(int i=0; i<pathPrefixs.Count; i++)
		{
			//get current path prefix
			string cPathPrefix = pathPrefixs[i];
			
			//find first path point in child
			for(int j=0; j<transform.childCount; j++)
			{
				Transform child = transform.GetChild(j);
				
				PathPoint p = child.GetComponent<PathPoint>();
				
				//if it is first path point
				if((p.PathPrefix == cPathPrefix) && (p.lastPoint == null))
				{
					//found it add to paths
					paths.Add(cPathPrefix, p);

					break;
				}
			}
		}
	}

	/// <summary>
	/// Analyzes the paths.
	/// </summary>
	/// <returns><c>true</c>, if paths was built without error, <c>false</c> there is an error.</returns>
	bool AnalyzePaths()
	{
		if(pathPrefixs != null)
		{
			for(int i=0; i<pathPrefixs.Count; i++)
			{
				//get current path prefix
				string cPathPrefix = pathPrefixs[i];

				int pathPointCount = 0;

				//count path point in child
				for(int j=0; j<transform.childCount; j++)
				{
					Transform child = transform.GetChild(j);
					
					PathPoint p = child.GetComponent<PathPoint>();
					

					if(p.PathPrefix == cPathPrefix)
					{
						pathPointCount++;
					}
				}

				if(pathPointCount < 2)
				{
					Debug.LogError("Path with prefix: " + cPathPrefix + " has PathPoint less then 2, it must be at least 2 PathPoint");

					return false;
				}

				//if PathPoint in path is odd number 
				if((pathPointCount % 2) != 0)
				{
					Debug.LogError("Path with prefix: " + cPathPrefix + " has odd number of PathPoint, it must be event number");

					return false;
				}
			}

			return true;
		}

		return false;
	}

	/// <summary>
	/// Finds all path prefix.
	/// </summary>
	/// <returns>The all path prefix.</returns>
	List<string> FindAllPathPrefix()
	{
		List<string> retPathPrefix = new List<string>();

		for(int i=0; i<transform.childCount; i++)
		{
			Transform child = transform.GetChild(i);

			PathPoint p = child.GetComponent<PathPoint>();

			if(p != null)
			{
				//if path prefix does not in container...add it
				if(!retPathPrefix.Contains(p.PathPrefix))
				{
					retPathPrefix.Add(p.PathPrefix);
				}
			}
			else
			{
				Debug.LogError("There is child without PathPoint component in PathManager");
			}
		}

		return retPathPrefix;
	}

	/// <summary>
	/// Gets the first path point.
	/// </summary>
	/// <returns>The first path point.</returns>
	/// <param name="ThePathPrefix">The path prefix.</param>
	public PathPoint GetFirstPathPoint(string ThePathPrefix)
	{
		return paths [ThePathPrefix];
	}

	/// <summary>
	/// Gets the last path point.
	/// 
	/// Instead return last path point, it return the one that is before last point
	/// e.g 6 PathPoint and it will reutrn number 5 as last point
	/// </summary>
	/// <returns>The last path point.</returns>
	/// <param name="ThePathPrefix">The path prefix.</param>
	public PathPoint GetLastPathPoint(string ThePathPrefix)
	{
		//get first point
		PathPoint currentPoint = paths [ThePathPrefix];

		while(true)
		{
			//get next path point
			PathPoint next = currentPoint.nextPoint;

			//check if next path point is last in path...return current path point
			if((next.nextPoint == null) && (next.lastPoint == currentPoint))
			{
				return currentPoint;
			}

			//set next point as current 
			currentPoint = currentPoint.nextPoint;
		}
	}
}
