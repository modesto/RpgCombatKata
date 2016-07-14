// From http://stackoverflow.com/questions/2357795/distance-feet-inches-class-or-struct

using System;

namespace RpgCombatKata.Core.Business.Map {
    public struct Distance : IEquatable<Distance>, IComparable<Distance>
    {
        private const double MetersPerKilometer = 1000.0;
        private const double CentimetersPerMeter = 100.0;
        private const double CentimetersPerInch = 2.54;
        private const double InchesPerFoot = 12.0;
        private const double FeetPerYard = 3.0;
        private const double FeetPerMeter = CentimetersPerMeter/(CentimetersPerInch*InchesPerFoot);
        private const double InchesPerMeter = CentimetersPerMeter/CentimetersPerInch;

        private readonly double meters;

        public Distance(double meters)
        {
            this.meters = meters;
        }

        public double TotalKilometers => meters / MetersPerKilometer;

        public double TotalMeters => meters;

        public double TotalCentimeters => meters * CentimetersPerMeter;

        public double TotalYards => meters * FeetPerMeter / FeetPerYard;

        public double TotalFeet => meters * FeetPerMeter;

        public double TotalInches => meters * InchesPerMeter;

        public static Distance FromKilometers(double value)
        {
            return new Distance(value * MetersPerKilometer);
        }

        public static Distance FromMeters(double value)
        {
            return new Distance(value);
        }

        public static Distance FromCentimeters(double value)
        {
            return new Distance(value / CentimetersPerMeter);
        }

        public static Distance FromYards(double value)
        {
            return new Distance(value * FeetPerYard / FeetPerMeter);
        }

        public static Distance FromFeet(double value)
        {
            return new Distance(value / FeetPerMeter);
        }

        public static Distance FromInches(double value)
        {
            return new Distance(value / InchesPerMeter);
        }

        public static Distance operator +(Distance a, Distance b)
        {
            return new Distance(a.meters + b.meters);
        }

        public static Distance operator -(Distance a, Distance b)
        {
            return new Distance(a.meters - b.meters);
        }

        public static Distance operator -(Distance a)
        {
            return new Distance(-a.meters);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Distance))
                return false;

            return Equals((Distance)obj);
        }

        public bool Equals(Distance other)
        {
            return meters == other.meters;
        }

        public int CompareTo(Distance other)
        {
            return meters.CompareTo(other.meters);
        }

        public override int GetHashCode()
        {
            return meters.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0}[m]", TotalMeters);
        }
    }
}