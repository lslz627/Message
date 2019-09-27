using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UITelegram : IComparer<UITelegram>
{
	//消息的id
	public int _msgId;

	//消息的频道，是一个中分类的方式
	public int _channel;

	//消息的发送者
	public object _sender;

	//消息的接收者
	public int _receiver;

	//消息发送的时间
	public long _dispathTime;

	//额外的信息
	public object _extraInfo;

	//额外的信息2
	public object _extraInfo2;

	//消息的优先级是根据消息的发送时间来定的,发送时间越先的优先级越高
	public int Compare(UITelegram x, UITelegram y)
	{
		int ret = 0;
		if ((x._dispathTime - y._dispathTime) <= 50 && 
						x._sender == y._sender &&
						x._receiver == y._receiver &&
						x._msgId == y._msgId) 
		{
			ret = 0;
		}

		ret = (int)(x._dispathTime - y._dispathTime);

		return ret;
	}

	public UITelegram(int msgId, object sender, int receiver) :
		this (msgId, sender, receiver, null, null)
	{

	}

	public UITelegram (int msgId, object sender, int receiver, object extraInfo) :
		this (msgId, sender, receiver, extraInfo, null)
	{

	}

	public UITelegram (int msgId, object sender, int receiver, object extraInfo, object extraInfo2)
	{
		this._msgId = msgId;
		this._sender = sender;
		this._receiver = receiver;
		this._extraInfo = extraInfo;
		this._extraInfo2 = extraInfo2;
		this._dispathTime = DateTime.Now.Ticks;
	}
}
