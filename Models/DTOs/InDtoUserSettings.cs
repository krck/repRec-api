
namespace RepRecApi.Models.DTOs;

public class InDtoUserSettings
{
    public required string Id { get; set; }
    public required string SettingTimezone { get; set; }
    public required string SettingWeightUnit { get; set; }
    public required string SettingDistanceUnit { get; set; }
}
