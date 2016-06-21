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

	public void SNS_ShareBtn()
	{
		AndroidSocialGate.StartShareIntent("Share Cellboy to SNS", string.Format("My highscore is {0} in Cellboy!\n->http://www.nanali.net/", m_PlayerData.m_Gamedata.m_iHighScore));
	}
}
