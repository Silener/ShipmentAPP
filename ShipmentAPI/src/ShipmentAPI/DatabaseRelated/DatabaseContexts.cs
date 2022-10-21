using Microsoft.EntityFrameworkCore;

public class DatabaseContexts : DbContext
{
    public DatabaseContexts(DbContextOptions<DatabaseContexts> options)
        : base(options)
    {
    }

    public DbSet<ShipmentAPI.DatabaseTables.ShipmentItem> ShipmentsTable { get; set; } = default!;
    public DbSet<ShipmentAPI.DatabaseTables.ShipmentDetailedDataItem> ShipmentDetailedDataTable { get; set; } = default!;
    public DbSet<ShipmentAPI.DatabaseTables.ShipmentAdditionalNotes> ShipmentAdditionalNotesTable { get; set; } = default!;
    public DbSet<ShipmentAPI.DatabaseTables.PostShipmentData> PostShipmentDataTable { get; set; } = default!;
}
