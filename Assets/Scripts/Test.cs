using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Test : UIEntity
{
	protected override void Load()
	{
		RegisterMessage(UIMessageID.Test, this, HandleTestMessage);
	}

	protected override void UnLoad()
	{
		UnRegisterMessage(UIMessageID.Test, this);
	}

	public bool HandleTestMessage(UITelegram msg)
	{
		string str = msg._extraInfo as string;
		Debug.Log(str);

		return true;
	}

	public Button TestButton;

	void Start()
	{
		TestButton.onClick.AddListener(() => {
			UIMessageDispatcher.Instance.SendMessage(UIMessageID.Test, null, "hello world!");
		});
	}
}
