using System;
using System.Collections.Generic;
using System.Linq;

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
