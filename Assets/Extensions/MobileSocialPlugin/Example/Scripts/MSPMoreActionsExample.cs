using UnityEngine;
using System.Collections;

public class MSPMoreActionsExample : MonoBehaviour {

	public Texture2D imageForPosting;

	public void InstaShare() {
		SPInstagram.Share(imageForPosting);
	}

	public void InstaShareWithText() {
		SPInstagram.Share(imageForPosting, "I am posting from my app");
	}

	public void NativeShare() {
		SPShareUtility.ShareMedia("Share Camption", "Share Message");
	}

	public void NativeShareWithImage() {
		SPShareUtility.ShareMedia("Share Camption", "Share Message", imageForPosting);
	}

	public void SendMail() {
		SPShareUtility.SendMail( "My E-mail Subject", "This is my text to share", "mail1@gmail.com, mail2@gmail.com", imageForPosting);
	}
}
