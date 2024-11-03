namespace TigerTix.Web.Data.Entities
{
    public interface ITicketRepository
    {
        // Creates an entry in the database's User table.
        void SaveTicket(Ticket tigerTixTicket);

        // Updates an entry in the database's User table. Pass in a valid User entity returned by GetAllUsers, GetUserByID, etc.
        void UpdateTicket(Ticket tigerTixTicket);

        // Deletes an entry in the database's User table. Pass in a valid User entity returned by GetAllUsers, GetUserByID, etc.
        void DeleteTicket(Ticket tigerTixTicket);

        // Queries and returns all entries from the database's User table.
        IEnumerable<Ticket> GetAllTickets();

        // Queries and returns a specific user from the database's User table by ID.
        Ticket GetTicketByID(int ticketID);

        // Saves all changes to the database and returns whether any entries were affected.
        bool SaveAll();
    }
}