/*  This file is part of the "Simple IAP System" project by Rebound Games.
 *  You are only allowed to use these resources if you've bought them directly or indirectly
 *  from Rebound Games. You shall not license, sublicense, sell, resell, transfer, assign,
 *  distribute or otherwise make available to any third party the Service or the Content. 
 */

using UnityEngine;
using System.Collections;
using OnePF;

namespace SIS
{
	/// <summary>
	/// OpenIAB cross-platform IAP product wrapper class
	/// </summary>
	public class IAPArticle
	{
		/// <summary>
		/// product id
		/// </summary>
		public string id;

		/// <summary>
		/// product title
		/// </summary>
		public string title;

		/// <summary>
		/// product description
		/// </summary>
		public string description;

		/// <summary>
		/// product price
		/// </summary>
		public string price;


		/// <summary>
		/// create new instance
		/// </summary>
		public IAPArticle(SkuDetails prod)
		{
			id = prod.Sku;
			title = prod.Title;
			description = prod.Description;
			price = prod.Price;
		}
	}
}