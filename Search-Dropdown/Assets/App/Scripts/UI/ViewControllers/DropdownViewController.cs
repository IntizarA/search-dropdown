using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using App.Scripts.Abstracts;
using App.Scripts.Concretes;
using App.Scripts.Models;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropdownViewController : MonoBehaviour
{
	[SerializeField] private DropdownView view;
	[SerializeField] private GraphicRaycaster raycaster;
	[SerializeField] private GraphicRaycaster scrollRectRaycaster;
	[SerializeField] private float debounceDelay = 0.5f;
	[SerializeField] private int pageSize = 10;

	public UIEventHub eventHub;
	
	private EventSystem eventSystem;
	private IDataProvider<DummyUser> _dataProvider;
	private IApiResponseParser<DummyUser> _apiResponseParser;
	private List<DropdownDatum> dropdownData = new ();

	private int currentPage = 1;
	private string lastInputValue = null;
	private bool isScrolledToBottom = false;
	private bool isLoadingData = false;

	// Debounce system
	private CancellationTokenSource debounceTokenSource;
	private readonly object debounceTokenLock = new object ();

	#region Unity Methods

	void Start ()
	{
		InitializeComponents ();
		SetupDataProvider ();
	}

	void OnEnable ()
	{
	eventHub.OnDropdownOptionSelectedEvent.AddListener (OnDropdownOptionSelectedEventHandler);	
	}

	void OnDisable ()
	{
		eventHub.OnDropdownOptionSelectedEvent.RemoveListener (OnDropdownOptionSelectedEventHandler);
	}

	void Update ()
	{
		HandleClickOutsideDropdown ();
	}

	void OnDestroy ()
	{
		CancelDebounce ();
	}

	#endregion

	#region Initialization

	private void InitializeComponents ()
	{
		eventSystem = EventSystem.current;
		if (eventSystem == null)
		{
			Debug.LogError ("EventSystem not found in scene!");
		}
	}

	private void SetupDataProvider ()
	{
		//option - 1
		_apiResponseParser = new DummyParser ();
		_dataProvider = new ApiDataProvider<DummyUser> ("https://dummyjson.com/users", _apiResponseParser);
	  
		//option -2
		//_dataProvider = new LocalDataProvider ();
	}

	#endregion

	#region Event Handlers

	private void OnDropdownOptionSelectedEventHandler (DropdownDatum eventArgs)
	{
		Debug.Log ($"Selected Option:{eventArgs.Id} - {eventArgs.Text}");
		view.SetInputText (eventArgs.Text);
		ResetPagination ();
		view.ClearContent ();
		view.ShowDropdown (false);
	}

	public async void OnInputSelected (string value)
	{
		if (!view.IsAnyContent ())
		{
			await RequestDataAsync ();
		}
		view.ShowDropdown (true);
	}

	public void OnInputDeselected (string value)
	{
		CancelDebounce ();
	}

	public void OnInputValueChanged (string value)
	{
		if (lastInputValue != null && lastInputValue.Equals (value)) return;
		lastInputValue = value;
		ResetPagination ();
		StartDebounce ();
	}

	public async void OnScroll (Vector2 scrollPosition)
	{
		if (ShouldLoadNextPage ())
		{
			Debug.Log ("Loading next page...");
			currentPage++;
			await RequestDataAsync ();
			isScrolledToBottom = true;
		}
		else if (view.scrollRect.verticalNormalizedPosition >= 0.02f)
		{
			isScrolledToBottom = false;
		}
	}

	#endregion

	#region Private Methods

	private bool ShouldLoadNextPage ()
	{
		return view.scrollRect.verticalNormalizedPosition <= 0f &&
		       !isScrolledToBottom &&
		       !isLoadingData && HasMoreData();
	}

	private bool HasMoreData () => currentPage * pageSize < _dataProvider.TotalCount;

	private void HandleClickOutsideDropdown ()
	{
		if (Input.GetMouseButtonDown (0) && IsClickOutside ())
		{
			view.ShowDropdown (false);
		}
	}

	private bool IsClickOutside ()
	{
		if (eventSystem == null) return true;

		PointerEventData pointerEventData = new PointerEventData (eventSystem)
		{
			position = Input.mousePosition
		};

		List<RaycastResult> allResults = GetAllRaycastResults (pointerEventData);

		foreach (RaycastResult result in allResults)
		{
			if (IsTargetOrChild (result.gameObject, view.scrollRect.gameObject) ||
			    IsTargetOrChild (result.gameObject, view.inputField.gameObject))
			{
				return false;
			}
		}

		return true;
	}

	private List<RaycastResult> GetAllRaycastResults (PointerEventData pointerEventData)
	{
		List<RaycastResult> allResults = new List<RaycastResult> ();

		if (raycaster != null)
		{
			List<RaycastResult> mainResults = new List<RaycastResult> ();
			raycaster.Raycast (pointerEventData, mainResults);
			allResults.AddRange (mainResults);
		}

		if (scrollRectRaycaster != null && scrollRectRaycaster != raycaster)
		{
			List<RaycastResult> scrollResults = new List<RaycastResult> ();
			scrollRectRaycaster.Raycast (pointerEventData, scrollResults);
			allResults.AddRange (scrollResults);
		}

		return allResults;
	}

	private bool IsTargetOrChild (GameObject hitObject, GameObject target)
	{
		if (hitObject == null || target == null) return false;

		Transform current = hitObject.transform;
		while (current != null)
		{
			if (current.gameObject == target)
				return true;
			current = current.parent;
		}

		return false;
	}

	private async Task RequestDataAsync ()
	{
		if (isLoadingData) return;

		isLoadingData = true;

		try
		{
			var response = await _dataProvider.GetDataAsync (currentPage, pageSize, lastInputValue);
			ProcessDataResponse (response);
		}
		catch (System.Exception ex)
		{
			Debug.LogError ($"Error loading data: {ex.Message}");
		}
		finally
		{
			isLoadingData = false;
		}
	}

	private void ProcessDataResponse (List<DummyUser> response)
	{
		Debug.Log ($"Received data for page {currentPage}");
		response.ForEach (x => dropdownData.Add (new ()
		{
			Id = x.id,
			Text = x.email
		}));
		view.InitializeContent (dropdownData);
	}

	private void ResetPagination ()
	{
		currentPage = 1;
		dropdownData.Clear ();
	}

	private void StartDebounce ()
	{
		CancelDebounce ();

		lock (debounceTokenLock)
		{
			debounceTokenSource = new CancellationTokenSource ();
			_ = DebounceAsync (debounceTokenSource.Token);
		}
	}

	private async Task DebounceAsync (CancellationToken cancellationToken)
	{
		try
		{
			await Task.Delay ((int)(debounceDelay * 1000), cancellationToken);

			if (!cancellationToken.IsCancellationRequested)
			{
				await RequestDataAsync ();
			}
		}
		catch (TaskCanceledException)
		{
			// Expected when debounce is cancelled
		}
	}

	private void CancelDebounce ()
	{
		lock (debounceTokenLock)
		{
			if (debounceTokenSource != null)
			{
				debounceTokenSource.Cancel ();
				debounceTokenSource.Dispose ();
				debounceTokenSource = null;
			}
		}
	}

	#endregion

	#region Button Handlers

	public void OnDropdownButtonClicked ()
	{
		// Toggle dropdown visibility or implement desired behavior
		bool isCurrentlyVisible = view.scrollRect.gameObject.activeInHierarchy;
		view.ShowDropdown (!isCurrentlyVisible);
	}

	#endregion
}