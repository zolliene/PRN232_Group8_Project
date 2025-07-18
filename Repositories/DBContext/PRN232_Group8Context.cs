﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;


namespace Repositories.Models;

public partial class PRN232_Group8Context : DbContext
{
    public PRN232_Group8Context(){}
    public PRN232_Group8Context(DbContextOptions<PRN232_Group8Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<ArvDrug> ArvDrugs { get; set; }

    public virtual DbSet<ArvDrugGroup> ArvDrugGroups { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<Examination> Examinations { get; set; }

    public virtual DbSet<LabTest> LabTests { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<PatientRegimen> PatientRegimens { get; set; }

    public virtual DbSet<RegimenDrug> RegimenDrugs { get; set; }

    public virtual DbSet<TestType> TestTypes { get; set; }

    public virtual DbSet<TreatmentRegimensMaster> TreatmentRegimensMasters { get; set; }

    public virtual DbSet<User> Users { get; set; }
    public static string GetConnectionString(string connectionStringName)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        string connectionString = config.GetConnectionString(connectionStringName);
        return connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(GetConnectionString("DefaultConnection")).UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__appointm__3213E83FD7944B3A");

            entity.ToTable("appointments");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CheckInTime)
                .HasColumnType("datetime")
                .HasColumnName("check_in_time");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.DoctorId).HasColumnName("doctor_id");
            entity.Property(e => e.Note)
                .HasMaxLength(500)
                .HasColumnName("note");
            entity.Property(e => e.PatientId).HasColumnName("patient_id");
            entity.Property(e => e.Session)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("session");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("status");

            entity.HasOne(d => d.Doctor).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.DoctorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__appointme__docto__5AEE82B9");

            entity.HasOne(d => d.Patient).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.PatientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__appointme__patie__59FA5E80");
        });

        modelBuilder.Entity<ArvDrug>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__arv_drug__3213E83FAA4D5F20");

            entity.ToTable("arv_drugs");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ActiveIngredient)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("active_ingredient");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.GroupId).HasColumnName("group_id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");

            entity.HasOne(d => d.Group).WithMany(p => p.ArvDrugs)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("FK__arv_drugs__group__778AC167");
        });

        modelBuilder.Entity<ArvDrugGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__arv_drug__3213E83F79C68522");

            entity.ToTable("arv_drug_groups");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__doctors__3213E83F3380C413");

            entity.ToTable("doctors");

            entity.HasIndex(e => e.UserId, "UQ__doctors__B9BE370E2D781468").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateOfLicense).HasColumnName("date_of_license");
            entity.Property(e => e.LicenseExpiryDate).HasColumnName("license_expiry_date");
            entity.Property(e => e.LicenseNumber)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("license_number");
            entity.Property(e => e.Qualification)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("qualification");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithOne(p => p.Doctor)
                .HasForeignKey<Doctor>(d => d.UserId)
                .HasConstraintName("FK__doctors__user_id__571DF1D5");
        });

        modelBuilder.Entity<Examination>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__examinat__3213E83FCC280EA4");

            entity.ToTable("examinations");

            entity.HasIndex(e => e.AppointmentId, "UQ__examinat__A50828FD52164B18").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
            entity.Property(e => e.ClinicalData).HasColumnName("clinical_data");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.DoctorId).HasColumnName("doctor_id");

            entity.HasOne(d => d.Appointment).WithOne(p => p.Examination)
                .HasForeignKey<Examination>(d => d.AppointmentId)
                .HasConstraintName("FK__examinati__appoi__619B8048");

            entity.HasOne(d => d.Doctor).WithMany(p => p.Examinations)
                .HasForeignKey(d => d.DoctorId)
                .HasConstraintName("FK__examinati__docto__628FA481");
        });

        modelBuilder.Entity<LabTest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__lab_test__3213E83F8BCBB761");

            entity.ToTable("lab_tests");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AppointmentId).HasColumnName("appointment_id");
            entity.Property(e => e.Comments)
                .HasMaxLength(500)
                .HasColumnName("comments");
            entity.Property(e => e.LabStaffId).HasColumnName("lab_staff_id");
            entity.Property(e => e.OrderStatus)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("pending")
                .HasColumnName("order_status");
            entity.Property(e => e.OrderTime)
                .HasColumnType("datetime")
                .HasColumnName("order_time");
            entity.Property(e => e.ReferenceRange)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("reference_range");
            entity.Property(e => e.ResultDate)
                .HasColumnType("datetime")
                .HasColumnName("result_date");
            entity.Property(e => e.ResultStatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("result_status");
            entity.Property(e => e.ResultValue)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("result_value");
            entity.Property(e => e.TestTypeId).HasColumnName("test_type_id");
            entity.Property(e => e.Unit)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("unit");

            entity.HasOne(d => d.Appointment).WithMany(p => p.LabTests)
                .HasForeignKey(d => d.AppointmentId)
                .HasConstraintName("FK__lab_tests__appoi__68487DD7");

            entity.HasOne(d => d.LabStaff).WithMany(p => p.LabTests)
                .HasForeignKey(d => d.LabStaffId)
                .HasConstraintName("FK__lab_tests__lab_s__6B24EA82");

            entity.HasOne(d => d.TestType).WithMany(p => p.LabTests)
                .HasForeignKey(d => d.TestTypeId)
                .HasConstraintName("FK__lab_tests__test___693CA210");
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__patients__3213E83F45030097");

            entity.ToTable("patients");

            entity.HasIndex(e => e.UserId, "UQ__patients__B9BE370E0332B924").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("address");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Dob).HasColumnName("dob");
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("first_name");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("gender");
            entity.Property(e => e.InsuranceNumber)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("insurance_number");
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("last_name");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithOne(p => p.Patient)
                .HasForeignKey<Patient>(d => d.UserId)
                .HasConstraintName("FK__patients__user_i__5165187F");
        });

        modelBuilder.Entity<PatientRegimen>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__patient___3213E83FDF2DF5C4");

            entity.ToTable("patient_regimens");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CustomNotes)
                .HasMaxLength(500)
                .HasColumnName("custom_notes");
            entity.Property(e => e.ExaminationId).HasColumnName("examination_id");
            entity.Property(e => e.RegimenMasterId).HasColumnName("regimen_master_id");

            entity.HasOne(d => d.Examination).WithMany(p => p.PatientRegimen)
                .HasForeignKey(d => d.ExaminationId)
                .HasConstraintName("FK__patient_r__exami__71D1E811");

            entity.HasOne(d => d.RegimenMaster).WithMany(p => p.PatientRegimen)
                .HasForeignKey(d => d.RegimenMasterId)
                .HasConstraintName("FK__patient_r__regim__72C60C4A");
        });

        modelBuilder.Entity<RegimenDrug>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__regimen___3213E83F2336CF1F");

            entity.ToTable("regimen_drugs");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Dose)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("dose");
            entity.Property(e => e.DrugId).HasColumnName("drug_id");
            entity.Property(e => e.Frequency)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("frequency");
            entity.Property(e => e.RegimenId).HasColumnName("regimen_id");

            entity.HasOne(d => d.Drug).WithMany(p => p.RegimenDrugs)
                .HasForeignKey(d => d.DrugId)
                .HasConstraintName("FK__regimen_d__drug___7B5B524B");

            entity.HasOne(d => d.Regimen).WithMany(p => p.RegimenDrugs)
                .HasForeignKey(d => d.RegimenId)
                .HasConstraintName("FK__regimen_d__regim__7A672E12");
        });

        modelBuilder.Entity<TestType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__test_typ__3213E83FC450186D");

            entity.ToTable("test_types");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.ReferenceRange)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("reference_range");
            entity.Property(e => e.Unit)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("unit");
        });

        modelBuilder.Entity<TreatmentRegimensMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__treatmen__3213E83FF1AF39E0");

            entity.ToTable("treatment_regimens_master");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Drugs)
                .HasColumnType("text")
                .HasColumnName("drugs");
            entity.Property(e => e.RegimenName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("regimen_name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83F231B50DB");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "UQ__users__AB6E6164B2C19410").IsUnique();

            entity.HasIndex(e => e.Username, "UQ__users__F3DBC5722371CF7B").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Role)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("role");
            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("full_name");

        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}