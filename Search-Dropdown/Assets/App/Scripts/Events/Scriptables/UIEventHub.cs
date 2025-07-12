using App.Scripts.Models;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "EventHub/UIEventHub")]
public class UIEventHub : ScriptableObject
{
	[System.Serializable]
	public class DropdownOptionSelectedEvent : UnityEvent<DropdownDatum> { }
	
	public DropdownOptionSelectedEvent OnDropdownOptionSelectedEvent= new DropdownOptionSelectedEvent();
}
