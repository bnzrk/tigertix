namespace TigerTix.Web.Data.Entities
{
    public interface IEventRepository
    {
        // Creates an entry in the database's User table.
        void SaveEvent(Event tigerTixEvent);

        // Updates an entry in the database's User table. Pass in a valid User entity returned by GetAllUsers, GetUserByID, etc.
        void UpdateEvent(Event tigerTixEvent);

        // Deletes an entry in the database's User table. Pass in a valid User entity returned by GetAllUsers, GetUserByID, etc.
        void DeleteEvent(Event tigerTixEvent);

        // Queries and returns all entries from the database's User table.
        IEnumerable<Event> GetAllEvents();

        // Queries and returns a specific user from the database's User table by ID.
        Event GetEventByID(int eventID);

        // Saves all changes to the database and returns whether any entries were affected.
        bool SaveAll();
    }
}