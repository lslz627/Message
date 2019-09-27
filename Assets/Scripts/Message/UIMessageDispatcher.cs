using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIMessageDispatcher 
{
	private Dictionary<UIMessageID, SortedList> _dict = new Dictionary<UIMessageID, SortedList>();

	private UIMessageDispatcher()
	{
	}

	private static UIMessageDispatcher _instance;
	public static UIMessageDispatcher Instance
	{
		get 
		{
			return Nested._instance;
		}
	}

	private class Nested
	{
		public readonly static UIMessageDispatcher _instance = new UIMessageDispatcher();
	}

	public void RegisterMessage(UIMessageID msgId, UIEntity entity)
	{
		SortedList list = null;

		if (_dict.ContainsKey(msgId)) 
		{
			list = _dict [msgId];
		} 
		else 
		{
			list = new SortedList();
		}
	
		list.Add (entity, null);
		_dict [msgId] = list;
	}

	public void UnRegisterMessage(UIMessageID msgId, UIEntity entity)
	{
		if (_dict.ContainsKey (msgId)) 
		{
			SortedList list = _dict[msgId];
			if (list.ContainsKey(entity))
			{
				list.Remove(entity);
			}
		}
	}

	public void SendMessage(UIMessageID msgId, UIEntity entity, object extraInfo)
	{
		SendMessage(msgId, entity, extraInfo, null);
	}

	public void SendMessage(UIMessageID msgId, UIEntity entity, object extraInfo, object extraInfo2)
	{
		if (!_dict.ContainsKey(msgId)) 
		{
			return;
		}
		
		SortedList list = _dict[msgId];
		UITelegram msg = new UITelegram((int)msgId, null, -1, extraInfo, extraInfo2);

		if (entity != null && list.ContainsKey(entity)) 
		{
			entity.HandleMessage (msg);
		}
		else
		{
			for (int i = 0; i < list.Count; i++)
			{
				UIEntity en = (UIEntity)list.GetKey(i);
				
				if (!en.HandleMessage(msg))
				{
					continue;
				}
				
				if (en.BlockType == UIEntity.UIBlockType.BlockAll)
				{
					break;
				}
				else if (en.BlockType == UIEntity.UIBlockType.BlockLower)
				{
					for (int j = i + 1; j < list.Count; j++) 
					{
						UIEntity ent = (UIEntity)list.GetKey(j);
						
						if (en.Priority == ent.Priority) 
						{
							ent.HandleMessage(msg);
						}
						else 
						{
							return;
						}
					}
				}
			}
		}
	}

	public void PostMessage(UIMessageID msgId, UIEntity entity, object extraInfo)
	{
		PostMessage(msgId, entity, extraInfo, null);
	}

	public void PostMessage(UIMessageID msgId, UIEntity entity, object extraInfo, object extraInfo2)
	{
		if (!_dict.ContainsKey(msgId)) 
		{
			return;
		}

		SortedList list = _dict[msgId];
		UITelegram msg = new UITelegram((int)msgId, null, -1, extraInfo, extraInfo2);

		if (entity != null && list.ContainsKey (entity)) 
		{
			entity.AddMessage (msg);
		} else 
		{
			for (int i = 0; i < list.Count; i++) 
			{
				UIEntity en = (UIEntity)list.GetKey(i);
				en.AddMessage(msg);
				
				if (en.BlockType == UIEntity.UIBlockType.BlockAll) 
				{
					break;
				}
				else if (en.BlockType == UIEntity.UIBlockType.BlockLower) 
				{
					for (int j = i + 1; j < list.Count; j++) 
					{
						UIEntity ent = (UIEntity)list.GetKey(j);
						if (en.Priority == ent.Priority) 
						{
							ent.AddMessage(msg);
						}
						else 
						{
							return;
						}
					}
				}
			}
		}
	}
	
}
