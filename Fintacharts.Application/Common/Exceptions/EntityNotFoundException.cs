namespace Fintacharts.Application.Common.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException() : base("Entity not found")
        {
        }

        public EntityNotFoundException(int id) : base($"Entity by {id} not found.")
        {
        }
    }
}
