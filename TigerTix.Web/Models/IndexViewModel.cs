﻿using System.ComponentModel.DataAnnotations;

namespace TigerTix.Web.Models
{
    public class IndexViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
		public string FirstName { get; set; }
        [Required]
		public string LastName { get; set; }
	}
}
