using System;
using System.IO;

internal static class AppSettingsStore
{
    private static readonly string SettingsDirectory =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ReplayFirewallTool");

    private static readonly string SettingsPath = Path.Combine(SettingsDirectory, "settings.ini");

    public static AppSettings Load()
    {
        AppSettings settings = new AppSettings
        {
            PreferredLanguage = AppText.GetDefaultLanguage(),
            HideWizard = false
        };

        if (!File.Exists(SettingsPath))
        {
            return settings;
        }

        foreach (string rawLine in File.ReadAllLines(SettingsPath))
        {
            if (string.IsNullOrWhiteSpace(rawLine) || rawLine.StartsWith("#", StringComparison.Ordinal))
            {
                continue;
            }

            string[] parts = rawLine.Split(new[] { '=' }, 2);
            if (parts.Length != 2)
            {
                continue;
            }

            string key = parts[0].Trim();
            string value = parts[1].Trim();

            if (string.Equals(key, "language", StringComparison.OrdinalIgnoreCase))
            {
                if (string.Equals(value, "tr", StringComparison.OrdinalIgnoreCase))
                {
                    settings.PreferredLanguage = UiLanguage.Turkish;
                }
                else if (string.Equals(value, "en", StringComparison.OrdinalIgnoreCase))
                {
                    settings.PreferredLanguage = UiLanguage.English;
                }
            }
            else if (string.Equals(key, "hideWizard", StringComparison.OrdinalIgnoreCase))
            {
                bool parsed;
                if (bool.TryParse(value, out parsed))
                {
                    settings.HideWizard = parsed;
                }
            }
        }

        return settings;
    }

    public static void Save(AppSettings settings)
    {
        Directory.CreateDirectory(SettingsDirectory);

        string[] lines =
        {
            "# Replay Firewall Tool user settings",
            "language=" + (settings.PreferredLanguage == UiLanguage.Turkish ? "tr" : "en"),
            "hideWizard=" + settings.HideWizard.ToString().ToLowerInvariant()
        };

        File.WriteAllLines(SettingsPath, lines);
    }
}
