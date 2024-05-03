﻿namespace EPR.Accreditation.API.Common.Dtos
{
    public class CheckAnswersSectionDto
    {
        //public Guid Id { get; set; }

        public string Title { get; set; }

        public bool Completed { get; set; }

        public List<CheckAnswersSectionRow> SectionRows { get; set; }

        //public Dictionary<string, string> QueryStringRouteData { get; set; } = new();
    }
}