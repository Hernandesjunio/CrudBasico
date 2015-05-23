use Master
go
Create database CrudBasico
go
CREATE TABLE [dbo].[Mensagem] (
    [MensagemID] INT          IDENTITY (1, 1) NOT NULL,
    [Descricao]  VARCHAR (50) NOT NULL,
    [DtCriacao]  DATETIME     NOT NULL,
    PRIMARY KEY CLUSTERED ([MensagemID] ASC)
);
go

CREATE TABLE [dbo].[MensagemUsuario] (
    [MensagemUsuarioID] INT          IDENTITY (1, 1) NOT NULL,
    [MensagemID]        INT          NOT NULL,
    [Destinatario]      VARCHAR (50) NOT NULL,
    [DtCriacao]         DATETIME     NOT NULL,
    [UsuarioCriacao]    VARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([MensagemUsuarioID] ASC),
    CONSTRAINT [FK_MensagemUsuario_Mensagem] FOREIGN KEY ([MensagemID]) REFERENCES [dbo].[Mensagem] ([MensagemID])
);

go
