namespace EPR.Accreditation.API.Helpers.Comparers
{
    using EPR.Accreditation.API.Common.Dtos;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Class used to compare DTO model Waste Code objects together
    /// </summary>
    public class WasteCodeDtoComparer : IEqualityComparer<WasteCode>
    {
        /// <summary>
        /// Implementation of Interface Equals comparison
        /// </summary>
        /// <param name="x">First waste code</param>
        /// <param name="y">Second waste code to compare with</param>
        /// <returns>true if they are the same, else false</returns>
        public bool Equals(WasteCode x, WasteCode y)
        {
            return x.Code == y.Code;
        }

        /// <summary>
        /// Calcuates an appropriate hash code for a waste code
        /// </summary>
        /// <param name="obj">Waste Code object to generate hash code for</param>
        /// <returns>Hash code value</returns>
        public int GetHashCode([DisallowNull] WasteCode obj)
        {
            return obj.WasteCodeTypeId.GetHashCode() ^ obj.Code.GetHashCode();
        }
    }
}