using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Data;

public partial class DatabaseContext : DbContext
{
    public string dbHost { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
        this.dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
    }

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql(@"Host=" + dbHost + ":5432;Username=postgres;Password=postgres;Database=postgres");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_chatid");

            entity.ToTable("chats");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.Name)
                .HasMaxLength(64)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_messageid");

            entity.ToTable("messages");

            entity.HasIndex(e => e.Chat, "fk_chatmessages");

            entity.HasIndex(e => e.Author, "fk_usermessages");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Author)
                .HasMaxLength(16)
                .HasColumnName("author");
            entity.Property(e => e.Chat).HasColumnName("chat");
            entity.Property(e => e.Content)
                .HasMaxLength(2048)
                .HasColumnName("content");
            entity.Property(e => e.Creationdate)
                .HasMaxLength(64)
                .HasColumnName("creationdate");
            entity.Property(e => e.Type)
                .HasMaxLength(8)
                .HasColumnName("type");

            entity.HasOne(d => d.AuthorNavigation).WithMany(p => p.Messages)
                .HasForeignKey(d => d.Author)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_usermessages");

            entity.HasOne(d => d.ChatNavigation).WithMany(p => p.Messages)
                .HasForeignKey(d => d.Chat)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_chatmessages");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Name).HasName("pk_user");

            entity.ToTable("users");

            entity.Property(e => e.Name)
                .HasMaxLength(16)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(64)
                .HasColumnName("password");
            entity.Property(e => e.Role)
                .HasMaxLength(8)
                .HasColumnName("role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
