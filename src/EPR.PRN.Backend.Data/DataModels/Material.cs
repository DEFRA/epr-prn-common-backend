﻿using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.Data.DataModels
{
    public class Material
    {
        [Key]
        [MaxLength(20)]
        public required string MaterialName { get; set; }

        [MaxLength(3)]
        public required string MaterialCode { get; set; }
    }
}
