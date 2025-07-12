
# Unity Searchable Dropdown Component

## ✨ Features

- **Dropdown search** with async API requests and local data
- **Debounce input** to prevent excessive calls
- **Pagination support** on scroll to bottom
- **Easily switch** between API and local data

---

## 📜 Script Responsibilities

### `DropdownViewController.cs`
Handles user input, debounce, API/local data switching, pagination, scroll detection, and dropdown open/close logic. Acts as the presenter/controller.

### `DropdownView.cs`
Manages the visual elements of the dropdown including the input field, scroll view, button visuals, and dynamic resizing.

### `DropdownItemController.cs`
Controls individual dropdown item behavior and selection handling.

### `DummyUser.cs`, `DropdownDatum.cs`
Models for user data and dropdown-friendly display format.

### `ApiDataProvider`, `LocalDataProvider`
Implements data retrieval from a remote API or a simulated local source, using a common `IDataProvider` interface.

### `DummyParser.cs`
Parses raw API responses into usable `DummyUser` models using the `IApiResponseParser` interface.

### `UIEventHub.cs`
A communication hub (likely using UnityEvents or ScriptableObjects) for broadcasting dropdown-related events like item selection.

---

## 📝 Note

If you only want to use the dropdown system **without any data integration**,  
you can simply use the `DropdownViewController` and `DropdownView` scripts.

- Populate the dropdown manually using `DropdownView.InitializeContent(...)`
- You do not need to implement `IDataProvider` or `IApiResponseParser`

---

## 🔁 Switching Between API and Local Integration

The dropdown system is designed to allow quick swapping between remote API and local data sources. You can toggle between them by modifying a single method in `DropdownViewController.cs`. Just comment one of the options:

```csharp
private void SetupDataProvider ()
{
    // ✅ Option 1: API Integration
    _apiResponseParser = new DummyParser ();

    _dataProvider = new ApiDataProvider<DummyUser>(
        "https://dummyjson.com/users",
        _apiResponseParser
    );

    // 🔁 Option 2: Local Data (Uncomment to use local data)
    //_dataProvider = new LocalDataProvider();
}
```
## 🖼️ Demo
![Dropdown Demo](Docs/dropdown-demo.gif)
---

## 📄 License

MIT — free to use, modify, and share.

---