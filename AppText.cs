using System;
using System.Globalization;

internal static class AppText
{
    public static string CurrentVersion
    {
        get { return "1.2.0"; }
    }

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
                    "Bu arac belirli bir outbound Windows Firewall rule'unu yonetir. " +
                    "Rule yoksa sihirbaz veya 'Kurali Ekle' butonu ile kapali durumda olusturabilirsin.",
                AddRuleButton = "Kurali Ekle",
                EnableButton = "Ac",
                DisableButton = "Kapat",
                ToggleButton = "Toggle",
                RefreshButton = "Yenile",
                ExitButton = "Cikis",
                InfoCaption = "Bilgi",
                ErrorCaption = "Hata",
                RuleAddedMessage = "Rule olusturuldu ve kapali durumda hazir.",
                RuleEnabledMessage = "Rule aktif edildi.",
                RuleDisabledMessage = "Rule kapatildi.",
                RuleToggledMessage = "Rule durumu degistirildi.",
                ToastSuccessTitle = "Islem Tamamlandi",
                RuleMissingError = "Kural bulunamadi. Once 'Kurali Ekle' secenegini kullan.",
                FirewallApiError = "Windows Firewall API baslatilamadi.",
                StatusMissing = "Kural Yok",
                StatusDisabled = "Kapali",
                StatusEnabled = "Acik",
                StatusMixedFormat = "Karisik ({0}/{1} acik)",
                StatusExplanationMissing = "Rule bu sistemde bulunmuyor. Ilk acilis sihirbazi veya 'Kurali Ekle' ile olusturabilirsin.",
                StatusExplanationDisabled = "Rule mevcut fakat devre disi. Su an hedef IP icin bloklama uygulanmiyor.",
                StatusExplanationEnabled = "Rule aktif. Hedef IP'ye giden outbound trafik su an bloklaniyor.",
                StatusExplanationMixed = "Birden fazla eslesen rule var ve hepsi ayni durumda degil. Temizlik kontrolu onerilir.",
                CountMissing = "0 adet",
                CountFormat = "{0} adet, {1} aktif",
                DetailsGroupTitle = "Detay Gorunumu",
                DetailsSummaryLabel = "Bu rule ne yapiyor?",
                DetailsSummaryText =
                    "Bu rule, 192.81.241.171 IP adresine giden outbound trafigi tum profillerde bloklar. " +
                    "Yeni olusturuldugunda varsayilan olarak kapali gelir.",
                DetailsRuleNameLabel = "Rule adi",
                DetailsDirectionLabel = "Yon",
                DetailsActionLabel = "Aksiyon",
                DetailsProtocolLabel = "Protokol",
                DetailsProfilesLabel = "Profiller",
                DetailsDefaultStateLabel = "Varsayilan durum",
                DetailsDescriptionLabel = "Aciklama",
                WizardTitle = "Ilk Aclis Sihirbazi",
                WizardIntro = "Bu bilgisayarda hedef firewall rule'u bulunmuyor. Asagidaki adimlarla guvenli sekilde olusturabilirsin.",
                WizardSteps =
                    "1. 'Simdi Olustur' butonu rule'u olusturur.\r\n" +
                    "2. Rule ilk etapta kapali olarak eklenir.\r\n" +
                    "3. Ana ekranda detaylarini ve durum rengini gorebilirsin.\r\n" +
                    "4. Ihtiyacin oldugunda 'Ac' butonuyla bloklamayi aktiflestirirsin.",
                WizardCreateButton = "Simdi Olustur",
                WizardLaterButton = "Daha Sonra",
                WizardDontShowAgain = "Bu rehberi tekrar otomatik gosterme",
                VersionLabel = "Surum",
                VersionTextFormat = "v{0}"
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
                "This tool manages a targeted outbound Windows Firewall rule. " +
                "If the rule is missing, you can create it from the first-run wizard or the 'Add Rule' button.",
            AddRuleButton = "Add Rule",
            EnableButton = "Enable",
            DisableButton = "Disable",
            ToggleButton = "Toggle",
            RefreshButton = "Refresh",
            ExitButton = "Exit",
            InfoCaption = "Information",
            ErrorCaption = "Error",
            RuleAddedMessage = "The rule was created and is ready in a disabled state.",
            RuleEnabledMessage = "The rule was enabled.",
            RuleDisabledMessage = "The rule was disabled.",
            RuleToggledMessage = "The rule state was toggled.",
            ToastSuccessTitle = "Action Completed",
            RuleMissingError = "The rule was not found. Use 'Add Rule' first.",
            FirewallApiError = "Windows Firewall API could not be started.",
            StatusMissing = "Missing",
            StatusDisabled = "Disabled",
            StatusEnabled = "Enabled",
            StatusMixedFormat = "Mixed ({0}/{1} enabled)",
            StatusExplanationMissing = "The rule does not exist on this system yet. You can create it from the wizard or by clicking 'Add Rule'.",
            StatusExplanationDisabled = "The rule exists but is turned off. Traffic to the target IP is not currently blocked.",
            StatusExplanationEnabled = "The rule is active. Outbound traffic to the target IP is currently blocked.",
            StatusExplanationMixed = "More than one matching rule was found and they do not share the same enabled state.",
            CountMissing = "0 rules",
            CountFormat = "{0} rules, {1} enabled",
            DetailsGroupTitle = "Details View",
            DetailsSummaryLabel = "What does this rule do?",
            DetailsSummaryText =
                "This rule blocks outbound traffic to 192.81.241.171 across all firewall profiles. " +
                "When created from this app, it starts in a disabled state so you can review it before enabling it.",
            DetailsRuleNameLabel = "Rule name",
            DetailsDirectionLabel = "Direction",
            DetailsActionLabel = "Action",
            DetailsProtocolLabel = "Protocol",
            DetailsProfilesLabel = "Profiles",
            DetailsDefaultStateLabel = "Default state",
            DetailsDescriptionLabel = "Description",
            WizardTitle = "First-Run Wizard",
            WizardIntro = "The target firewall rule was not found on this computer. Follow these steps to create it safely.",
            WizardSteps =
                "1. Click 'Create Now' to add the rule.\r\n" +
                "2. The rule will be created in a disabled state.\r\n" +
                "3. Review the details and status color in the main window.\r\n" +
                "4. Enable it when you actually want blocking to apply.",
            WizardCreateButton = "Create Now",
            WizardLaterButton = "Maybe Later",
            WizardDontShowAgain = "Do not automatically show this guide again",
            VersionLabel = "Version",
            VersionTextFormat = "v{0}"
        };
    }
}
