

namespace Common;

public class CustomerEvent
{
    public CustomerEvent(Guid guid, string firstName, string lastName)
    {
        Guid = guid;
        FirstName = firstName;
        LastName = lastName;
    }

    public Guid Guid{get; set;}
    public string FirstName {get;set;}
    public string LastName {get;set;}
}