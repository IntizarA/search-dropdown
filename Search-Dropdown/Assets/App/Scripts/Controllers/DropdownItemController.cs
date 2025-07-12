using App.Scripts.Models;
using TMPro;
using UnityEngine;

namespace App.Scripts.Controllers
{
	public class DropdownItemController:MonoBehaviour
	{
		public UIEventHub eventHub;
		[SerializeField] private TMP_Text textField;
		private int _id;

		public void SetParameters (int id, string text)
		{
			_id = id;
			textField.text = text;
		}

		public void OnButtonClicked ()
		{
			if(eventHub==null) return;
			
			eventHub.OnDropdownOptionSelectedEvent.Invoke (new DropdownDatum (){Id = _id, Text = textField.text});	
		}
	}
}