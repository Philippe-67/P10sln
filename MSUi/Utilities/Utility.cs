public static class Utility
{
    public static int CalculerAge(DateTime dateDeNaissance)
    {
        var aujourdhui = DateTime.Today;
        var age = aujourdhui.Year - dateDeNaissance.Year;

        if (dateDeNaissance.Date > aujourdhui.AddYears(-age))
        {
            age--;
        }

        return age;
    }
}