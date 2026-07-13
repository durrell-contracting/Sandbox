using DatabaseAccess.Configuration;
using Microsoft.EntityFrameworkCore;

namespace DatabaseAccess.Models;

public partial class RevealContext : DbContext
{
    public RevealContext()
    {
    }

    public RevealContext(DbContextOptions<RevealContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Action> Actions { get; set; }

    public virtual DbSet<AiPrompt> AiPrompts { get; set; }

    public virtual DbSet<Analysis> Analyses { get; set; }

    public virtual DbSet<AnalysisTip> AnalysisTips { get; set; }

    public virtual DbSet<Archetype> Archetypes { get; set; }

    public virtual DbSet<BetaRequest> BetaRequests { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<GameSkill> GameSkills { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<PasswordResetToken> PasswordResetTokens { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    public virtual DbSet<Skill> Skills { get; set; }

    public virtual DbSet<SkillActionMapping> SkillActionMappings { get; set; }

    public virtual DbSet<SkillDetection> SkillDetections { get; set; }

    public virtual DbSet<TipFeedback> TipFeedbacks { get; set; }

    public virtual DbSet<TipLibrary> TipLibraries { get; set; }

    public virtual DbSet<TrainingExample> TrainingExamples { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserArchetype> UserArchetypes { get; set; }

    public virtual DbSet<UserInvite> UserInvites { get; set; }

    public virtual DbSet<Video> Videos { get; set; }

    public virtual DbSet<VideoFrame> VideoFrames { get; set; }

    public virtual DbSet<VideoSegment> VideoSegments { get; set; }

    public virtual DbSet<VideoTag> VideoTags { get; set; }

    /// <summary>
    /// Configures the database connection and model relationships.
    /// </summary>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(new DatabaseConfig().GetConnectionString());
        }

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Action>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("actions_pkey");

            entity.ToTable("actions");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnType("character varying")
                .HasColumnName("id");
            entity.Property(e => e.Category).HasColumnName("category");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.GameId)
                .HasColumnType("character varying")
                .HasColumnName("game_id");
            entity.Property(e => e.Name).HasColumnName("name");

            entity.HasOne(d => d.Game).WithMany(p => p.Actions)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("actions_game_id_games_id_fk");
        });

        modelBuilder.Entity<AiPrompt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ai_prompts_pkey");

            entity.ToTable("ai_prompts");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnType("character varying")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.GameId)
                .HasColumnType("character varying")
                .HasColumnName("game_id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Model).HasColumnName("model");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Priority)
                .HasDefaultValue(0)
                .HasColumnName("priority");
            entity.Property(e => e.SystemPrompt).HasColumnName("system_prompt");
            entity.Property(e => e.TaskType).HasColumnName("task_type");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedById)
                .HasColumnType("character varying")
                .HasColumnName("updated_by_id");
            entity.Property(e => e.UserPromptTemplate).HasColumnName("user_prompt_template");
            entity.Property(e => e.Version)
                .HasDefaultValue(1)
                .HasColumnName("version");

            entity.HasOne(d => d.Game).WithMany(p => p.AiPrompts)
                .HasForeignKey(d => d.GameId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("ai_prompts_game_id_games_id_fk");

            entity.HasOne(d => d.UpdatedBy).WithMany(p => p.AiPrompts)
                .HasForeignKey(d => d.UpdatedById)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("ai_prompts_updated_by_id_users_id_fk");
        });

        modelBuilder.Entity<Analysis>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("analyses_pkey");

            entity.ToTable("analyses");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnType("character varying")
                .HasColumnName("id");
            entity.Property(e => e.AiProvider)
                .HasDefaultValueSql("'claude'::text")
                .HasColumnName("ai_provider");
            entity.Property(e => e.AnalysisMode)
                .HasDefaultValueSql("'standard'::text")
                .HasColumnName("analysis_mode");
            entity.Property(e => e.CompletedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("completed_at");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.DetectedActions).HasColumnName("detected_actions");
            entity.Property(e => e.IsPinned)
                .HasDefaultValue(false)
                .HasColumnName("is_pinned");
            entity.Property(e => e.RawResponse)
                .HasColumnType("jsonb")
                .HasColumnName("raw_response");
            entity.Property(e => e.SegmentId)
                .HasColumnType("character varying")
                .HasColumnName("segment_id");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'pending'::text")
                .HasColumnName("status");
            entity.Property(e => e.Summary).HasColumnName("summary");
            entity.Property(e => e.VideoId)
                .HasColumnType("character varying")
                .HasColumnName("video_id");

            entity.HasOne(d => d.Segment).WithMany(p => p.Analyses)
                .HasForeignKey(d => d.SegmentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("analyses_segment_id_video_segments_id_fk");

            entity.HasOne(d => d.Video).WithMany(p => p.Analyses)
                .HasForeignKey(d => d.VideoId)
                .HasConstraintName("analyses_video_id_videos_id_fk");
        });

        modelBuilder.Entity<AnalysisTip>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("analysis_tips_pkey");

            entity.ToTable("analysis_tips");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnType("character varying")
                .HasColumnName("id");
            entity.Property(e => e.AnalysisId)
                .HasColumnType("character varying")
                .HasColumnName("analysis_id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.LibraryTipId)
                .HasColumnType("character varying")
                .HasColumnName("library_tip_id");
            entity.Property(e => e.ResourceLabel)
                .HasMaxLength(200)
                .HasColumnName("resource_label");
            entity.Property(e => e.ResourceUrl)
                .HasMaxLength(500)
                .HasColumnName("resource_url");
            entity.Property(e => e.SkillName)
                .HasMaxLength(100)
                .HasColumnName("skill_name");
            entity.Property(e => e.SkillScore).HasColumnName("skill_score");
            entity.Property(e => e.Source)
                .HasMaxLength(20)
                .HasColumnName("source");
            entity.Property(e => e.TipType)
                .HasMaxLength(20)
                .HasColumnName("tip_type");
            entity.Property(e => e.UserId)
                .HasColumnType("character varying")
                .HasColumnName("user_id");

            entity.HasOne(d => d.Analysis).WithMany(p => p.AnalysisTips)
                .HasForeignKey(d => d.AnalysisId)
                .HasConstraintName("analysis_tips_analysis_id_analyses_id_fk");

            entity.HasOne(d => d.LibraryTip).WithMany(p => p.AnalysisTips)
                .HasForeignKey(d => d.LibraryTipId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("analysis_tips_library_tip_id_tip_library_id_fk");

            entity.HasOne(d => d.User).WithMany(p => p.AnalysisTips)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("analysis_tips_user_id_users_id_fk");
        });

        modelBuilder.Entity<Archetype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("archetypes_pkey");

            entity.ToTable("archetypes");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnType("character varying")
                .HasColumnName("id");
            entity.Property(e => e.Colour)
                .HasDefaultValueSql("'#283597'::text")
                .HasColumnName("colour");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Icon)
                .HasDefaultValueSql("'shield'::text")
                .HasColumnName("icon");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.SkillWeights)
                .HasColumnType("jsonb")
                .HasColumnName("skill_weights");
            entity.Property(e => e.SortOrder)
                .HasDefaultValue(0)
                .HasColumnName("sort_order");
            entity.Property(e => e.Tagline).HasColumnName("tagline");
        });

        modelBuilder.Entity<BetaRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("beta_requests_pkey");

            entity.ToTable("beta_requests");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnType("character varying")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.ReviewedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("reviewed_at");
            entity.Property(e => e.ReviewedById)
                .HasColumnType("character varying")
                .HasColumnName("reviewed_by_id");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'pending'::text")
                .HasColumnName("status");

            entity.HasOne(d => d.ReviewedBy).WithMany(p => p.BetaRequests)
                .HasForeignKey(d => d.ReviewedById)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("beta_requests_reviewed_by_id_users_id_fk");
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("games_pkey");

            entity.ToTable("games");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnType("character varying")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Genre).HasColumnName("genre");
            entity.Property(e => e.IconUrl).HasColumnName("icon_url");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<GameSkill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("game_skills_pkey");

            entity.ToTable("game_skills");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnType("character varying")
                .HasColumnName("id");
            entity.Property(e => e.GameId)
                .HasColumnType("character varying")
                .HasColumnName("game_id");
            entity.Property(e => e.RelevanceScore)
                .HasDefaultValueSql("1")
                .HasColumnName("relevance_score");
            entity.Property(e => e.SkillId)
                .HasColumnType("character varying")
                .HasColumnName("skill_id");

            entity.HasOne(d => d.Game).WithMany(p => p.GameSkills)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("game_skills_game_id_games_id_fk");

            entity.HasOne(d => d.Skill).WithMany(p => p.GameSkills)
                .HasForeignKey(d => d.SkillId)
                .HasConstraintName("game_skills_skill_id_skills_id_fk");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("notifications_pkey");

            entity.ToTable("notifications");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnType("character varying")
                .HasColumnName("id");
            entity.Property(e => e.Body).HasColumnName("body");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Read)
                .HasDefaultValue(false)
                .HasColumnName("read");
            entity.Property(e => e.ReferenceId)
                .HasColumnType("character varying")
                .HasColumnName("reference_id");
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .HasColumnName("title");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");
            entity.Property(e => e.UserId)
                .HasColumnType("character varying")
                .HasColumnName("user_id");
            entity.Property(e => e.VideoId)
                .HasColumnType("character varying")
                .HasColumnName("video_id");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("notifications_user_id_users_id_fk");
        });

        modelBuilder.Entity<PasswordResetToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("password_reset_tokens_pkey");

            entity.ToTable("password_reset_tokens");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnType("character varying")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.ExpiresAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("expires_at");
            entity.Property(e => e.TokenHash).HasColumnName("token_hash");
            entity.Property(e => e.UsedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("used_at");
            entity.Property(e => e.UserId)
                .HasColumnType("character varying")
                .HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.PasswordResetTokens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("password_reset_tokens_user_id_users_id_fk");
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.Sid).HasName("session_pkey");

            entity.ToTable("session");

            entity.Property(e => e.Sid)
                .HasColumnType("character varying")
                .HasColumnName("sid");
            entity.Property(e => e.Expire)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("expire");
            entity.Property(e => e.Sess)
                .HasColumnType("jsonb")
                .HasColumnName("sess");
        });

        modelBuilder.Entity<Skill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("skills_pkey");

            entity.ToTable("skills");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnType("character varying")
                .HasColumnName("id");
            entity.Property(e => e.Category).HasColumnName("category");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Type).HasColumnName("type");
        });

        modelBuilder.Entity<SkillActionMapping>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("skill_action_mappings_pkey");

            entity.ToTable("skill_action_mappings");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnType("character varying")
                .HasColumnName("id");
            entity.Property(e => e.ActionId)
                .HasColumnType("character varying")
                .HasColumnName("action_id");
            entity.Property(e => e.SkillId)
                .HasColumnType("character varying")
                .HasColumnName("skill_id");
            entity.Property(e => e.Weight)
                .HasDefaultValueSql("1")
                .HasColumnName("weight");

            entity.HasOne(d => d.Action).WithMany(p => p.SkillActionMappings)
                .HasForeignKey(d => d.ActionId)
                .HasConstraintName("skill_action_mappings_action_id_actions_id_fk");

            entity.HasOne(d => d.Skill).WithMany(p => p.SkillActionMappings)
                .HasForeignKey(d => d.SkillId)
                .HasConstraintName("skill_action_mappings_skill_id_skills_id_fk");
        });

        modelBuilder.Entity<SkillDetection>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("skill_detections_pkey");

            entity.ToTable("skill_detections");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnType("character varying")
                .HasColumnName("id");
            entity.Property(e => e.AnalysisId)
                .HasColumnType("character varying")
                .HasColumnName("analysis_id");
            entity.Property(e => e.ConfidenceScore).HasColumnName("confidence_score");
            entity.Property(e => e.Evidence).HasColumnName("evidence");
            entity.Property(e => e.FrameId)
                .HasColumnType("character varying")
                .HasColumnName("frame_id");
            entity.Property(e => e.HumanConfidence).HasColumnName("human_confidence");
            entity.Property(e => e.HumanVerified)
                .HasDefaultValue(false)
                .HasColumnName("human_verified");
            entity.Property(e => e.IsCandidate)
                .HasDefaultValue(false)
                .HasColumnName("is_candidate");
            entity.Property(e => e.IsCorrect).HasColumnName("is_correct");
            entity.Property(e => e.ReviewedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("reviewed_at");
            entity.Property(e => e.ReviewerNotes).HasColumnName("reviewer_notes");
            entity.Property(e => e.SkillId)
                .HasColumnType("character varying")
                .HasColumnName("skill_id");
            entity.Property(e => e.Source)
                .HasDefaultValueSql("'ai'::text")
                .HasColumnName("source");
            entity.Property(e => e.TagId)
                .HasColumnType("character varying")
                .HasColumnName("tag_id");
            entity.Property(e => e.Timestamp).HasColumnName("timestamp");

            entity.HasOne(d => d.Analysis).WithMany(p => p.SkillDetections)
                .HasForeignKey(d => d.AnalysisId)
                .HasConstraintName("skill_detections_analysis_id_analyses_id_fk");

            entity.HasOne(d => d.Frame).WithMany(p => p.SkillDetections)
                .HasForeignKey(d => d.FrameId)
                .HasConstraintName("skill_detections_frame_id_video_frames_id_fk");

            entity.HasOne(d => d.Skill).WithMany(p => p.SkillDetections)
                .HasForeignKey(d => d.SkillId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("skill_detections_skill_id_skills_id_fk");

            entity.HasOne(d => d.Tag).WithMany(p => p.SkillDetections)
                .HasForeignKey(d => d.TagId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("skill_detections_tag_id_video_tags_id_fk");
        });

        modelBuilder.Entity<TipFeedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tip_feedback_pkey");

            entity.ToTable("tip_feedback");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnType("character varying")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Rating)
                .HasMaxLength(10)
                .HasColumnName("rating");
            entity.Property(e => e.TipId)
                .HasColumnType("character varying")
                .HasColumnName("tip_id");
            entity.Property(e => e.UserId)
                .HasColumnType("character varying")
                .HasColumnName("user_id");

            entity.HasOne(d => d.Tip).WithMany(p => p.TipFeedbacks)
                .HasForeignKey(d => d.TipId)
                .HasConstraintName("tip_feedback_tip_id_analysis_tips_id_fk");

            entity.HasOne(d => d.User).WithMany(p => p.TipFeedbacks)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("tip_feedback_user_id_users_id_fk");
        });

        modelBuilder.Entity<TipLibrary>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tip_library_pkey");

            entity.ToTable("tip_library");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnType("character varying")
                .HasColumnName("id");
            entity.Property(e => e.Active)
                .HasDefaultValue(true)
                .HasColumnName("active");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.ResourceLabel)
                .HasMaxLength(200)
                .HasColumnName("resource_label");
            entity.Property(e => e.ResourceUrl)
                .HasMaxLength(500)
                .HasColumnName("resource_url");
            entity.Property(e => e.ScoreMax).HasColumnName("score_max");
            entity.Property(e => e.ScoreMin).HasColumnName("score_min");
            entity.Property(e => e.SkillName)
                .HasMaxLength(100)
                .HasColumnName("skill_name");
            entity.Property(e => e.Track)
                .HasMaxLength(20)
                .HasColumnName("track");
        });

        modelBuilder.Entity<TrainingExample>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("training_examples_pkey");

            entity.ToTable("training_examples");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnType("character varying")
                .HasColumnName("id");
            entity.Property(e => e.ActionId)
                .HasColumnType("character varying")
                .HasColumnName("action_id");
            entity.Property(e => e.ContextNotes).HasColumnName("context_notes");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.FrameImageUrl).HasColumnName("frame_image_url");
            entity.Property(e => e.GameId)
                .HasColumnType("character varying")
                .HasColumnName("game_id");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.QualityScore)
                .HasDefaultValueSql("1")
                .HasColumnName("quality_score");
            entity.Property(e => e.SkillId)
                .HasColumnType("character varying")
                .HasColumnName("skill_id");
            entity.Property(e => e.SourceTagId)
                .HasColumnType("character varying")
                .HasColumnName("source_tag_id");
            entity.Property(e => e.SuccessRate).HasColumnName("success_rate");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UseCount)
                .HasDefaultValue(0)
                .HasColumnName("use_count");

            entity.HasOne(d => d.Action).WithMany(p => p.TrainingExamples)
                .HasForeignKey(d => d.ActionId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("training_examples_action_id_actions_id_fk");

            entity.HasOne(d => d.Game).WithMany(p => p.TrainingExamples)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("training_examples_game_id_games_id_fk");

            entity.HasOne(d => d.Skill).WithMany(p => p.TrainingExamples)
                .HasForeignKey(d => d.SkillId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("training_examples_skill_id_skills_id_fk");

            entity.HasOne(d => d.SourceTag).WithMany(p => p.TrainingExamples)
                .HasForeignKey(d => d.SourceTagId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("training_examples_source_tag_id_video_tags_id_fk");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_unique").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnType("character varying")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.ForcePasswordChange)
                .HasDefaultValue(true)
                .HasColumnName("force_password_change");
            entity.Property(e => e.LastLoginAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("last_login_at");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
            entity.Property(e => e.Role)
                .HasDefaultValueSql("'user'::text")
                .HasColumnName("role");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'active'::text")
                .HasColumnName("status");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<UserArchetype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_archetypes_pkey");

            entity.ToTable("user_archetypes");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnType("character varying")
                .HasColumnName("id");
            entity.Property(e => e.ArchetypeId)
                .HasColumnType("character varying")
                .HasColumnName("archetype_id");
            entity.Property(e => e.ComputedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("computed_at");
            entity.Property(e => e.Score).HasColumnName("score");
            entity.Property(e => e.TopSkills)
                .HasColumnType("jsonb")
                .HasColumnName("top_skills");
            entity.Property(e => e.UserId)
                .HasColumnType("character varying")
                .HasColumnName("user_id");
            entity.Property(e => e.VideoCount)
                .HasDefaultValue(0)
                .HasColumnName("video_count");

            entity.HasOne(d => d.Archetype).WithMany(p => p.UserArchetypes)
                .HasForeignKey(d => d.ArchetypeId)
                .HasConstraintName("user_archetypes_archetype_id_archetypes_id_fk");

            entity.HasOne(d => d.User).WithMany(p => p.UserArchetypes)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("user_archetypes_user_id_users_id_fk");
        });

        modelBuilder.Entity<UserInvite>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_invites_pkey");

            entity.ToTable("user_invites");

            entity.HasIndex(e => e.Code, "user_invites_code_unique").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnType("character varying")
                .HasColumnName("id");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedById)
                .HasColumnType("character varying")
                .HasColumnName("created_by_id");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.ExpiresAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("expires_at");
            entity.Property(e => e.MaxUses)
                .HasDefaultValue(1)
                .HasColumnName("max_uses");
            entity.Property(e => e.Role)
                .HasDefaultValueSql("'user'::text")
                .HasColumnName("role");
            entity.Property(e => e.UsedCount)
                .HasDefaultValue(0)
                .HasColumnName("used_count");

            entity.HasOne(d => d.CreatedBy).WithMany(p => p.UserInvites)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("user_invites_created_by_id_users_id_fk");
        });

        modelBuilder.Entity<Video>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("videos_pkey");

            entity.ToTable("videos");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnType("character varying")
                .HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.FileName).HasColumnName("file_name");
            entity.Property(e => e.FileUrl).HasColumnName("file_url");
            entity.Property(e => e.GameId)
                .HasColumnType("character varying")
                .HasColumnName("game_id");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'pending'::text")
                .HasColumnName("status");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.UploadedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("uploaded_at");
            entity.Property(e => e.UserId)
                .HasColumnType("character varying")
                .HasColumnName("user_id");

            entity.HasOne(d => d.Game).WithMany(p => p.Videos)
                .HasForeignKey(d => d.GameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("videos_game_id_games_id_fk");

            entity.HasOne(d => d.User).WithMany(p => p.Videos)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("videos_user_id_users_id_fk");
        });

        modelBuilder.Entity<VideoFrame>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("video_frames_pkey");

            entity.ToTable("video_frames");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnType("character varying")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.FrameNumber).HasColumnName("frame_number");
            entity.Property(e => e.ImageUrl).HasColumnName("image_url");
            entity.Property(e => e.SegmentId)
                .HasColumnType("character varying")
                .HasColumnName("segment_id");
            entity.Property(e => e.Timestamp).HasColumnName("timestamp");
            entity.Property(e => e.VideoId)
                .HasColumnType("character varying")
                .HasColumnName("video_id");

            entity.HasOne(d => d.Segment).WithMany(p => p.VideoFrames)
                .HasForeignKey(d => d.SegmentId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("video_frames_segment_id_video_segments_id_fk");

            entity.HasOne(d => d.Video).WithMany(p => p.VideoFrames)
                .HasForeignKey(d => d.VideoId)
                .HasConstraintName("video_frames_video_id_videos_id_fk");
        });

        modelBuilder.Entity<VideoSegment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("video_segments_pkey");

            entity.ToTable("video_segments");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnType("character varying")
                .HasColumnName("id");
            entity.Property(e => e.AiSummary).HasColumnName("ai_summary");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.EndTime).HasColumnName("end_time");
            entity.Property(e => e.InterestScore)
                .HasDefaultValueSql("0.5")
                .HasColumnName("interest_score");
            entity.Property(e => e.Label).HasColumnName("label");
            entity.Property(e => e.Metadata)
                .HasColumnType("jsonb")
                .HasColumnName("metadata");
            entity.Property(e => e.StartTime).HasColumnName("start_time");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'detected'::text")
                .HasColumnName("status");
            entity.Property(e => e.ThumbnailUrl).HasColumnName("thumbnail_url");
            entity.Property(e => e.VideoId)
                .HasColumnType("character varying")
                .HasColumnName("video_id");

            entity.HasOne(d => d.Video).WithMany(p => p.VideoSegments)
                .HasForeignKey(d => d.VideoId)
                .HasConstraintName("video_segments_video_id_videos_id_fk");
        });

        modelBuilder.Entity<VideoTag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("video_tags_pkey");

            entity.ToTable("video_tags");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnType("character varying")
                .HasColumnName("id");
            entity.Property(e => e.ActionId)
                .HasColumnType("character varying")
                .HasColumnName("action_id");
            entity.Property(e => e.Confidence)
                .HasDefaultValueSql("1")
                .HasColumnName("confidence");
            entity.Property(e => e.ConsensusCount)
                .HasDefaultValue(1)
                .HasColumnName("consensus_count");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Difficulty).HasColumnName("difficulty");
            entity.Property(e => e.EndTimestamp).HasColumnName("end_timestamp");
            entity.Property(e => e.FrameId)
                .HasColumnType("character varying")
                .HasColumnName("frame_id");
            entity.Property(e => e.FrameImageUrl).HasColumnName("frame_image_url");
            entity.Property(e => e.IsExemplar)
                .HasDefaultValue(false)
                .HasColumnName("is_exemplar");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.QualityScore).HasColumnName("quality_score");
            entity.Property(e => e.SegmentId)
                .HasColumnType("character varying")
                .HasColumnName("segment_id");
            entity.Property(e => e.SkillId)
                .HasColumnType("character varying")
                .HasColumnName("skill_id");
            entity.Property(e => e.Source)
                .HasDefaultValueSql("'human'::text")
                .HasColumnName("source");
            entity.Property(e => e.Timestamp).HasColumnName("timestamp");
            entity.Property(e => e.UseForLearning)
                .HasDefaultValue(false)
                .HasColumnName("use_for_learning");
            entity.Property(e => e.VideoId)
                .HasColumnType("character varying")
                .HasColumnName("video_id");

            entity.HasOne(d => d.Action).WithMany(p => p.VideoTags)
                .HasForeignKey(d => d.ActionId)
                .HasConstraintName("video_tags_action_id_actions_id_fk");

            entity.HasOne(d => d.Frame).WithMany(p => p.VideoTags)
                .HasForeignKey(d => d.FrameId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("video_tags_frame_id_video_frames_id_fk");

            entity.HasOne(d => d.Segment).WithMany(p => p.VideoTags)
                .HasForeignKey(d => d.SegmentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("video_tags_segment_id_video_segments_id_fk");

            entity.HasOne(d => d.Skill).WithMany(p => p.VideoTags)
                .HasForeignKey(d => d.SkillId)
                .HasConstraintName("video_tags_skill_id_skills_id_fk");

            entity.HasOne(d => d.Video).WithMany(p => p.VideoTags)
                .HasForeignKey(d => d.VideoId)
                .HasConstraintName("video_tags_video_id_videos_id_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
