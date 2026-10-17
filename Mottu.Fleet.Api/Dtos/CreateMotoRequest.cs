namespace Mottu.Fleet.Api.Dtos;

public class CreateMotoRequest
{
    public string Placa { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public string Status { get; set; } = "disponivel";
    public double Quilometragem { get; set; }
    public int Bateria { get; set; }
}