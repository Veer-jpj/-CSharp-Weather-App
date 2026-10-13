using System.Globalization;

namespace WeatherApp;

public class MainForm : Form
{
    private const string ApiKeyVariable = "VISUAL_CROSSING_API_KEY";

    private readonly TextBox _txtLocation = new() { PlaceholderText = "Enter a city (e.g. Mumbai)", Width = 260 };
    private readonly ComboBox _cboUnits = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 130 };
    private readonly Button _btnGet = new() { Text = "Get Weather", AutoSize = true };
    private readonly Label _lblStatus = new() { AutoSize = true, ForeColor = Color.DimGray };

    private readonly Label _lblPlace = new() { AutoSize = true, Font = new Font("Segoe UI", 16, FontStyle.Bold) };
    private readonly Label _lblTemp = new() { AutoSize = true, Font = new Font("Segoe UI", 32, FontStyle.Bold) };
    private readonly Label _lblConditions = new() { AutoSize = true, Font = new Font("Segoe UI", 12) };

    private readonly Dictionary<string, Label> _details = new();
    private readonly DataGridView _grid = new();

    private WeatherService? _service;

    public MainForm()
    {
        Text = "Weather App - Visual Crossing";
        MinimumSize = new Size(760, 640);
        StartPosition = FormStartPosition.CenterScreen;
        Font = new Font("Segoe UI", 10);

        BuildLayout();

        _cboUnits.Items.AddRange(new object[] { "Metric (°C)", "US (°F)" });
        _cboUnits.SelectedIndex = 0;

        _btnGet.Click += async (_, _) => await LoadWeatherAsync();
        _txtLocation.KeyDown += async (_, e) =>
        {
            if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; await LoadWeatherAsync(); }
        };

        var apiKey = Environment.GetEnvironmentVariable(ApiKeyVariable);
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            _btnGet.Enabled = false;
            _lblStatus.ForeColor = Color.Firebrick;
            _lblStatus.Text = $"Missing {ApiKeyVariable}. Add it to the .env file and restart.";
        }
        else
        {
            _service = new WeatherService(apiKey);
            _lblStatus.Text = "Enter a location and click Get Weather.";
        }
    }

    private void BuildLayout()
    {
        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 5,
            Padding = new Padding(16)
        };
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // search bar
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // status
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // headline
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // details
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // forecast grid

        // Search bar
        var search = new FlowLayoutPanel { AutoSize = true, Dock = DockStyle.Top, WrapContents = false };
        search.Controls.AddRange(new Control[] { _txtLocation, _cboUnits, _btnGet });
        root.Controls.Add(search, 0, 0);

        root.Controls.Add(_lblStatus, 0, 1);

        // Headline
        var headline = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.TopDown,
            AutoSize = true,
            Dock = DockStyle.Top,
            Margin = new Padding(0, 12, 0, 8)
        };
        headline.Controls.AddRange(new Control[] { _lblPlace, _lblTemp, _lblConditions });
        root.Controls.Add(headline, 0, 2);

        // Detail grid (label / value pairs, 3 per row)
        var details = new TableLayoutPanel { ColumnCount = 6, AutoSize = true, Dock = DockStyle.Top };
        for (int i = 0; i < 6; i++)
            details.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, i % 2 == 0 ? 12 : 21.33f));

        string[] names = { "Feels like", "Humidity", "Wind", "Pressure", "Visibility", "UV index", "Sunrise", "Sunset" };
        for (int i = 0; i < names.Length; i++)
        {
            var name = new Label { Text = names[i], AutoSize = true, ForeColor = Color.Gray, Margin = new Padding(0, 6, 8, 6) };
            var value = new Label { Text = "—", AutoSize = true, Font = new Font(Font, FontStyle.Bold), Margin = new Padding(0, 6, 16, 6) };
            _details[names[i]] = value;
            details.Controls.Add(name, (i % 3) * 2, i / 3);
            details.Controls.Add(value, (i % 3) * 2 + 1, i / 3);
        }
        root.Controls.Add(details, 0, 3);

        // Forecast grid
        _grid.Dock = DockStyle.Fill;
        _grid.ReadOnly = true;
        _grid.AllowUserToAddRows = false;
        _grid.AllowUserToDeleteRows = false;
        _grid.RowHeadersVisible = false;
        _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _grid.BackgroundColor = SystemColors.Window;
        _grid.Margin = new Padding(0, 12, 0, 0);
        foreach (var col in new[] { "Date", "High", "Low", "Rain chance", "Conditions" })
            _grid.Columns.Add(col, col);
        root.Controls.Add(_grid, 0, 4);

        Controls.Add(root);
    }

    private async Task LoadWeatherAsync()
    {
        if (_service == null) return;

        var location = _txtLocation.Text.Trim();
        if (location.Length == 0)
        {
            _lblStatus.ForeColor = Color.Firebrick;
            _lblStatus.Text = "Please enter a location.";
            return;
        }

        var metric = _cboUnits.SelectedIndex == 0;
        _btnGet.Enabled = false;
        _lblStatus.ForeColor = Color.DimGray;
        _lblStatus.Text = "Loading...";

        try
        {
            var data = await _service.GetWeatherAsync(location, metric ? "metric" : "us");
            Display(data, metric);
            _lblStatus.Text = $"Updated {DateTime.Now:t}";
        }
        catch (TaskCanceledException)
        {
            ShowError("The request timed out. Check your connection and try again.");
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
        finally
        {
            _btnGet.Enabled = true;
        }
    }

    private void ShowError(string message)
    {
        _lblStatus.ForeColor = Color.Firebrick;
        _lblStatus.Text = message;
    }

    private void Display(WeatherResponse data, bool metric)
    {
        var t = metric ? "°C" : "°F";
        var speed = metric ? "km/h" : "mph";
        var dist = metric ? "km" : "mi";

        _lblPlace.Text = data.ResolvedAddress ?? "Unknown location";

        var c = data.Current;
        if (c != null)
        {
            _lblTemp.Text = Fmt(c.Temp, t);
            _lblConditions.Text = c.Conditions ?? "";
            _details["Feels like"].Text = Fmt(c.FeelsLike, t);
            _details["Humidity"].Text = Fmt(c.Humidity, "%");
            _details["Wind"].Text = Fmt(c.WindSpeed, " " + speed);
            _details["Pressure"].Text = Fmt(c.Pressure, " mb");
            _details["Visibility"].Text = Fmt(c.Visibility, " " + dist);
            _details["UV index"].Text = Fmt(c.UvIndex, "");
            _details["Sunrise"].Text = c.Sunrise ?? "—";
            _details["Sunset"].Text = c.Sunset ?? "—";
        }

        _grid.Rows.Clear();
        foreach (var d in data.Days)
        {
            var date = DateTime.TryParse(d.Date, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt)
                ? dt.ToString("ddd, MMM d")
                : d.Date;
            _grid.Rows.Add(date, Fmt(d.TempMax, t), Fmt(d.TempMin, t), Fmt(d.PrecipProb, "%"), d.Conditions);
        }
    }

    private static string Fmt(double? value, string suffix) =>
        value.HasValue ? $"{value.Value.ToString("0.#", CultureInfo.InvariantCulture)}{suffix}" : "—";
}
