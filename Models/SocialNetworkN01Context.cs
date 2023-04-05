using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Web_Social_network_BE.Models;

public partial class SocialNetworkN01Context : DbContext
{
    public SocialNetworkN01Context()
    {
    }

    public SocialNetworkN01Context(DbContextOptions<SocialNetworkN01Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Like> Likes { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Relation> Relations { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UsersInfo> UsersInfos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-89QSK5E;Initial Catalog=Social_network_N01;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__comments__E79576871C924027");

            entity.ToTable("comments");

            entity.Property(e => e.CommentId).HasColumnName("comment_id");
            entity.Property(e => e.CommentAt)
                .HasColumnType("datetime")
                .HasColumnName("comment_at");
            entity.Property(e => e.Content)
                .HasMaxLength(5000)
                .IsUnicode(false)
                .HasColumnName("content");
            entity.Property(e => e.LikeCount)
                .HasDefaultValueSql("((0))")
                .HasColumnName("like_count");
            entity.Property(e => e.PostId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("post_id");
            entity.Property(e => e.UserId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("user_id");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__images__DC9AC955FE57E930");

            entity.ToTable("images");

            entity.Property(e => e.ImageId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("image_id");
            entity.Property(e => e.PostId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("post_id");
            entity.Property(e => e.Type)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("type");
            entity.Property(e => e.Url)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("url");
        });

        modelBuilder.Entity<Like>(entity =>
        {
            entity.HasKey(e => e.LikeId).HasName("PK__likes__992C7930C2FCACF6");

            entity.ToTable("likes");

            entity.Property(e => e.LikeId).HasColumnName("like_id");
            entity.Property(e => e.CommentId).HasColumnName("comment_id");
            entity.Property(e => e.LikeAt)
                .HasColumnType("datetime")
                .HasColumnName("like_at");
            entity.Property(e => e.PostId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("post_id");
            entity.Property(e => e.UserId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("user_id");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PK__posts__3ED7876656D6019B");

            entity.ToTable("posts");

            entity.Property(e => e.PostId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("post_id");
            entity.Property(e => e.AccessModifier)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("access_modifier");
            entity.Property(e => e.Content)
                .HasMaxLength(5000)
                .IsUnicode(false)
                .HasColumnName("content");
            entity.Property(e => e.CreateAt)
                .HasColumnType("datetime")
                .HasColumnName("create_at");
            entity.Property(e => e.LikeCount)
                .HasDefaultValueSql("((0))")
                .HasColumnName("like_count");
            entity.Property(e => e.CommnetCount)
                .HasDefaultValueSql("((0))")
                .HasColumnName("commnet_count");
            entity.Property(e => e.PostType)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("post_type");
            entity.Property(e => e.UserId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("user_id");
        });

        modelBuilder.Entity<Relation>(entity =>
        {
            entity.HasKey(e => e.RelationId).HasName("PK__relation__C409F3239379D2BC");

            entity.ToTable("relations");

            entity.Property(e => e.RelationId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("relation_id");
            entity.Property(e => e.ChangeAt)
                .HasColumnType("datetime")
                .HasColumnName("change_at");
            entity.Property(e => e.TypeRelation)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("type_relation");
            entity.Property(e => e.UserId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("user_id");
            entity.Property(e => e.UserTargetIduserId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("user_target_iduser_id");
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.RegisterId).HasName("PK__requests__1418262F5D5CCE9E");

            entity.ToTable("requests");

            entity.Property(e => e.RegisterId).HasColumnName("register_id");
            entity.Property(e => e.CodeType)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("code_type");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.RegisterAt)
                .HasColumnType("datetime")
                .HasColumnName("register_at");
            entity.Property(e => e.RequestCode).HasColumnName("request_code");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__users__B9BE370F2F3AD39B");

            entity.ToTable("users");

            entity.Property(e => e.UserId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("user_id");
            entity.Property(e => e.Avatar)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("avatar");
            entity.Property(e => e.FullName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("full_name");
            entity.Property(e => e.UserInfoId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("user_info_id");
        });

        modelBuilder.Entity<UsersInfo>(entity =>
        {
            entity.HasKey(e => e.UserInfoId).HasName("PK__users_in__82ABEB54C3872EED");

            entity.ToTable("users_info");

            entity.Property(e => e.UserInfoId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("user_info_id");
            entity.Property(e => e.AboutMe)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("about_me");
            entity.Property(e => e.Address)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.CoverImage)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("cover_image");
            entity.Property(e => e.Dob)
                .HasColumnType("date")
                .HasColumnName("dob");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Gender)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("gender");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.UserRole)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("user_role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
