namespace CLITaskManager.Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string entityName, int id)
         : base($"{entityName} with ID {id} was not found.")
    {
        
    }
    public NotFoundException(string entityName, string identifier)
         : base($"{entityName} with Identifier {identifier} was not found.")
    {
        
    }
}