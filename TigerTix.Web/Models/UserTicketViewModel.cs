﻿using System.ComponentModel.DataAnnotations;
using TigerTix.Web.Data.Entities;

namespace TigerTix.Web.Models
{
    public class UserTicketViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Section { get; set; }
        [Required]
        public int Row { get; set; }
        [Required]
        public int SeatNumber { get; set; }
        [Required]
        public DateTime EventDate { get; set; }
        [Required]
        public string EventName { get; set; }
    }
}
