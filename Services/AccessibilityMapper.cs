using Server.Models;

namespace Server.Services
{
    public static class AccessibilityMapper
    {
        // =========================
        // יחיד (נשאר לשימושים ישנים)
        // =========================
        public static Accessibility Map(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return Accessibility.None;

            var value = input
                .Trim()
                .ToLower()
                .Replace(" ", "");

            return value switch
            {
                "0" or "none" or "ללא" => Accessibility.None,
                "1" or "deaf" or "deafness" or "חרשות" => Accessibility.Deafness,
                "2" or "blind" or "עיוורון" => Accessibility.Blind,
                "3" or "disabled" or "נכות" or "מוגבלות" => Accessibility.Disabled,
                _ => Accessibility.None
            };
        }

       
          public static List<Accessibility> Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return new List<Accessibility>();

        return input
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(x => AccessibilityMapper.Map(x))
            .Distinct()
            .ToList();
    }
    }
}