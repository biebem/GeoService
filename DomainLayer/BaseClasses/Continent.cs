﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainLayer.BaseClasses
{
    public class Continent
    {
        /// <summary>
        /// continentId
        /// </summary>
        public uint Id { get; set; }
        private string _name;
        /// <summary>
        /// coninent name = unique
        /// </summary>
        public string Name { get => _name; 
            private set
            {
                if (string.IsNullOrEmpty(value))
                { 
                    throw new ArgumentException("Name can't be null or empty.");
                }
                _name = value; 
            } 
        }
        /// <summary>
        /// population generated from populations of all countries
        /// </summary>
        public ulong Population { get; set; } = 0;
        private List<Country> _countries = new List<Country>();
        /// <summary>
        /// countries in continent
        /// </summary>
        public IReadOnlyList<Country> Countries { get => _countries.AsReadOnly();}

        public Continent() { }
        /// <summary>
        /// constructor of continent with no countries
        /// </summary>
        /// <param name="name">Name of continent</param>
        public Continent(string name)
        {
            Name = name;
        }

        /// <summary>
        /// method to add country and automatically updates population
        /// </summary>
        /// <param name="name">Name of country</param>
        /// <param name="population">Population of country</param>
        /// <param name="surface">surface area of country</param>
        public void AddCountry(Country country) 
        {
            CheckIfSameContinent(country);
            country.Id = (uint)_countries.Count;
            _countries.Add(country);
            Population += country.Population;
        }
        /// <summary>
        /// method to delete country and automatically updates population
        /// </summary>
        /// <param name="country"></param>
        public void DeleteCountry(Country country)
        {
            CheckCountryForNull(country);
            bool removed = _countries.Remove(country);
            if (removed == false) 
            {
                    throw new ArgumentException($"country is not in {Name}");
            }
            Population -= country.Population;
        }
        /// <summary>
        /// checks country for null value and returns exception if true
        /// </summary>
        /// <param name="country">Country to check</param>
        private void CheckCountryForNull(Country country) 
        {
            if (country == null)
                throw new ArgumentException("country can't be null");
        }

        private void CheckIfSameContinent(Country country) 
        {
            if (country.Continent != this)
                throw new ArgumentException("country is not from this continent.");
        }

        #region equals
        public override bool Equals(object obj)
        {
            return obj is Continent continent &&
                   Name == continent.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }
        #endregion
    }
}
