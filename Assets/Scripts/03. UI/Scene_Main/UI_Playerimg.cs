using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Playerimg : MonoBehaviour {

	public PLAYER_ID m_PlayerID = PLAYER_ID.NORMAL;
	public string m_strName = "A";
	public string m_strInfo = "ABCDEFG";
	public int m_iPlayerIdx = 0;
	public bool m_bSelected = false;
	public int m_iChargePrice = 10;
	public int m_iBuyPrice = 10;

	public UIScrollView m_ScrollView = null;
	private UISprite m_MySprite = null;
	private SkeletonAnimation m_MySkeleton = null;


	public UILabel m_NameLabel = null;

	public BtnManager m_btnMgr = null;
	public SkeletonAnimation m_Skeleton = null;


	void Start()
	{
		m_btnMgr = GameObject.Find ("BtnManager").GetComponent<BtnManager>();

		m_MySprite = GetComponent<UISprite> ();
		m_MySkeleton = GetComponent<SkeletonAnimation> ();

		m_ScrollView = transform.parent.gameObject.GetComponent<UIScrollView>();
		m_Skeleton = gameObject.GetComponent<SkeletonAnimation> ();
	}

	void FixedUpdate()
	{
		if (gameObject != gameObject.transform.parent.gameObject.GetComponent<UICenterOnChild> ().centeredObject)
			m_bSelected = false;

		if (m_bSelected == true
		    && m_ScrollView.isDragging == false) {

			m_MySprite.enabled = false;
			m_MySkeleton.enabled = true;
			transform.localScale = new Vector3 (130.0f, 130.0f);
			m_NameLabel.text = m_strName;
			m_btnMgr.m_iCurrnetPlayerIndex = m_iPlayerIdx;
		} else {
			m_MySprite.enabled = true;
			m_MySkeleton.enabled = false;
			transform.localScale = new Vector3 (1.0f, 1.0f);
		}

		if (m_PlayerID == m_btnMgr.m_PlayerData.m_PlayerID
		    && m_Skeleton != null) {

			if(m_btnMgr.m_PlayerData.m_Gamedata.m_PlayerInfo[(int)m_PlayerID].fTiredPercent <= 0)
			{
				if(m_Skeleton.AnimationName != "sleep")
			   m_Skeleton.state.SetAnimation(0, "sleep", true);
			}
			 else
			{
				if(m_Skeleton.AnimationName != "idle")
			   m_Skeleton.state.SetAnimation(0, "idle", true);
			}
		}
	}
}
