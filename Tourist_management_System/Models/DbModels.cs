using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Tourist_management_System.Models
{
    public class Tourist
    {
        public Tourist()
        {
            this.BookingEntries = new List<BookingEntry>();
        }
        public int TouristId { get; set; }
        [Required, StringLength(50, ErrorMessage = "Tourist name is required!!"), Display(Name = "Tourist Name")]
        public string TouristName { get; set; } = default!;
        [Required, Column(TypeName = "date"), Display(Name = "Date of Birth"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }
        public int Age { get; set; }
        public string? Picture { get; set; }
        public bool MaritalStatus { get; set; }
        //
        public virtual ICollection<BookingEntry> BookingEntries { get; set; }

    }
    public class Spot
    {
        public Spot()
        {
            this.BookingEntries = new List<BookingEntry>();
        }
        public int SpotId { get; set; }
        [Required, StringLength(50, ErrorMessage = "Spot name is required!!"), Display(Name = "Spot Name")]
        public string SpotName { get; set; } = default!;
        //nev

        public virtual ICollection<BookingEntry> BookingEntries { get; set; }
    }
    public class BookingEntry
    {
        public int BookingEntryId { get; set; }
        [ForeignKey("Tourist")]
        public int TouristId { get; set; }
        [ForeignKey("Spot")]
        public int SpotId { get; set; }

        //nev
        public virtual Tourist Tourist { get; set; } = default!;
        public virtual Spot Spot { get; set; } = default!;

    }
    public class TravelDbContext :DbContext
    {
        public TravelDbContext(DbContextOptions<TravelDbContext> options) : base(options) { }

        public DbSet<Tourist> Tourists { get; set; }
        public DbSet<Spot> Spots { get; set; }
        public DbSet<BookingEntry> BookingEntries { get; set; }
    }
}