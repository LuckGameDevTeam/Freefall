using UnityEngine;
using System.Collections;
using System;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using SIS;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum AdStyle
{
	Banner,
	Interstitial,
	Unknow
}

public class AdControl : MonoBehaviour 
{
	public delegate void EventOnAdLoaded(AdControl control, object sender, EventArgs args);
	/// <summary>
	/// Event when ad successful loaded
	/// </summary>
	public EventOnAdLoaded Evt_OnAdLoaded;

	public delegate void EventOnAdFailToLoad(AdControl control, object sender, string message);
	/// <summary>
	/// Event when ad fail on loaded
	/// </summary>
	public EventOnAdFailToLoad Evt_OnAdFailToLoad;
	
	public delegate void EventOnNoAdToLoad(AdControl control);
	/// <summary>
	/// Ad Control detect there is no add should be created
	/// </summary>
	public EventOnNoAdToLoad Evt_OnNoAdToLoad;

	/// <summary>
	/// The style of ad.
	/// </summary>
	public AdStyle style = AdStyle.Banner;

	/// <summary>
	/// The create on start.
	/// </summary>
	public bool createOnStart = true;

	/// <summary>
	/// The ad unit identifier.
	/// </summary>
	public string adUnitId = "Ad Unit Id Here";

	private BannerView bannerView;
	private InterstitialAd interstitial;

	void OnDisable()
	{
		if(bannerView != null)
		{
			bannerView.Destroy();
		}

		if(interstitial != null)
		{
			interstitial.Destroy();
		}
	}

	// Use this for initialization
	void Start () 
	{
		if(createOnStart)
		{
			CreateAd (style);
		}

	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}



	public void CreateAd(AdStyle adStyle)
	{
#if TestMode
		DebugEx.DebugError("TestMode do not create ad");
		return;
#endif
		//if(!StoreInventory.NonConsumableItemExists(StoreAssets.UNLOCK_ALL_LEVEL_NO_AD_ITEM_ID))
		if(!DBManager.isPurchased("CC_BuyFullGame"))
		{
			if(adStyle == AdStyle.Banner)
			{
				CreateBannerAd();
			}
			else if(adStyle == AdStyle.Interstitial)
			{
				CreateInterstitialAd();
			}
			else
			{
				print("No ad should be created");

				//no ad should be create
				if(Evt_OnNoAdToLoad != null)
				{
					Evt_OnNoAdToLoad(this);
				}
			}
		}
		else
		{
			print("No ad should be created");

			//no ad should be create
			if(Evt_OnNoAdToLoad != null)
			{
				Evt_OnNoAdToLoad(this);
			}
		}
	}

	/// <summary>
	/// Creates the banner ad.
	/// </summary>
	void CreateBannerAd()
	{
		if(bannerView != null)
		{
			bannerView.Destroy();
			bannerView = null;
		}
		
		// Create a 320x50 banner at the top of the screen.
		bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom);
		// Register for ad events.
		bannerView.AdLoaded += HandleAdLoaded;
		bannerView.AdFailedToLoad += HandleAdFailedToLoad;
		bannerView.AdOpened += HandleAdOpened;
		bannerView.AdClosing += HandleAdClosing;
		bannerView.AdClosed += HandleAdClosed;
		bannerView.AdLeftApplication += HandleAdLeftApplication;
		// Load a banner ad.
		bannerView.LoadAd(createAdRequest());
	}

	/// <summary>
	/// Creates the interstitial ad.
	/// </summary>
	void CreateInterstitialAd()
	{
		if(interstitial != null)
		{
			interstitial.Destroy();
			interstitial = null;
		}

		// Create an interstitial.
		interstitial = new InterstitialAd(adUnitId);
		// Register for ad events.
		interstitial.AdLoaded += HandleInterstitialLoaded;
		interstitial.AdFailedToLoad += HandleInterstitialFailedToLoad;
		interstitial.AdOpened += HandleInterstitialOpened;
		interstitial.AdClosing += HandleInterstitialClosing;
		interstitial.AdClosed += HandleInterstitialClosed;
		interstitial.AdLeftApplication += HandleInterstitialLeftApplication;
		// Load an interstitial ad.
		interstitial.LoadAd(createAdRequest());
	}

	// Returns an ad request with custom ad targeting.
	private AdRequest createAdRequest()
	{
		return new AdRequest.Builder()
			//.AddTestDevice(AdRequest.TestDeviceSimulator)
			//.AddTestDevice("b9621b1a81aebcf675f35569b7348757")
			.AddKeyword("game")
			.SetGender(Gender.Male)
			.SetBirthday(new DateTime(1985, 1, 1))
			.TagForChildDirectedTreatment(false)
			.AddExtra("color_bg", "9B30FF")
			.Build();
		
	}
	
	private void ShowInterstitial()
	{
		if (interstitial.IsLoaded())
		{
			interstitial.Show();
		}
		else
		{
			print("Interstitial is not ready yet.");
		}
	}

	/// <summary>
	/// Shows the ad.
	/// </summary>
	public void ShowAd()
	{
		if(style == AdStyle.Banner)
		{
			bannerView.Show();
		}
		else if(style == AdStyle.Interstitial)
		{
			ShowInterstitial();
		}
	}

	/// <summary>
	/// Hides the ad.
	/// 
	/// If it is Interstitial ad it will destroy it
	/// </summary>
	public void HideAd()
	{
		if(style == AdStyle.Banner)
		{
			bannerView.Hide();
		}
		else if(style == AdStyle.Interstitial)
		{
			DestroyAd();
		}
	}

	/// <summary>
	/// Destroies the ad.
	/// </summary>
	public void DestroyAd()
	{
		if(style == AdStyle.Banner)
		{
			bannerView.Destroy();
			bannerView = null;
		}
		else if(style == AdStyle.Interstitial)
		{
			interstitial.Destroy();
			interstitial = null;
		}
	}

	#region Banner callback handlers
	
	public void HandleAdLoaded(object sender, EventArgs args)
	{
		print("HandleAdLoaded event received.");

		if(Evt_OnAdLoaded != null)
		{
			Evt_OnAdLoaded(this, sender, args);
		}
	}
	
	public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		print("HandleFailedToReceiveAd event received with message: " + args.Message);

		if(Evt_OnAdFailToLoad != null)
		{
			Evt_OnAdFailToLoad(this, sender, args.Message);
		}
	}
	
	public void HandleAdOpened(object sender, EventArgs args)
	{
		print("HandleAdOpened event received");
	}
	
	void HandleAdClosing(object sender, EventArgs args)
	{
		print("HandleAdClosing event received");
	}
	
	public void HandleAdClosed(object sender, EventArgs args)
	{
		print("HandleAdClosed event received");
	}
	
	public void HandleAdLeftApplication(object sender, EventArgs args)
	{
		print("HandleAdLeftApplication event received");
	}
	
	#endregion
	
	#region Interstitial callback handlers
	
	public void HandleInterstitialLoaded(object sender, EventArgs args)
	{
		print("HandleInterstitialLoaded event received.");

		if(Evt_OnAdLoaded != null)
		{
			Evt_OnAdLoaded(this, sender, args);
		}

		//show ad of interstitial
		ShowAd ();
	}
	
	public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		print("HandleInterstitialFailedToLoad event received with message: " + args.Message);

		if(Evt_OnAdFailToLoad != null)
		{
			Evt_OnAdFailToLoad(this, sender, args.Message);
		}
	}
	
	public void HandleInterstitialOpened(object sender, EventArgs args)
	{
		print("HandleInterstitialOpened event received");
	}
	
	void HandleInterstitialClosing(object sender, EventArgs args)
	{
		print("HandleInterstitialClosing event received");
	}
	
	public void HandleInterstitialClosed(object sender, EventArgs args)
	{
		print("HandleInterstitialClosed event received");
	}
	
	public void HandleInterstitialLeftApplication(object sender, EventArgs args)
	{
		print("HandleInterstitialLeftApplication event received");
	}
	
	#endregion

}
