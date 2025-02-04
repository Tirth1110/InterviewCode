USE [InterviewDB]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 2024-05-08 02:20:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Students]    Script Date: 2024-05-08 02:20:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Students](
	[StudentId] [uniqueidentifier] NOT NULL,
	[Name] [varchar](20) NOT NULL,
	[Email] [varchar](30) NOT NULL,
	[ContactNo] [varchar](15) NOT NULL,
	[CollegeName] [varchar](30) NOT NULL,
	[EnrolmentNumber] [varchar](30) NOT NULL,
	[ProfileImage] [nvarchar](max) NULL,
 CONSTRAINT [PK_Students] PRIMARY KEY CLUSTERED 
(
	[StudentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StudentSubjects]    Script Date: 2024-05-08 02:20:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StudentSubjects](
	[StudentSubjectsId] [uniqueidentifier] NOT NULL,
	[StudentId] [uniqueidentifier] NOT NULL,
	[SubjectName] [varchar](20) NOT NULL,
	[Marks] [int] NOT NULL,
 CONSTRAINT [PK_StudentSubjects] PRIMARY KEY CLUSTERED 
(
	[StudentSubjectsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240507142435_init', N'8.0.4')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240507151208_AddProfileImageColumn', N'8.0.4')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240507155523_MakeNullableProfileImageColumn', N'8.0.4')
GO
INSERT [dbo].[Students] ([StudentId], [Name], [Email], [ContactNo], [CollegeName], [EnrolmentNumber], [ProfileImage]) VALUES (N'4a4b1ee9-fa8f-49a4-66e8-08dc6eb4cc0a', N'wsw', N'jay@gmail.com', N'7984255014', N'wsw', N'wsw13', N'Images\StudentImages\20db8fa8-216b-4626-8268-d39af741a7c5.jpg')
INSERT [dbo].[Students] ([StudentId], [Name], [Email], [ContactNo], [CollegeName], [EnrolmentNumber], [ProfileImage]) VALUES (N'1b3ab371-38ee-4ad0-7260-08dc6eb57106', N'de', N'jayesh@gmail.com', N'1561561560', N'WSSW', N'wdwsw', N'Images\StudentImages\82840942-1045-4b1e-b428-0a36eafae38e.jpg')
INSERT [dbo].[Students] ([StudentId], [Name], [Email], [ContactNo], [CollegeName], [EnrolmentNumber], [ProfileImage]) VALUES (N'5107c1fb-8508-4cd3-715d-08dc6ec54a99', N'S', N'wsw@gmail.com', N'6515565653', N'SHIWd', N'mmlw', N'Images\StudentImages\9ec3a8b8-9596-44e1-92d6-e713411e4cd0.jpg')
INSERT [dbo].[Students] ([StudentId], [Name], [Email], [ContactNo], [CollegeName], [EnrolmentNumber], [ProfileImage]) VALUES (N'3fa85f64-5717-4562-b3fc-2c963f66afa6', N'Jigensh', N'tempwsww@gmail.com', N'1985626800', N'BCAollaege', N'JIG132', N'Images\StudentImages\33f57fec-17fa-4bc0-be3f-5e1ddab03331.jpg')
GO
INSERT [dbo].[StudentSubjects] ([StudentSubjectsId], [StudentId], [SubjectName], [Marks]) VALUES (N'4e998a1f-69d6-44ad-2beb-08dc6ecb6d73', N'1b3ab371-38ee-4ad0-7260-08dc6eb57106', N'Test', 10)
INSERT [dbo].[StudentSubjects] ([StudentSubjectsId], [StudentId], [SubjectName], [Marks]) VALUES (N'db89318a-c398-481f-2bec-08dc6ecb6d73', N'1b3ab371-38ee-4ad0-7260-08dc6eb57106', N'Test', 20)
INSERT [dbo].[StudentSubjects] ([StudentSubjectsId], [StudentId], [SubjectName], [Marks]) VALUES (N'7c2f5624-4058-480f-2bed-08dc6ecb6d73', N'1b3ab371-38ee-4ad0-7260-08dc6eb57106', N'Test', 30)
INSERT [dbo].[StudentSubjects] ([StudentSubjectsId], [StudentId], [SubjectName], [Marks]) VALUES (N'0294df1f-1f4f-4107-2bee-08dc6ecb6d73', N'1b3ab371-38ee-4ad0-7260-08dc6eb57106', N'Test', 40)
INSERT [dbo].[StudentSubjects] ([StudentSubjectsId], [StudentId], [SubjectName], [Marks]) VALUES (N'19adba7e-e3d5-43db-2bef-08dc6ecb6d73', N'1b3ab371-38ee-4ad0-7260-08dc6eb57106', N'Test', 50)
INSERT [dbo].[StudentSubjects] ([StudentSubjectsId], [StudentId], [SubjectName], [Marks]) VALUES (N'19dfd788-264e-452c-6a24-08dc6ecfd2d9', N'4a4b1ee9-fa8f-49a4-66e8-08dc6eb4cc0a', N'DD', 50)
INSERT [dbo].[StudentSubjects] ([StudentSubjectsId], [StudentId], [SubjectName], [Marks]) VALUES (N'c7e45c27-3334-450f-6a25-08dc6ecfd2d9', N'4a4b1ee9-fa8f-49a4-66e8-08dc6eb4cc0a', N'DE', 45)
INSERT [dbo].[StudentSubjects] ([StudentSubjectsId], [StudentId], [SubjectName], [Marks]) VALUES (N'f23ac359-ead1-4602-6a26-08dc6ecfd2d9', N'4a4b1ee9-fa8f-49a4-66e8-08dc6eb4cc0a', N'EDE', 75)
INSERT [dbo].[StudentSubjects] ([StudentSubjectsId], [StudentId], [SubjectName], [Marks]) VALUES (N'adac9bf8-47bf-4b7f-6a27-08dc6ecfd2d9', N'4a4b1ee9-fa8f-49a4-66e8-08dc6eb4cc0a', N'DE', 90)
INSERT [dbo].[StudentSubjects] ([StudentSubjectsId], [StudentId], [SubjectName], [Marks]) VALUES (N'd94727b2-5b64-4e5c-6a28-08dc6ecfd2d9', N'4a4b1ee9-fa8f-49a4-66e8-08dc6eb4cc0a', N'dEDE', 78)
INSERT [dbo].[StudentSubjects] ([StudentSubjectsId], [StudentId], [SubjectName], [Marks]) VALUES (N'af275829-2ffc-4b84-91df-08dc6ed626a8', N'5107c1fb-8508-4cd3-715d-08dc6ec54a99', N'WS', 33)
INSERT [dbo].[StudentSubjects] ([StudentSubjectsId], [StudentId], [SubjectName], [Marks]) VALUES (N'e3f02f1d-df85-4cc7-91e0-08dc6ed626a8', N'5107c1fb-8508-4cd3-715d-08dc6ec54a99', N'wde', 12)
INSERT [dbo].[StudentSubjects] ([StudentSubjectsId], [StudentId], [SubjectName], [Marks]) VALUES (N'd3a41c40-b20f-4ba2-91e1-08dc6ed626a8', N'5107c1fb-8508-4cd3-715d-08dc6ec54a99', N'sww', 54)
INSERT [dbo].[StudentSubjects] ([StudentSubjectsId], [StudentId], [SubjectName], [Marks]) VALUES (N'88e12604-70bd-4086-91e2-08dc6ed626a8', N'5107c1fb-8508-4cd3-715d-08dc6ec54a99', N'sw', 6)
INSERT [dbo].[StudentSubjects] ([StudentSubjectsId], [StudentId], [SubjectName], [Marks]) VALUES (N'5288aa74-ea38-4315-91e3-08dc6ed626a8', N'5107c1fb-8508-4cd3-715d-08dc6ec54a99', N'swswfw', 85)
GO
/****** Object:  Index [IX_StudentSubjects_StudentId]    Script Date: 2024-05-08 02:20:02 ******/
CREATE NONCLUSTERED INDEX [IX_StudentSubjects_StudentId] ON [dbo].[StudentSubjects]
(
	[StudentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[StudentSubjects]  WITH CHECK ADD  CONSTRAINT [FK_StudentSubjects_Students_StudentId] FOREIGN KEY([StudentId])
REFERENCES [dbo].[Students] ([StudentId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StudentSubjects] CHECK CONSTRAINT [FK_StudentSubjects_Students_StudentId]
GO
USE [master]
GO
ALTER DATABASE [InterviewDB] SET  READ_WRITE 
GO