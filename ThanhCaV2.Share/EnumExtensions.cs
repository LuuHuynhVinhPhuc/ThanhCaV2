namespace ThanhCaV2.Share.Enums;

public static class EnumExtensions
{
    public static string ToFriendlyString(this HymnSeason season)
    {
        return season switch
        {
            HymnSeason.ThuongNien => "Thường Niên",
            HymnSeason.PhucSinh => "Phục Sinh",
            HymnSeason.MuaChay => "Mùa Chay",
            HymnSeason.VongGiangSinh => "Vọng - Giáng Sinh",
            _ => season.ToString()
        };
    }

    public static string ToFriendlyString(this LyricType type)
    {
        return type switch
        {
            LyricType.PhienKhuc => "Phiên khúc",
            LyricType.DiepKhuc => "Điệp khúc",
            _ => type.ToString()
        };
    }
}
