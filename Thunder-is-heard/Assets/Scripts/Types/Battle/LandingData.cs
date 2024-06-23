

[System.Serializable]
public class LandingData
{
    public Bector2Int[] zone;
    public int maxStaff;

    public LandingData(
        Bector2Int[] landingZone,
        int landingMaxStaff
        )
    {
        zone = landingZone;
        maxStaff = landingMaxStaff;
    }
}
