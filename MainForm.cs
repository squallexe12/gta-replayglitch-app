using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

internal sealed class MainForm : Form
{
    private readonly AppSettings settings;
    private readonly Icon appIcon;
    private readonly ToastNotifier toastNotifier;

    private readonly Label languageLabel;
    private readonly ComboBox languageSelector;
    private readonly Label versionCaptionLabel;
    private readonly Label versionValueLabel;
    private readonly Label ruleLabel;
    private readonly Label ruleValueLabel;
    private readonly Label remoteIpLabel;
    private readonly Label remoteIpValueLabel;
    private readonly Label statusLabel;
    private readonly Label statusBadgeLabel;
    private readonly Label statusDescriptionLabel;
    private readonly Label matchCountLabel;
    private readonly Label countValueLabel;
    private readonly Panel notePanel;
    private readonly Label noteLabel;
    private readonly GroupBox detailsGroup;
    private readonly Label detailsSummaryLabel;
    private readonly Label detailsSummaryValueLabel;
    private readonly Label detailsRuleNameLabel;
    private readonly Label detailsRuleNameValueLabel;
    private readonly Label detailsDirectionLabel;
    private readonly Label detailsDirectionValueLabel;
    private readonly Label detailsActionLabel;
    private readonly Label detailsActionValueLabel;
    private readonly Label detailsProtocolLabel;
    private readonly Label detailsProtocolValueLabel;
    private readonly Label detailsProfilesLabel;
    private readonly Label detailsProfilesValueLabel;
    private readonly Label detailsDefaultStateLabel;
    private readonly Label detailsDefaultStateValueLabel;
    private readonly Label detailsDescriptionLabel;
    private readonly Label detailsDescriptionValueLabel;
    private readonly Button addButton;
    private readonly Button enableButton;
    private readonly Button disableButton;
    private readonly Button toggleButton;
    private readonly Button refreshButton;
    private readonly Button exitButton;

    private LocalizedText text;

    public MainForm()
    {
        settings = AppSettingsStore.Load();

        Text = "Replay Firewall Tool";
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(760, 620);
        MinimumSize = new Size(760, 620);
        Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
        ShowIcon = true;

        appIcon = ResolveAppIcon();
        if (appIcon != null)
        {
            Icon = appIcon;
        }

        toastNotifier = new ToastNotifier(appIcon ?? SystemIcons.Application);

        TableLayoutPanel layout = new TableLayoutPanel();
        layout.Dock = DockStyle.Fill;
        layout.Padding = new Padding(16);
        layout.RowCount = 8;
        layout.ColumnCount = 2;
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 120F));

        languageLabel = BuildLabel("Language");

        TableLayoutPanel headerPanel = new TableLayoutPanel();
        headerPanel.Dock = DockStyle.Fill;
        headerPanel.ColumnCount = 3;
        headerPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        headerPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 68F));
        headerPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 72F));

        languageSelector = new ComboBox();
        languageSelector.DropDownStyle = ComboBoxStyle.DropDownList;
        languageSelector.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        languageSelector.Margin = new Padding(0, 4, 8, 4);

        versionCaptionLabel = BuildValue("Version");
        versionCaptionLabel.TextAlign = ContentAlignment.MiddleRight;
        versionValueLabel = BuildValue(AppText.CurrentVersion);
        versionValueLabel.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point);

        headerPanel.Controls.Add(languageSelector, 0, 0);
        headerPanel.Controls.Add(versionCaptionLabel, 1, 0);
        headerPanel.Controls.Add(versionValueLabel, 2, 0);

        ruleLabel = BuildLabel("Rule");
        ruleValueLabel = BuildValue(FirewallRuleConfig.RuleName);
        remoteIpLabel = BuildLabel("Remote IP");
        remoteIpValueLabel = BuildValue(FirewallRuleConfig.RemoteIp);
        matchCountLabel = BuildLabel("Matching rules");
        countValueLabel = BuildValue("-");

        statusLabel = BuildLabel("Status");

        TableLayoutPanel statusPanel = new TableLayoutPanel();
        statusPanel.Dock = DockStyle.Fill;
        statusPanel.RowCount = 2;
        statusPanel.ColumnCount = 1;
        statusPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
        statusPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        statusBadgeLabel = new Label();
        statusBadgeLabel.AutoSize = false;
        statusBadgeLabel.Dock = DockStyle.Left;
        statusBadgeLabel.TextAlign = ContentAlignment.MiddleCenter;
        statusBadgeLabel.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point);
        statusBadgeLabel.ForeColor = Color.White;
        statusBadgeLabel.Padding = new Padding(12, 4, 12, 4);
        statusBadgeLabel.Margin = new Padding(0, 2, 0, 6);

        statusDescriptionLabel = new Label();
        statusDescriptionLabel.Dock = DockStyle.Fill;
        statusDescriptionLabel.ForeColor = Color.FromArgb(70, 70, 70);

        statusPanel.Controls.Add(statusBadgeLabel, 0, 0);
        statusPanel.Controls.Add(statusDescriptionLabel, 0, 1);

        notePanel = new Panel();
        notePanel.Dock = DockStyle.Fill;
        notePanel.Padding = new Padding(12, 10, 12, 10);

        noteLabel = new Label();
        noteLabel.Dock = DockStyle.Fill;
        noteLabel.ForeColor = Color.FromArgb(55, 55, 55);

        notePanel.Controls.Add(noteLabel);

        detailsGroup = new GroupBox();
        detailsGroup.Dock = DockStyle.Fill;
        detailsGroup.Padding = new Padding(12, 14, 12, 12);

        TableLayoutPanel detailsLayout = new TableLayoutPanel();
        detailsLayout.Dock = DockStyle.Fill;
        detailsLayout.RowCount = 8;
        detailsLayout.ColumnCount = 2;
        detailsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170F));
        detailsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        detailsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
        detailsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
        detailsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
        detailsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
        detailsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
        detailsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
        detailsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
        detailsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

        detailsSummaryLabel = BuildLabel("What does this rule do?");
        detailsSummaryValueLabel = BuildValue(string.Empty);
        detailsSummaryValueLabel.MaximumSize = new Size(0, 44);

        detailsRuleNameLabel = BuildLabel("Rule name");
        detailsRuleNameValueLabel = BuildValue(FirewallRuleConfig.RuleName);
        detailsDirectionLabel = BuildLabel("Direction");
        detailsDirectionValueLabel = BuildValue(FirewallRuleConfig.Direction);
        detailsActionLabel = BuildLabel("Action");
        detailsActionValueLabel = BuildValue(FirewallRuleConfig.Action);
        detailsProtocolLabel = BuildLabel("Protocol");
        detailsProtocolValueLabel = BuildValue(FirewallRuleConfig.Protocol);
        detailsProfilesLabel = BuildLabel("Profiles");
        detailsProfilesValueLabel = BuildValue(FirewallRuleConfig.Profiles);
        detailsDefaultStateLabel = BuildLabel("Default state");
        detailsDefaultStateValueLabel = BuildValue(FirewallRuleConfig.DefaultCreationState);
        detailsDescriptionLabel = BuildLabel("Description");
        detailsDescriptionValueLabel = BuildValue(FirewallRuleConfig.Description);

        detailsLayout.Controls.Add(detailsSummaryLabel, 0, 0);
        detailsLayout.Controls.Add(detailsSummaryValueLabel, 1, 0);
        detailsLayout.Controls.Add(detailsRuleNameLabel, 0, 1);
        detailsLayout.Controls.Add(detailsRuleNameValueLabel, 1, 1);
        detailsLayout.Controls.Add(detailsDirectionLabel, 0, 2);
        detailsLayout.Controls.Add(detailsDirectionValueLabel, 1, 2);
        detailsLayout.Controls.Add(detailsActionLabel, 0, 3);
        detailsLayout.Controls.Add(detailsActionValueLabel, 1, 3);
        detailsLayout.Controls.Add(detailsProtocolLabel, 0, 4);
        detailsLayout.Controls.Add(detailsProtocolValueLabel, 1, 4);
        detailsLayout.Controls.Add(detailsProfilesLabel, 0, 5);
        detailsLayout.Controls.Add(detailsProfilesValueLabel, 1, 5);
        detailsLayout.Controls.Add(detailsDefaultStateLabel, 0, 6);
        detailsLayout.Controls.Add(detailsDefaultStateValueLabel, 1, 6);
        detailsLayout.Controls.Add(detailsDescriptionLabel, 0, 7);
        detailsLayout.Controls.Add(detailsDescriptionValueLabel, 1, 7);

        detailsGroup.Controls.Add(detailsLayout);

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
        layout.Controls.Add(headerPanel, 1, 0);
        layout.Controls.Add(ruleLabel, 0, 1);
        layout.Controls.Add(ruleValueLabel, 1, 1);
        layout.Controls.Add(remoteIpLabel, 0, 2);
        layout.Controls.Add(remoteIpValueLabel, 1, 2);
        layout.Controls.Add(statusLabel, 0, 3);
        layout.Controls.Add(statusPanel, 1, 3);
        layout.Controls.Add(matchCountLabel, 0, 4);
        layout.Controls.Add(countValueLabel, 1, 4);
        layout.Controls.Add(notePanel, 0, 5);
        layout.SetColumnSpan(notePanel, 2);
        layout.Controls.Add(detailsGroup, 0, 6);
        layout.SetColumnSpan(detailsGroup, 2);
        layout.Controls.Add(buttons, 0, 7);
        layout.SetColumnSpan(buttons, 2);

        Controls.Add(layout);

        PopulateLanguages();
        ApplyLanguage(settings.PreferredLanguage);

        Load += MainFormLoad;
        FormClosed += MainFormClosed;
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

    private Icon ResolveAppIcon()
    {
        Icon extracted = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
        if (extracted != null)
        {
            return extracted;
        }

        return SystemIcons.Application;
    }

    private void MainFormLoad(object sender, EventArgs e)
    {
        RefreshView();
        BeginInvoke(new MethodInvoker(ShowWizardIfNeeded));
    }

    private void MainFormClosed(object sender, FormClosedEventArgs e)
    {
        toastNotifier.Dispose();
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

        settings.PreferredLanguage = option.Language;
        AppSettingsStore.Save(settings);
        ApplyLanguage(option.Language);
    }

    private void ApplyLanguage(UiLanguage language)
    {
        text = AppText.For(language);

        Text = text.WindowTitle;
        languageLabel.Text = text.LanguageLabel;
        versionCaptionLabel.Text = text.VersionLabel;
        versionValueLabel.Text = text.FormatVersion(AppText.CurrentVersion);
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
        detailsGroup.Text = text.DetailsGroupTitle;
        detailsSummaryLabel.Text = text.DetailsSummaryLabel;
        detailsSummaryValueLabel.Text = text.DetailsSummaryText;
        detailsRuleNameLabel.Text = text.DetailsRuleNameLabel;
        detailsDirectionLabel.Text = text.DetailsDirectionLabel;
        detailsActionLabel.Text = text.DetailsActionLabel;
        detailsProtocolLabel.Text = text.DetailsProtocolLabel;
        detailsProfilesLabel.Text = text.DetailsProfilesLabel;
        detailsDefaultStateLabel.Text = text.DetailsDefaultStateLabel;
        detailsDescriptionLabel.Text = text.DetailsDescriptionLabel;

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

            countValueLabel.Text = text.FormatRuleCount(info);
            statusBadgeLabel.Text = text.FormatStatus(info);
            statusDescriptionLabel.Text = text.GetStatusExplanation(info.State);

            ApplyStatusColors(info.State);

            addButton.Enabled = !info.Exists;
            enableButton.Enabled = info.Exists && info.EnabledCount < info.Count;
            disableButton.Enabled = info.Exists && info.EnabledCount > 0;
            toggleButton.Enabled = info.Exists;
        }
        catch (Exception ex)
        {
            countValueLabel.Text = "-";
            statusBadgeLabel.Text = text.ErrorCaption;
            statusDescriptionLabel.Text = text.FirewallApiError;
            statusBadgeLabel.BackColor = Color.FromArgb(176, 0, 32);
            addButton.Enabled = true;
            enableButton.Enabled = false;
            disableButton.Enabled = false;
            toggleButton.Enabled = false;
            ShowError(ex.Message);
        }
    }

    private void ApplyStatusColors(RuleState state)
    {
        Color badgeColor;
        Color panelColor;

        switch (state)
        {
            case RuleState.Enabled:
                badgeColor = Color.FromArgb(46, 125, 50);
                panelColor = Color.FromArgb(232, 245, 233);
                break;
            case RuleState.Disabled:
                badgeColor = Color.FromArgb(97, 97, 97);
                panelColor = Color.FromArgb(245, 245, 245);
                break;
            case RuleState.Mixed:
                badgeColor = Color.FromArgb(239, 108, 0);
                panelColor = Color.FromArgb(255, 243, 224);
                break;
            default:
                badgeColor = Color.FromArgb(198, 40, 40);
                panelColor = Color.FromArgb(255, 235, 238);
                break;
        }

        statusBadgeLabel.BackColor = badgeColor;
        notePanel.BackColor = panelColor;
    }

    private void ShowWizardIfNeeded()
    {
        FirewallRuleInfo info;

        try
        {
            info = FirewallRuleService.GetRuleInfo();
        }
        catch
        {
            return;
        }

        if (info.Exists || settings.HideWizard)
        {
            return;
        }

        using (WizardForm wizard = new WizardForm(text))
        {
            DialogResult result = wizard.ShowDialog(this);

            if (wizard.HideOnStartup)
            {
                settings.HideWizard = true;
                AppSettingsStore.Save(settings);
            }

            if (result == DialogResult.OK)
            {
                RunAction(
                    delegate { FirewallRuleService.AddRuleIfMissing(false); },
                    text.RuleAddedMessage);
            }
        }
    }

    private void AddRuleClicked(object sender, EventArgs e)
    {
        RunAction(delegate { FirewallRuleService.AddRuleIfMissing(false); }, text.RuleAddedMessage);
    }

    private void EnableClicked(object sender, EventArgs e)
    {
        RunAction(delegate { FirewallRuleService.SetRuleEnabled(true, text); }, text.RuleEnabledMessage);
    }

    private void DisableClicked(object sender, EventArgs e)
    {
        RunAction(delegate { FirewallRuleService.SetRuleEnabled(false, text); }, text.RuleDisabledMessage);
    }

    private void ToggleClicked(object sender, EventArgs e)
    {
        RunAction(delegate { FirewallRuleService.ToggleRule(text); }, text.RuleToggledMessage);
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
            toastNotifier.ShowInfo(text.ToastSuccessTitle, successMessage);
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }

    private void ShowError(string message)
    {
        string displayMessage = message;

        if (string.Equals(message, FirewallRuleService.FirewallApiUnavailableCode, StringComparison.Ordinal))
        {
            displayMessage = text.FirewallApiError;
        }

        MessageBox.Show(displayMessage, text.ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
