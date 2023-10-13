namespace Pizzeria.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DettagliOrdine",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ID_Ordine = c.Int(),
                        ID_Prodotto = c.Int(),
                        Quantità = c.Int(nullable: false),
                        PrezzoTotale = c.Decimal(nullable: false, precision: 10, scale: 2),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Ordini", t => t.ID_Ordine)
                .ForeignKey("dbo.Prodotti", t => t.ID_Prodotto)
                .Index(t => t.ID_Ordine)
                .Index(t => t.ID_Prodotto);
            
            CreateTable(
                "dbo.Ordini",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ID_Utente = c.Int(),
                        DataOrdine = c.DateTime(),
                        IndirizzoSpedizione = c.String(nullable: false, maxLength: 500),
                        Note = c.String(),
                        Stato = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Utenti", t => t.ID_Utente)
                .Index(t => t.ID_Utente);
            
            CreateTable(
                "dbo.Utenti",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 255),
                        Email = c.String(nullable: false, maxLength: 255),
                        Password = c.String(nullable: false, maxLength: 255),
                        Ruolo = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Prodotti",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 255),
                        Foto = c.String(maxLength: 255),
                        Prezzo = c.Decimal(nullable: false, precision: 10, scale: 2),
                        TempoConsegna = c.Int(nullable: false),
                        Ingredienti = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DettagliOrdine", "ID_Prodotto", "dbo.Prodotti");
            DropForeignKey("dbo.Ordini", "ID_Utente", "dbo.Utenti");
            DropForeignKey("dbo.DettagliOrdine", "ID_Ordine", "dbo.Ordini");
            DropIndex("dbo.Ordini", new[] { "ID_Utente" });
            DropIndex("dbo.DettagliOrdine", new[] { "ID_Prodotto" });
            DropIndex("dbo.DettagliOrdine", new[] { "ID_Ordine" });
            DropTable("dbo.Prodotti");
            DropTable("dbo.Utenti");
            DropTable("dbo.Ordini");
            DropTable("dbo.DettagliOrdine");
        }
    }
}
