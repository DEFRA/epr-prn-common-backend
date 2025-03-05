﻿namespace EPR.PRN.Backend.Data.Dto
{
    public class EprnTonnageResultsDto
    {
        public Guid OrganisationId { get; set; }

        public required string MaterialName { get; set; }

        public required string StatusName { get; set; }

        public int TotalTonnage { get; set; }
    }
}
