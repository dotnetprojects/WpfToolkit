// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
// All other rights reserved.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Windows.Controls;

[assembly: SuppressMessage("General", "SWC1001:XmlDocumentationCommentShouldBeSpelledCorrectly", MessageId = "Bing", Justification = "Bing is a Microsoft web site.")]

namespace System.Windows.Controls.Samples
{
    /// <summary>
    /// An Airport class.
    /// </summary>
    public class Airport
    {
        /// <summary>
        /// Gets or sets the friendly airport name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets a sometimes shorter representation of the Name property.
        /// </summary>
        public string LimitedName
        {
            get
            {
                if (Name == null || Name.Length < 30)
                {
                    return Name;
                }

                return Name.Substring(0, 30) + "...";
            }
        }

        /// <summary>
        /// Gets or sets the airport city or cities name.
        /// </summary>
        public string City { get; set; }
        
        /// <summary>
        /// Gets or sets the state, region, or territory name.
        /// </summary>
        public string State { get; set; }
        
        /// <summary>
        /// Gets or sets the Federal Aviation Administration code.
        /// </summary>
        public string CodeFaa { get; set; }
        
        /// <summary>
        /// Gets or sets the International Air Transport Association code.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Iata", Justification = "This is a recognized abbreviation for the code.")]
        public string CodeIata { get; set; }
        
        /// <summary>
        /// Gets or sets the four-digit International Civil Aviation 
        /// Organization code.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Icao", Justification = "This is a recognized abbreviation for the code.")]
        public string CodeIcao { get; set; }

        /// <summary>
        /// Initializes a new Airport class instance.
        /// </summary>
        public Airport()
        {
        }

        /// <summary>
        /// Initializes a new Airport class instance. This is a data-entry 
        /// friendly constructor.
        /// </summary>
        /// <param name="city">The city or cities name.</param>
        /// <param name="state">The state or region.</param>
        /// <param name="faa">The Federal Aviation Administration code.</param>
        /// <param name="iata">The International Air Transport Association code.</param>
        /// <param name="icao">The four-digit International Civil Aviation
        /// Organization code.</param>
        /// <param name="airport">The friendly airport name.</param>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "icao", Justification = "This is a recognized abbreviation for the code.")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "iata", Justification = "This is a recognized abbreviation for the code.")]
        public Airport(string city, string state, string faa, string iata, string icao, string airport)
        {
            City = city;
            State = state;
            CodeFaa = faa;
            CodeIata = iata;
            CodeIcao = icao;
            Name = airport;
        }

        /// <summary>
        /// The code and name together.
        /// </summary>
        /// <returns>Returns a string.</returns>
        public override string ToString()
        {
            return CodeFaa;
        }

        /// <summary>
        /// Gets a collection of sample airports.
        /// </summary>
        public static ObjectCollection SampleAirports
        {
            get
            {
                ObjectCollection airports = new ObjectCollection();
                airports.Add(new Airport("Phoenix", "Arizona", "PHX", "PHX", "KPHX", "Phoenix Sky Harbor International Airport"));
                airports.Add(new Airport("Los Angeles", "California", "LAX", "LAX", "KLAX", "Los Angeles International Airport"));
                airports.Add(new Airport("San Diego", "California", "SAN", "SAN", "KSAN", "San Diego International Airport"));
                airports.Add(new Airport("San Francisco", "California", "SFO", "SFO", "KSFO", "San Francisco International Airport"));
                airports.Add(new Airport("Denver", "Colorado", "DEN", "DEN", "KDEN", "Denver International Airport"));
                airports.Add(new Airport("Fort Lauderdale", "Florida", "FLL", "FLL", "KFLL", "Fort Lauderdale-Hollywood International Airport"));
                airports.Add(new Airport("Miami", "Florida", "MIA", "MIA", "KMIA", "Miami International Airport"));
                airports.Add(new Airport("Orlando", "Florida", "MCO", "MCO", "KMCO", "Orlando International Airport"));
                airports.Add(new Airport("Tampa", "Florida", "TPA", "TPA", "KTPA", "Tampa International Airport"));
                airports.Add(new Airport("Atlanta", "Georgia", "ATL", "ATL", "KATL", "Hartsfield-Jackson Atlanta International Airport"));
                airports.Add(new Airport("Honolulu", "Hawaii", "HNL", "HNL", "PHNL", "Honolulu International Airport / Hickam AFB"));
                airports.Add(new Airport("Boise", "Idaho", "BOI", "BOI", "KBOI", "Boise Air Terminal (Gowen Field)"));
                airports.Add(new Airport("Chicago", "Illinois", "ORD", "ORD", "KORD", "Chicago O'Hare International Airport"));
                airports.Add(new Airport("Chicago", "Illinois", "MDW", "MDW", "KMDW", "Chicago Midway International Airport"));
                airports.Add(new Airport("Indianapolis", "Indiana", "IND", "IND", "KIND", "Indianapolis International Airport"));
                airports.Add(new Airport("Covington", "Kentucky", "CVG", "CVG", "KCVG", "Cincinnati/Northern Kentucky International Airport"));
                airports.Add(new Airport("Louisville", "Kentucky", "SDF", "SDF", "KSDF", "Louisville International Airport (Standiford Field)"));
                airports.Add(new Airport("New Orleans", "Louisiana", "MSY", "MSY", "KMSY", "Louis Armstrong New Orleans International Airport"));
                airports.Add(new Airport("Baltimore / Glen Burnie", "Maryland", "BWI", "BWI", "KBWI", "Baltimore-Washington International Thurgood Marshall Airport"));
                airports.Add(new Airport("Boston", "Massachusetts", "BOS", "BOS", "KBOS", "Gen. Edward Lawrence Logan International Airport"));
                airports.Add(new Airport("Detroit", "Michigan", "DTW", "DTW", "KDTW", "Detroit Metropolitan Wayne County Airport"));
                airports.Add(new Airport("Grand Rapids", "Michigan", "GRR", "GRR", "KGRR", "Gerald R. Ford International Airport"));
                airports.Add(new Airport("Minneapolis", "Minnesota", "MSP", "MSP", "KMSP", "Minneapolis-St. Paul International Airport (Wold-Chamberlain Field)"));
                airports.Add(new Airport("Kansas City", "Missouri", "MCI", "MCI", "KMCI", "Kansas City International Airport"));
                airports.Add(new Airport("St. Louis", "Missouri", "STL", "STL", "KSTL", "Lambert-St. Louis International Airport"));
                airports.Add(new Airport("Omaha", "Nebraska", "OMA", "OMA", "KOMA", "Eppley Airfield"));
                airports.Add(new Airport("Las Vegas", "Nevada", "LAS", "LAS", "KLAS", "McCarran International Airport"));
                airports.Add(new Airport("Reno", "Nevada", "RNO", "RNO", "KRNO", "Reno-Tahoe International Airport"));
                airports.Add(new Airport("Manchester", "New Hampshire", "MHT", "MHT", "KMHT", "Manchester-Boston Regional Airport"));
                airports.Add(new Airport("Newark", "New Jersey", "EWR", "EWR", "KEWR", "Newark Liberty International Airport"));
                airports.Add(new Airport("Albuquerque", "New Mexico", "ABQ", "ABQ", "KABQ", "Albuquerque International Sunport"));
                airports.Add(new Airport("Albany", "New York", "ALB", "ALB", "KALB", "Albany International Airport"));
                airports.Add(new Airport("Buffalo", "New York", "BUF", "BUF", "KBUF", "Buffalo Niagara International Airport"));
                airports.Add(new Airport("Islip", "New York", "ISP", "ISP", "KISP", "Long Island MacArthur Airport"));
                airports.Add(new Airport("New York", "New York", "JFK", "JFK", "KJFK", "John F. Kennedy International Airport"));
                airports.Add(new Airport("New York", "New York", "LGA", "LGA", "KLGA", "LaGuardia Airport"));
                airports.Add(new Airport("Rochester", "New York", "ROC", "ROC", "KROC", "Greater Rochester International Airport"));
                airports.Add(new Airport("Syracuse", "New York", "SYR", "SYR", "KSYR", "Syracuse Hancock International Airport"));
                airports.Add(new Airport("Charlotte", "North Carolina", "CLT", "CLT", "KCLT", "Charlotte/Douglas International Airport"));
                airports.Add(new Airport("Greensboro", "North Carolina", "GSO", "GSO", "KGSO", "Piedmont Triad International Airport"));
                airports.Add(new Airport("Raleigh", "North Carolina", "RDU", "RDU", "KRDU", "Raleigh-Durham International Airport"));
                airports.Add(new Airport("Cleveland", "Ohio", "CLE", "CLE", "KCLE", "Cleveland-Hopkins International Airport"));
                airports.Add(new Airport("Columbus", "Ohio", "CMH", "CMH", "KCMH", "Port Columbus International Airport"));
                airports.Add(new Airport("Dayton", "Ohio", "DAY", "DAY", "KDAY", "James M. Cox Dayton International Airport"));
                airports.Add(new Airport("Oklahoma City", "Oklahoma", "OKC", "OKC", "KOKC", "Will Rogers World Airport"));
                airports.Add(new Airport("Tulsa", "Oklahoma", "TUL", "TUL", "KTUL", "Tulsa International Airport"));
                airports.Add(new Airport("Portland", "Oregon", "PDX", "PDX", "KPDX", "Portland International Airport"));
                airports.Add(new Airport("Philadelphia", "Pennsylvania", "PHL", "PHL", "KPHL", "Philadelphia International Airport"));
                airports.Add(new Airport("Pittsburgh", "Pennsylvania", "PIT", "PIT", "KPIT", "Pittsburgh International Airport"));
                airports.Add(new Airport("Providence", "Rhode Island", "PVD", "PVD", "KPVD", "Theodore Francis Green State Airport"));
                airports.Add(new Airport("Memphis", "Tennessee", "MEM", "MEM", "KMEM", "Memphis International Airport"));
                airports.Add(new Airport("Nashville", "Tennessee", "BNA", "BNA", "KBNA", "Nashville International Airport (Berry Field)"));
                airports.Add(new Airport("Austin", "Texas", "AUS", "AUS", "KAUS", "Austin-Bergstrom International Airport"));
                airports.Add(new Airport("Dallas", "Texas", "DAL", "DAL", "KDAL", "Dallas Love Field"));
                airports.Add(new Airport("Dallas-Fort Worth", "Texas", "DFW", "DFW", "KDFW", "Dallas-Fort Worth International Airport"));
                airports.Add(new Airport("El Paso", "Texas", "ELP", "ELP", "KELP", "El Paso International Airport"));
                airports.Add(new Airport("Houston", "Texas", "IAH", "IAH", "KIAH", "George Bush Intercontinental Airport"));
                airports.Add(new Airport("Houston", "Texas", "HOU", "HOU", "KHOU", "William P. Hobby Airport"));
                airports.Add(new Airport("San Antonio", "Texas", "SAT", "SAT", "KSAT", "San Antonio International Airport"));
                airports.Add(new Airport("Salt Lake City", "Utah", "SLC", "SLC", "KSLC", "Salt Lake City International Airport"));
                airports.Add(new Airport("Norfolk", "Virginia", "ORF", "ORF", "KORF", "Norfolk International Airport"));
                airports.Add(new Airport("Richmond", "Virginia", "RIC", "RIC", "KRIC", "Richmond International Airport"));
                airports.Add(new Airport("Washington, D.C. (Arlington County)", "Virginia", "DCA", "DCA", "KDCA", "Ronald Reagan Washington National Airport"));
                airports.Add(new Airport("Washington, D.C. (Chantilly / Dulles)", "Virginia", "IAD", "IAD", "KIAD", "Washington Dulles International Airport"));
                airports.Add(new Airport("Seattle / Tacoma (SeaTac)", "Washington", "SEA", "SEA", "KSEA", "Seattle-Tacoma International Airport"));
                airports.Add(new Airport("Spokane", "Washington", "GEG", "GEG", "KGEG", "Spokane International Airport (Geiger Field)"));
                airports.Add(new Airport("Milwaukee", "Wisconsin", "MKE", "MKE", "KMKE", "General Mitchell International Airport"));
                airports.Add(new Airport("San Juan", "Puerto Rico", "SJU", "SJU", "TJSJ", "Luis Muñoz Marín International Airport"));
                return airports;
            }
        }
    }
}