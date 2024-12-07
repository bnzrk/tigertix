﻿namespace TigerTix.Web.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
        public bool IsActive { get; set; } = false;
    }
}
