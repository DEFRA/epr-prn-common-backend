﻿namespace ObligationCalculationDemoUI.Models
{
    public class ObligationCalculation
    {
        public int Tonnage { get; set; }
        public ObligationCalculationMaterial Material { get; set; }
        public int SelectedYear { get; set; }
    }

    public enum ObligationCalculationMaterial
    {
        Steel,
        Glass,
        Wood,
        Paper,
        Aluminium,
        Plastic
    }

    public class RecyclingTarget
    {
        public int Year { get; set; }
        public ObligationCalculationMaterial Material { get; set; }
        public double Number { get; set; }
    }

    public class RecyclingTargets
    {
        public List<RecyclingTarget> Entries { get; set; }
        public RecyclingTarget NewEntry { get; set; } = new RecyclingTarget();
    }
}
