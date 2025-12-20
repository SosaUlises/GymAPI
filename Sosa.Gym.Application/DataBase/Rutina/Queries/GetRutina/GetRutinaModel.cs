namespace Sosa.Gym.Application.DataBase.Rutina.Queries.GetRutinaByClienteId
{
    public class GetRutinaModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
