namespace FreyaApi.Repository;

public class DatabaseContext : DbContext
{
    public DatabaseContext()
    {
        //for mock
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    public DbSet<FAQTable>? FAQ { get; set; }

    public DbSet<CompanyTable>? Company { get; set; }

    public DbSet<ClinicTable>? Clinic { get; set; }

    public DbSet<ClinicClient>? ClinicClients { get; set; }

    public DbSet<AppManagerTable>? AppManager { get; set; }

    public DbSet<EmployeeTable>? Employee { get; set; }

    public DbSet<ClientTable>? Client { get; set; }

    public DbSet<VideoTable>? Videos { get; set; }

    public DbSet<UltrasoundTable>? Ultrasounds { get; set; }

    public DbSet<SoundTable>? Sounds { get; set; }

    public DbSet<SubscriptionProductTable>? SubscriptionProducts { get; set; }

    public DbSet<SubscriptionPlanTable>? SubscriptionPlans { get; set; }

    public DbSet<SubscriptionPaymentTable>? SubscriptionPayments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<AppManagerTable>()
            .HasIndex(u => u.Email)
            .IsUnique();

        builder.Entity<EmployeeTable>()
            .HasIndex(u => u.Email)
            .IsUnique();

        builder.Entity<ClientTable>()
            .HasIndex(u => u.Email)
            .IsUnique();

        builder.Entity<ClinicClient>()
        .HasKey(t => new { t.ClinicId, t.ClientId });

        builder.Entity<ClinicClient>()
            .HasOne(pt => pt.Client)
            .WithMany(p => p.ClinicClients)
            .HasForeignKey(pt => pt.ClientId);

        builder.Entity<ClinicClient>()
            .HasOne(pt => pt.Clinic)
            .WithMany(t => t.ClinicClients)
            .HasForeignKey(pt => pt.ClinicId);

        builder.Entity<SubscriptionPlanTable>()
            .HasOne(pt => pt.Company)
            .WithMany(p => p.SubscriptionPlans)
            .HasForeignKey(pt => pt.CompanyId);

        builder.Entity<SubscriptionPlanTable>()
            .HasOne(pt => pt.SubscriptionProduct)
            .WithMany(t => t.SubscriptionPlans)
            .HasForeignKey(pt => pt.SubscriptionProductId);

        builder.Entity<SubscriptionPaymentTable>()
            .HasOne(pt => pt.SubscriptionPlan)
            .WithMany(t => t.SubscriptionPayments)
            .HasForeignKey(pt => pt.SubscriptionPlanId);

        builder.Entity<SubscriptionPaymentTable>()
            .HasOne(pt => pt.Clinic)
            .WithMany(t => t.SubscriptionPayments)
            .HasForeignKey(pt => pt.ClinicId);

        builder.Entity<EmployeeTable>()
            .HasOne(s => s.Company)
            .WithMany(g => g.Employees)
            .HasForeignKey(s => s.CompanyId);

        builder.Entity<VideoTable>()
            .HasOne(s => s.Client)
            .WithMany(g => g.Videos)
            .HasForeignKey(s => s.ClientId);

        builder.Entity<UltrasoundTable>()
            .HasOne(s => s.Client)
            .WithMany(g => g.Ultrasounds)
            .HasForeignKey(s => s.ClientId);

        builder.Entity<SoundTable>()
            .HasOne(s => s.Client)
            .WithMany(g => g.Sounds)
            .HasForeignKey(s => s.ClientId);

        builder.Entity<VideoTable>()
            .HasOne(s => s.Clinic)
            .WithMany(g => g.Videos)
            .HasForeignKey(s => s.ClinicId);

        builder.Entity<UltrasoundTable>()
            .HasOne(s => s.Clinic)
            .WithMany(g => g.Ultrasounds)
            .HasForeignKey(s => s.ClinicId);

        builder.Entity<SoundTable>()
            .HasOne(s => s.Clinic)
            .WithMany(g => g.Sounds)
            .HasForeignKey(s => s.ClinicId);

        builder.Entity<ClinicTable>()
            .HasOne(s => s.Company)
            .WithMany(g => g.Clinics)
            .HasForeignKey(s => s.CompanyId);
    }
}
