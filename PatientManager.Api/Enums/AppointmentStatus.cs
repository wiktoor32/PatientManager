using System.Text.Json.Serialization;

namespace PatientManager.Api.Enums;

[JsonConverter(typeof(JsonStringEnumConverter<AppointmentStatus>))]
public enum AppointmentStatus
{
    Unknown = 0,
    Scheduled = 1,
    Completed = 2,
    Cancelled = 3,
    NoShow = 4
}