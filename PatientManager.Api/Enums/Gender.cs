using System.Text.Json.Serialization;

namespace PatientManager.Api.Enums;

[JsonConverter(typeof(JsonStringEnumConverter<Gender>))]
public enum Gender
{
    Unknown = 0,
    Male = 1,
    Female = 2
}