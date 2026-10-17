namespace Mottu.Fleet.Api.Models;

public class Moto
{
    public int Id { get; set; }
    public string Placa { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public string Status { get; set; } = "disponivel";
    public double Quilometragem { get; set; }
    public int Bateria { get; set; }
    public DateTime UltimaAtualizacao { get; set; } = DateTime.UtcNow;
}