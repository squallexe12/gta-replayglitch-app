using System;
using System.Globalization;

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
    public const string Direction = "Outbound";
    public const string Action = "Block";
    public const string Protocol = "Any";
    public const string Profiles = "All";
    public const string DefaultCreationState = "Disabled";
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
    public string ToastSuccessTitle { get; set; }
    public string RuleMissingError { get; set; }
    public string FirewallApiError { get; set; }
    public string StatusMissing { get; set; }
    public string StatusDisabled { get; set; }
    public string StatusEnabled { get; set; }
    public string StatusMixedFormat { get; set; }
    public string StatusExplanationMissing { get; set; }
    public string StatusExplanationDisabled { get; set; }
    public string StatusExplanationEnabled { get; set; }
    public string StatusExplanationMixed { get; set; }
    public string CountMissing { get; set; }
    public string CountFormat { get; set; }
    public string DetailsGroupTitle { get; set; }
    public string DetailsSummaryLabel { get; set; }
    public string DetailsSummaryText { get; set; }
    public string DetailsRuleNameLabel { get; set; }
    public string DetailsDirectionLabel { get; set; }
    public string DetailsActionLabel { get; set; }
    public string DetailsProtocolLabel { get; set; }
    public string DetailsProfilesLabel { get; set; }
    public string DetailsDefaultStateLabel { get; set; }
    public string DetailsDescriptionLabel { get; set; }
    public string WizardTitle { get; set; }
    public string WizardIntro { get; set; }
    public string WizardSteps { get; set; }
    public string WizardCreateButton { get; set; }
    public string WizardLaterButton { get; set; }
    public string WizardDontShowAgain { get; set; }
    public string VersionTextFormat { get; set; }
    public string VersionLabel { get; set; }

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

    public string FormatVersion(string version)
    {
        return string.Format(CultureInfo.InvariantCulture, VersionTextFormat, version);
    }

    public string GetStatusExplanation(RuleState state)
    {
        switch (state)
        {
            case RuleState.Enabled:
                return StatusExplanationEnabled;
            case RuleState.Disabled:
                return StatusExplanationDisabled;
            case RuleState.Mixed:
                return StatusExplanationMixed;
            default:
                return StatusExplanationMissing;
        }
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

internal sealed class AppSettings
{
    public UiLanguage PreferredLanguage { get; set; }
    public bool HideWizard { get; set; }
}
