using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIEntity : MonoBehaviour, IComparer<UIEntity>
{
	//实体的优先级
	private int _priority;
	public int Priority 
	{
		get 
		{ 
			return _priority;
		}
		set 
		{ 
			_priority = value;
		}
	}

	public enum UIBlockType 
	{
		NotBlock,
		BlockLower,
		BlockAll
	}

	//实体的阻塞类型
	private UIBlockType _blockType;
	public UIBlockType BlockType 
	{
		get 
		{
			return _blockType;
		}
		set 
		{
			_blockType = value;
		}
	}

	public int Compare(UIEntity x, UIEntity y)
	{
		return x.Priority - y.Priority;
	}

	//使用一个队列来接收所有的消息
	private Queue<UITelegram> _queue = new Queue<UITelegram>();

	//返回是否已经处理了消息
	public delegate bool HandleMessageDelegate(UITelegram msg);

	private Dictionary<UIMessageID, HandleMessageDelegate> _dict = new Dictionary<UIMessageID, HandleMessageDelegate>();

	private void OnEnable()
	{
		Load();
	}

	private void OnDisable()
	{
		UnLoad();
	}

	private void OnDestory() 
	{
		if (gameObject.activeInHierarchy) 
		{
			UnLoad();
		}
		
		_dict.Clear ();
	}

	//所有的注册函数在这里写
	protected virtual void Load()
	{
	}

	//所有的取消注册函数在这里写
	protected virtual void UnLoad() 
	{
	}

	protected void RegisterMessage(UIMessageID msgId, UIEntity entity, HandleMessageDelegate handler)
	{
		UIMessageDispatcher.Instance.RegisterMessage(msgId, entity);
		_dict [msgId] = handler;
	}

	protected void UnRegisterMessage(UIMessageID msgId, UIEntity entity) 
	{
		UIMessageDispatcher.Instance.UnRegisterMessage(msgId, entity);
		if (_dict.ContainsKey(msgId)) 
		{
			_dict.Remove(msgId);
		}
	}	

	public bool HandleMessage(UITelegram msg)
	{
		bool ret = false;
		UIMessageID id = (UIMessageID)msg._msgId;

		if (_dict.ContainsKey(id)) 
		{
			ret = _dict[id](msg);
		}

		return ret;
	}

	public void AddMessage(UITelegram msg)
	{
		_queue.Enqueue(msg);
	}

	//actual update
	protected virtual void Tick() 
	{
	}

	void Update() 
	{
		Tick();
		if (_queue.Count > 0) 
		{
			UITelegram msg = _queue.Dequeue();
			HandleMessage(msg);
		}
	}
}
