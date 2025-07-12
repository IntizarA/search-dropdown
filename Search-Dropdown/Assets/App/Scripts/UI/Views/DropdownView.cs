using System.Collections.Generic;
using App.Scripts.Controllers;
using App.Scripts.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropdownView : MonoBehaviour
{
	public TMP_InputField inputField;
	public ScrollRect scrollRect;
	public Button dropdownButton;

	[SerializeField] private Transform content;
	[SerializeField] private DropdownItemController itemPrefab;

	private int maxContentItemCount = 7;
	private float prefabHeight;
	private bool isDropdownOpen = false;

	#region Unity Methods

	void Start ()
	{
		prefabHeight = itemPrefab.GetComponent<RectTransform> ().sizeDelta.y;
	}

	#endregion
	public void ShowDropdown (bool visible)
	{
		if (isDropdownOpen == visible) return;
		isDropdownOpen = visible;
		scrollRect.gameObject.SetActive(visible);
		UpdateDropdownButtonImage (visible);
	} 
	
	private void UpdateDropdownButtonImage (bool isOpen)
	{
		dropdownButton.transform.localScale=new Vector3 (dropdownButton.transform.localScale.x,
			isOpen?-1:1,dropdownButton.transform.localScale.z);
	}

	public void ClearContent ()
	{
		foreach (Transform child in content.transform)
		{
			Destroy (child.gameObject);
		}
	}
	

	public void SetInputText(string value) => inputField.text = value;

	public void InitializeContent (List<DropdownDatum> data)
	{
		ClearContent ();
		foreach (DropdownDatum datum in data)
		{
			DropdownItemController itemController = Instantiate (itemPrefab.gameObject, content).GetComponent<DropdownItemController> ();
			itemController.SetParameters (datum.Id,datum.Text);
		}
		UpdateScrollViewHeight (data.Count);
	}
	
	private void UpdateScrollViewHeight (int childCount)
	{
		float newHeight = Mathf.Clamp (childCount, 0, maxContentItemCount) * prefabHeight;

		scrollRect.GetComponent<RectTransform> ().sizeDelta = new Vector2(scrollRect.GetComponent<RectTransform> ().sizeDelta.x, newHeight);
		
		LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.GetComponent<RectTransform> ());
	}

	public bool IsAnyContent () => content.childCount>0;
}
