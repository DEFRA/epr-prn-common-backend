﻿using System.ComponentModel.DataAnnotations;

namespace EPR.PRN.Backend.API.Commands;
public class UpdateRegulatorRegistrationTaskCommand: UpdateRegulatorTaskCommandBase
{
    [Required]
    public required Guid RegistrationId { get; set; }

}