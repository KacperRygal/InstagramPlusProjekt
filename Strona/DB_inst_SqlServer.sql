CREATE TABLE [Uzytkownicy] (
    [id_uzytkownik] INT             NOT NULL IDENTITY,
    [nazwa]         NVARCHAR (255)  NULL,
    [haslo]         NVARCHAR (255)  NULL,
    [zdjecie]       VARBINARY (MAX) NULL,
    [vip_do]        DATE            NULL,
    [moderator]     TINYINT         NULL,
    PRIMARY KEY CLUSTERED ([id_uzytkownik] ASC)
)
GO

CREATE TABLE [Posty] (
  [id_posty] int PRIMARY KEY,
  [opis] nvarchar(255),
  [zdjecie] blob,
  [id_komentarz] int,
  [id_uzytkownik] int
)
GO

CREATE TABLE [Komentarze] (
  [id_komentarz] int PRIMARY KEY,
  [id_post] int,
  [tresc] nvarchar(255),
  [id_uzytkownik] int
)
GO

CREATE TABLE [Polubienia_Posty] (
  [id_uzytkownik] int,
  [id_post] int,
  PRIMARY KEY ([id_uzytkownik], [id_post])
)
GO

CREATE TABLE [Polubienia_Komentarze] (
  [id_polubienia] int,
  [id_uzytkownik] int,
  [id_komentarz] int
)
GO

CREATE TABLE [Obserwowani] (
  [id_obserwowanego] int,
  [id_obserwatora] int
)
GO

CREATE TABLE [Obserwujacy] (
  [id_obserwowanego] int,
  [id_obserwatora] int
)
GO

ALTER TABLE [Obserwowani] ADD FOREIGN KEY ([id_obserwowanego]) REFERENCES [Uzytkownicy] ([id_uzytkownik])
GO

ALTER TABLE [Obserwowani] ADD FOREIGN KEY ([id_obserwatora]) REFERENCES [Uzytkownicy] ([id_uzytkownik])
GO

ALTER TABLE [Obserwujacy] ADD FOREIGN KEY ([id_obserwowanego]) REFERENCES [Uzytkownicy] ([id_uzytkownik])
GO

ALTER TABLE [Obserwujacy] ADD FOREIGN KEY ([id_obserwatora]) REFERENCES [Uzytkownicy] ([id_uzytkownik])
GO

ALTER TABLE [Komentarze] ADD FOREIGN KEY ([id_post]) REFERENCES [Posty] ([id_posty])
GO

ALTER TABLE [Komentarze] ADD FOREIGN KEY ([id_uzytkownik]) REFERENCES [Uzytkownicy] ([id_uzytkownik])
GO

ALTER TABLE [Posty] ADD FOREIGN KEY ([id_uzytkownik]) REFERENCES [Uzytkownicy] ([id_uzytkownik])
GO

ALTER TABLE [Polubienia_Posty] ADD FOREIGN KEY ([id_uzytkownik]) REFERENCES [Uzytkownicy] ([id_uzytkownik])
GO

ALTER TABLE [Polubienia_Posty] ADD FOREIGN KEY ([id_post]) REFERENCES [Posty] ([id_posty])
GO

ALTER TABLE [Polubienia_Komentarze] ADD FOREIGN KEY ([id_uzytkownik]) REFERENCES [Uzytkownicy] ([id_uzytkownik])
GO

ALTER TABLE [Polubienia_Komentarze] ADD FOREIGN KEY ([id_komentarz]) REFERENCES [Komentarze] ([id_komentarz])
GO
