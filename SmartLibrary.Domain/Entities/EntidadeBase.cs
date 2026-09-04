namespace SmartLibrary.Domain.Entities
{
    public abstract class EntidadeBase
    {
        public int Id { get; protected set; }
        public DateTime DataCriacao { get; protected set; }
    }
}
