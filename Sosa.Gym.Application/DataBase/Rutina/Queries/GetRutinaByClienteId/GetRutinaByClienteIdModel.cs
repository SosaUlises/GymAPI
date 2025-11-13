namespace Sosa.Gym.Application.DataBase.Rutina.Queries.GetRutinaByClienteId
{
    public class GetRutinaByClienteIdModel
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int ClienteId { get; set; }
    }
}
