using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Web_Social_network_BE.Models;

public partial class SocialNetworkN01Ver2Context : DbContext
{
    public SocialNetworkN01Ver2Context()
    {
    }

    public SocialNetworkN01Ver2Context(DbContextOptions<SocialNetworkN01Ver2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Conversation> Conversations { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Like> Likes { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Relation> Relations { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UsersInfo> UsersInfos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-89QSK5E;Initial Catalog=Social_network_N01_ver2;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__comments__E7957687ED7A3471");

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

        modelBuilder.Entity<Conversation>(entity =>
        {
            entity.HasKey(e => e.ConversationId).HasName("PK__conversa__311E7E9A664D11B8");

            entity.ToTable("conversations");

            entity.Property(e => e.ConversationId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("conversation_id");
            entity.Property(e => e.UserId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("user_id");
            entity.Property(e => e.UserTargetId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("user_target_id");

           
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PK__images__DC9AC955EEDEA951");

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
            entity.HasKey(e => e.LikeId).HasName("PK__likes__992C793073892314");

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

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PK__messages__0BBF6EE607218DF1");

            entity.ToTable("messages");

            entity.Property(e => e.MessageId).HasColumnName("message_id");
            entity.Property(e => e.Content)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("content");
            entity.Property(e => e.ConversationId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("conversation_id");
            entity.Property(e => e.SendAt)
                .HasColumnType("datetime")
                .HasColumnName("send_at");
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("type");

        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__notifica__E059842F221B65AB");

            entity.ToTable("notifications");

            entity.Property(e => e.NotificationId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("notification_id");
            entity.Property(e => e.Content)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("content");
            entity.Property(e => e.NotificationsAt)
                .HasColumnType("datetime")
                .HasColumnName("notifications_at");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.UserId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("user_id");
            entity.Property(e => e.UserTargetId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("user_target_id");

            
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PK__posts__3ED787662A269CD5");

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
            entity.HasKey(e => e.RelationId).HasName("PK__relation__C409F323A7DC3B3C");

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

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(r => new { r.UserId, r.PostId });

            entity.ToTable("reports");

            entity.Property(e => e.PostId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("post_id");
            entity.Property(e => e.ReportContent)
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("report_content");
            entity.Property(e => e.UserId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("user_id");

           
        });



        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.RegisterId).HasName("PK__requests__1418262FB9A5EE40");

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
            entity.HasKey(e => e.UserId).HasName("PK__users__B9BE370F4C86F916");

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
            entity.HasKey(e => e.UserInfoId).HasName("PK__users_in__82ABEB542257A841");

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
            entity.Property(e => e.RegisterAt)
                .HasColumnType("datetime")
                .HasColumnName("register_at");
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
