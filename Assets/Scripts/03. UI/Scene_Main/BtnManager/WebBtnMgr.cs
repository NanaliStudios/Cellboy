using UnityEngine;
using System.Collections;

public partial class BtnManager : MonoBehaviour {

	public void ConnectFacebook()
	{
		Application.OpenURL ("http://facebook.com/nanalistudios");
	}

	public void ConnectTwitter()
	{
		Application.OpenURL ("https://twitter.com/nanalistudios");
	}

	public void ConnectNanali()
	{
		Application.OpenURL ("http://www.nanali.net");
	}

	public void ConnectMail()
	{
		Application.OpenURL ("http://www.nanali.net/bbs/write/bbs_qna");
	}
}
