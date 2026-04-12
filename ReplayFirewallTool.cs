using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

[assembly: AssemblyTitle("Replay Firewall Tool")]
[assembly: AssemblyDescription("Windows Defender Firewall outbound rule manager for GTA 5 replay glitch.")]
[assembly: AssemblyCompany("Kapsi Tools")]
[assembly: AssemblyProduct("Replay Firewall Tool")]
[assembly: AssemblyCopyright("Copyright (c) 2026")]
[assembly: AssemblyVersion("1.1.0.0")]
[assembly: AssemblyFileVersion("1.1.0.0")]
[assembly: AssemblyInformationalVersion("1.1.0")]

internal enum UiLanguage
{
    English,
    Turkish
}

internal enum RuleState
{
    Missing,
    Disabled,
    Enabled,
    Mixed
}

internal static class FirewallRuleConfig
{
    public const string RuleName = "GTA 5 REPLAY GLITCH";
    public const string Description = "Blocks outbound access for the replay glitch target IP.";
    public const string RemoteIp = "192.81.241.171";
    public const int DirectionOutbound = 2;
    public const int ActionBlock = 0;
    public const int ProtocolAny = 256;
    public const int ProfilesAll = unchecked((int)0x7fffffff);
}

internal sealed class FirewallRuleInfo
{
    public bool Exists { get; set; }
    public int Count { get; set; }
    public int EnabledCount { get; set; }

    public RuleState State
    {
        get
        {
            if (!Exists)
            {
                return RuleState.Missing;
            }

            if (EnabledCount == 0)
            {
                return RuleState.Disabled;
            }

            if (EnabledCount == Count)
            {
                return RuleState.Enabled;
            }

            return RuleState.Mixed;
        }
    }
}

internal sealed class LocalizedText
{
    public string WindowTitle { get; set; }
    public string LanguageLabel { get; set; }
    public string RuleLabel { get; set; }
    public string RemoteIpLabel { get; set; }
    public string StatusLabel { get; set; }
    public string MatchCountLabel { get; set; }
    public string NoteText { get; set; }
    public string AddRuleButton { get; set; }
    public string EnableButton { get; set; }
    public string DisableButton { get; set; }
    public string ToggleButton { get; set; }
    public string RefreshButton { get; set; }
    public string ExitButton { get; set; }
    public string InfoCaption { get; set; }
    public string ErrorCaption { get; set; }
    public string RuleAddedMessage { get; set; }
    public string RuleEnabledMessage { get; set; }
    public string RuleDisabledMessage { get; set; }
    public string RuleToggledMessage { get; set; }
    public string RuleMissingError { get; set; }
    public string FirewallApiError { get; set; }
    public string StatusMissing { get; set; }
    public string StatusDisabled { get; set; }
    public string StatusEnabled { get; set; }
    public string StatusMixedFormat { get; set; }
    public string CountMissing { get; set; }
    public string CountFormat { get; set; }

    public string FormatStatus(FirewallRuleInfo info)
    {
        switch (info.State)
        {
            case RuleState.Enabled:
                return StatusEnabled;
            case RuleState.Disabled:
                return StatusDisabled;
            case RuleState.Mixed:
                return string.Format(CultureInfo.InvariantCulture, StatusMixedFormat, info.EnabledCount, info.Count);
            default:
                return StatusMissing;
        }
    }

    public string FormatRuleCount(FirewallRuleInfo info)
    {
        if (!info.Exists)
        {
            return CountMissing;
        }

        return string.Format(CultureInfo.InvariantCulture, CountFormat, info.Count, info.EnabledCount);
    }
}

internal sealed class LanguageOption
{
    public LanguageOption(UiLanguage language, string displayName)
    {
        Language = language;
        DisplayName = displayName;
    }

    public UiLanguage Language { get; private set; }
    public string DisplayName { get; private set; }

    public override string ToString()
    {
        return DisplayName;
    }
}

internal static class AppText
{
    public static UiLanguage GetDefaultLanguage()
    {
        return string.Equals(
            CultureInfo.CurrentUICulture.TwoLetterISOLanguageName,
            "tr",
            StringComparison.OrdinalIgnoreCase)
            ? UiLanguage.Turkish
            : UiLanguage.English;
    }

    public static LocalizedText For(UiLanguage language)
    {
        if (language == UiLanguage.Turkish)
        {
            return new LocalizedText
            {
                WindowTitle = "Replay Firewall Tool",
                LanguageLabel = "Dil",
                RuleLabel = "Rule",
                RemoteIpLabel = "Remote IP",
                StatusLabel = "Durum",
                MatchCountLabel = "Eslesen rule",
                NoteText =
                    "Bu arac mevcut outbound rule'u acip kapatir. Rule yoksa 'Kurali Ekle' butonu, " +
                    "192.81.241.171 IP'sine giden outbound block rule'unu kapali olarak olusturur.",
                AddRuleButton = "Kurali Ekle",
                EnableButton = "Ac",
                DisableButton = "Kapat",
                ToggleButton = "Toggle",
                RefreshButton = "Yenile",
                ExitButton = "Cikis",
                InfoCaption = "Bilgi",
                ErrorCaption = "Hata",
                RuleAddedMessage =
                    "Rule eklendi. Varsayilan olarak kapali olusturuldu; istersen simdi 'Ac' ile aktiflestirebilirsin.",
                RuleEnabledMessage = "Rule aktif edildi.",
                RuleDisabledMessage = "Rule kapatildi.",
                RuleToggledMessage = "Rule durumu degistirildi.",
                RuleMissingError = "Kural bulunamadi. Once 'Kurali Ekle' secenegini kullan.",
                FirewallApiError = "Windows Firewall API baslatilamadi.",
                StatusMissing = "Kural bulunamadi",
                StatusDisabled = "Kapali",
                StatusEnabled = "Acik",
                StatusMixedFormat = "Karisik ({0}/{1} acik)",
                CountMissing = "0 adet",
                CountFormat = "{0} adet, {1} aktif"
            };
        }

        return new LocalizedText
        {
            WindowTitle = "Replay Firewall Tool",
            LanguageLabel = "Language",
            RuleLabel = "Rule",
            RemoteIpLabel = "Remote IP",
            StatusLabel = "Status",
            MatchCountLabel = "Matching rules",
            NoteText =
                "This tool enables and disables the outbound rule for the replay setup. " +
                "If the rule does not exist, 'Add Rule' creates the outbound block rule for 192.81.241.171 in a disabled state.",
            AddRuleButton = "Add Rule",
            EnableButton = "Enable",
            DisableButton = "Disable",
            ToggleButton = "Toggle",
            RefreshButton = "Refresh",
            ExitButton = "Exit",
            InfoCaption = "Information",
            ErrorCaption = "Error",
            RuleAddedMessage =
                "The rule was added in a disabled state. You can enable it now with the 'Enable' button.",
            RuleEnabledMessage = "The rule was enabled.",
            RuleDisabledMessage = "The rule was disabled.",
            RuleToggledMessage = "The rule state was toggled.",
            RuleMissingError = "The rule was not found. Use 'Add Rule' first.",
            FirewallApiError = "Windows Firewall API could not be started.",
            StatusMissing = "Rule not found",
            StatusDisabled = "Disabled",
            StatusEnabled = "Enabled",
            StatusMixedFormat = "Mixed ({0}/{1} enabled)",
            CountMissing = "0 rules",
            CountFormat = "{0} rules, {1} enabled"
        };
    }
}

internal static class FirewallRuleService
{
    public const string FirewallApiUnavailableCode = "FIREWALL_API_UNAVAILABLE";

    public static FirewallRuleInfo GetRuleInfo()
    {
        dynamic policy = CreatePolicy();
        List<dynamic> rules = GetMatchingRules(policy);

        return new FirewallRuleInfo
        {
            Exists = rules.Count > 0,
            Count = rules.Count,
            EnabledCount = rules.Count(rule => Convert.ToBoolean(rule.Enabled))
        };
    }

    public static void AddRuleIfMissing(bool enabledByDefault)
    {
        dynamic policy = CreatePolicy();
        List<dynamic> rules = GetMatchingRules(policy);
        if (rules.Count > 0)
        {
            return;
        }

        dynamic newRule = Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
        newRule.Name = FirewallRuleConfig.RuleName;
        newRule.Description = FirewallRuleConfig.Description;
        newRule.Direction = FirewallRuleConfig.DirectionOutbound;
        newRule.Action = FirewallRuleConfig.ActionBlock;
        newRule.Enabled = enabledByDefault;
        newRule.Protocol = FirewallRuleConfig.ProtocolAny;
        newRule.Profiles = FirewallRuleConfig.ProfilesAll;
        newRule.RemoteAddresses = FirewallRuleConfig.RemoteIp;

        policy.Rules.Add(newRule);
    }

    public static void SetRuleEnabled(bool enabled, LocalizedText text)
    {
        dynamic policy = CreatePolicy();
        List<dynamic> rules = GetMatchingRules(policy);
        if (rules.Count == 0)
        {
            throw new InvalidOperationException(text.RuleMissingError);
        }

        foreach (dynamic rule in rules)
        {
            rule.Enabled = enabled;
        }
    }

    public static void ToggleRule(LocalizedText text)
    {
        FirewallRuleInfo info = GetRuleInfo();
        if (!info.Exists)
        {
            throw new InvalidOperationException(text.RuleMissingError);
        }

        SetRuleEnabled(info.EnabledCount == 0, text);
    }

    private static dynamic CreatePolicy()
    {
        Type type = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
        if (type == null)
        {
            throw new InvalidOperationException(FirewallApiUnavailableCode);
        }

        return Activator.CreateInstance(type);
    }

    private static List<dynamic> GetMatchingRules(dynamic policy)
    {
        List<dynamic> matches = new List<dynamic>();

        foreach (dynamic rule in policy.Rules)
        {
            if (!string.Equals(Convert.ToString(rule.Name), FirewallRuleConfig.RuleName, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (Convert.ToInt32(rule.Direction) != FirewallRuleConfig.DirectionOutbound)
            {
                continue;
            }

            matches.Add(rule);
        }

        return matches;
    }
}

internal sealed class MainForm : Form
{
    private readonly Label languageLabel;
    private readonly Label ruleLabel;
    private readonly Label remoteIpLabel;
    private readonly Label statusLabel;
    private readonly Label matchCountLabel;
    private readonly Label statusValue;
    private readonly Label countValue;
    private readonly Label noteLabel;
    private readonly ComboBox languageSelector;
    private readonly Button addButton;
    private readonly Button enableButton;
    private readonly Button disableButton;
    private readonly Button toggleButton;
    private readonly Button refreshButton;
    private readonly Button exitButton;

    private LocalizedText text;

    public MainForm()
    {
        Text = "Replay Firewall Tool";
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(640, 410);
        MinimumSize = new Size(640, 410);
        Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
        ShowIcon = true;

        Icon icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
        if (icon != null)
        {
            Icon = icon;
        }

        TableLayoutPanel layout = new TableLayoutPanel();
        layout.Dock = DockStyle.Fill;
        layout.Padding = new Padding(16);
        layout.RowCount = 7;
        layout.ColumnCount = 2;
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 120F));

        languageLabel = BuildLabel("Language");
        languageSelector = new ComboBox();
        languageSelector.DropDownStyle = ComboBoxStyle.DropDownList;
        languageSelector.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        languageSelector.Margin = new Padding(0, 4, 0, 4);

        ruleLabel = BuildLabel("Rule");
        Label ruleValue = BuildValue(FirewallRuleConfig.RuleName);
        remoteIpLabel = BuildLabel("Remote IP");
        Label remoteIpValue = BuildValue(FirewallRuleConfig.RemoteIp);
        statusLabel = BuildLabel("Status");
        statusValue = BuildValue("-");
        matchCountLabel = BuildLabel("Matching rules");
        countValue = BuildValue("-");

        Panel notePanel = new Panel();
        notePanel.Dock = DockStyle.Fill;
        notePanel.Padding = new Padding(0, 12, 0, 0);

        noteLabel = new Label();
        noteLabel.AutoSize = false;
        noteLabel.Dock = DockStyle.Fill;
        noteLabel.ForeColor = Color.FromArgb(55, 55, 55);

        notePanel.Controls.Add(noteLabel);

        TableLayoutPanel buttons = new TableLayoutPanel();
        buttons.Dock = DockStyle.Fill;
        buttons.ColumnCount = 3;
        buttons.RowCount = 2;
        buttons.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
        buttons.Margin = new Padding(0);
        buttons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
        buttons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
        buttons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));
        buttons.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
        buttons.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));

        addButton = BuildButton("Add Rule", AddRuleClicked);
        enableButton = BuildButton("Enable", EnableClicked);
        disableButton = BuildButton("Disable", DisableClicked);
        toggleButton = BuildButton("Toggle", ToggleClicked);
        refreshButton = BuildButton("Refresh", RefreshClicked);
        exitButton = BuildButton("Exit", ExitClicked);

        buttons.Controls.Add(addButton, 0, 0);
        buttons.Controls.Add(enableButton, 1, 0);
        buttons.Controls.Add(disableButton, 2, 0);
        buttons.Controls.Add(toggleButton, 0, 1);
        buttons.Controls.Add(refreshButton, 1, 1);
        buttons.Controls.Add(exitButton, 2, 1);

        layout.Controls.Add(languageLabel, 0, 0);
        layout.Controls.Add(languageSelector, 1, 0);
        layout.Controls.Add(ruleLabel, 0, 1);
        layout.Controls.Add(ruleValue, 1, 1);
        layout.Controls.Add(remoteIpLabel, 0, 2);
        layout.Controls.Add(remoteIpValue, 1, 2);
        layout.Controls.Add(statusLabel, 0, 3);
        layout.Controls.Add(statusValue, 1, 3);
        layout.Controls.Add(matchCountLabel, 0, 4);
        layout.Controls.Add(countValue, 1, 4);
        layout.Controls.Add(notePanel, 0, 5);
        layout.SetColumnSpan(notePanel, 2);
        layout.Controls.Add(buttons, 0, 6);
        layout.SetColumnSpan(buttons, 2);

        Controls.Add(layout);

        PopulateLanguages();
        ApplyLanguage(AppText.GetDefaultLanguage());
        Load += delegate { RefreshView(); };
    }

    private static Label BuildLabel(string value)
    {
        return new Label
        {
            Text = value,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point)
        };
    }

    private static Label BuildValue(string value)
    {
        return new Label
        {
            Text = value,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft
        };
    }

    private static Button BuildButton(string value, EventHandler handler)
    {
        Button button = new Button();
        button.Text = value;
        button.Dock = DockStyle.Fill;
        button.MinimumSize = new Size(0, 40);
        button.Margin = new Padding(4);
        button.Click += handler;
        return button;
    }

    private void PopulateLanguages()
    {
        languageSelector.Items.Add(new LanguageOption(UiLanguage.English, "English"));
        languageSelector.Items.Add(new LanguageOption(UiLanguage.Turkish, "Turkce"));
        languageSelector.SelectedIndexChanged += LanguageChanged;
    }

    private void LanguageChanged(object sender, EventArgs e)
    {
        LanguageOption option = languageSelector.SelectedItem as LanguageOption;
        if (option == null)
        {
            return;
        }

        ApplyLanguage(option.Language);
    }

    private void ApplyLanguage(UiLanguage language)
    {
        text = AppText.For(language);

        Text = text.WindowTitle;
        languageLabel.Text = text.LanguageLabel;
        ruleLabel.Text = text.RuleLabel;
        remoteIpLabel.Text = text.RemoteIpLabel;
        statusLabel.Text = text.StatusLabel;
        matchCountLabel.Text = text.MatchCountLabel;
        noteLabel.Text = text.NoteText;
        addButton.Text = text.AddRuleButton;
        enableButton.Text = text.EnableButton;
        disableButton.Text = text.DisableButton;
        toggleButton.Text = text.ToggleButton;
        refreshButton.Text = text.RefreshButton;
        exitButton.Text = text.ExitButton;

        LanguageOption selected = languageSelector.Items
            .OfType<LanguageOption>()
            .FirstOrDefault(item => item.Language == language);

        if (selected != null && !ReferenceEquals(languageSelector.SelectedItem, selected))
        {
            languageSelector.SelectedItem = selected;
        }

        RefreshView();
    }

    private void RefreshView()
    {
        try
        {
            FirewallRuleInfo info = FirewallRuleService.GetRuleInfo();

            statusValue.Text = text.FormatStatus(info);
            countValue.Text = text.FormatRuleCount(info);

            addButton.Enabled = !info.Exists;
            enableButton.Enabled = info.Exists && info.EnabledCount < info.Count;
            disableButton.Enabled = info.Exists && info.EnabledCount > 0;
            toggleButton.Enabled = info.Exists;
        }
        catch (Exception ex)
        {
            statusValue.Text = text != null ? text.ErrorCaption : "Error";
            countValue.Text = "-";
            addButton.Enabled = true;
            enableButton.Enabled = false;
            disableButton.Enabled = false;
            toggleButton.Enabled = false;
            ShowError(ex.Message);
        }
    }

    private void AddRuleClicked(object sender, EventArgs e)
    {
        RunAction(
            delegate { FirewallRuleService.AddRuleIfMissing(false); },
            text.RuleAddedMessage);
    }

    private void EnableClicked(object sender, EventArgs e)
    {
        RunAction(
            delegate { FirewallRuleService.SetRuleEnabled(true, text); },
            text.RuleEnabledMessage);
    }

    private void DisableClicked(object sender, EventArgs e)
    {
        RunAction(
            delegate { FirewallRuleService.SetRuleEnabled(false, text); },
            text.RuleDisabledMessage);
    }

    private void ToggleClicked(object sender, EventArgs e)
    {
        RunAction(
            delegate { FirewallRuleService.ToggleRule(text); },
            text.RuleToggledMessage);
    }

    private void RefreshClicked(object sender, EventArgs e)
    {
        RefreshView();
    }

    private void ExitClicked(object sender, EventArgs e)
    {
        Close();
    }

    private void RunAction(Action action, string successMessage)
    {
        try
        {
            action();
            RefreshView();
            MessageBox.Show(successMessage, text.InfoCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }

    private void ShowError(string message)
    {
        string displayMessage = message;

        if (text != null && string.Equals(message, FirewallRuleService.FirewallApiUnavailableCode, StringComparison.Ordinal))
        {
            displayMessage = text.FirewallApiError;
        }

        MessageBox.Show(displayMessage, text != null ? text.ErrorCaption : "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new MainForm());
    }
}
