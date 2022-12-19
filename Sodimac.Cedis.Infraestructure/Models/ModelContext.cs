using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Sodimac.Cedis.Core.Utils;
using Sodimac.Cedis.Infraestructure.Data;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Sodimac.Cedis.Infraestructure.Models
{
    public partial class ModelContext : DbContext
    {

        private readonly IConfiguration _configuration;
        public ModelContext()
        {
        }

        public ModelContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }



        public ModelContext(DbContextOptions<ModelContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblEtqEtiqueta> TblEtqEtiqueta { get; set; }
        public virtual DbSet<TblEtqOla> TblEtqOla { get; set; }
        public virtual DbSet<TblEtqZona> TblEtqZona { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseOracle(_configuration.GetConnectionString("sgl_prod").Desencriptar());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:DefaultSchema", "SGL");

            modelBuilder.Entity<TblEtqEtiqueta>(entity =>
            {
                entity.HasKey(e => e.IdEtiqueta)
                    .HasName("TBL_ETQ_ETIQUETA_PK");

                entity.ToTable("TBL_ETQ_ETIQUETA");

                entity.HasIndex(e => e.FechaCreacion)
                    .HasName("IX_ETQ_FECHA_CREACION");

                entity.HasIndex(e => e.IdLoteImpresion)
                    .HasName("IX_ETQ_LOTE_IMPRESION");

                entity.HasIndex(e => e.PurchaseOrderNumber)
                    .HasName("IX_ETQ_PURCHASE_ORDER");

                entity.HasIndex(e => e.TcLpnId)
                    .HasName("IX_ETQ_LPN_ID");

                entity.HasIndex(e => e.TcShipmentId)
                    .HasName("IX_ETQ_SHIPMENT_ID");

                entity.HasIndex(e => e.WaveNbr)
                    .HasName("IX_ETQ_WAVE_NBR");

                entity.HasIndex(e => new { e.Ref1, e.IdEstado, e.UsuarioAsignado })
                    .HasName("IX_ETQ_REF1");

                entity.HasIndex(e => new { e.LocationZona, e.LocationArea, e.LocationCedis, e.UsuarioAsignado, e.IdEstado })
                    .HasName("IX_ETQ_LOCATION");

                entity.Property(e => e.IdEtiqueta)
                    .HasColumnName("ID_ETIQUETA")
                    .HasColumnType("NUMBER(15)")
                    .ValueGeneratedNever();

                entity.Property(e => e.Cargado)
                    .HasColumnName("CARGADO")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasColumnName("CITY")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CiudadDestino)
                    .HasColumnName("CIUDAD_DESTINO")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CodeImpresion)
                    .HasColumnName("CODE_IMPRESION")
                    .HasColumnType("CLOB");

                entity.Property(e => e.ConsDspLocn)
                    .HasColumnName("CONS_DSP_LOCN")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DAddress1)
                    .HasColumnName("D_ADDRESS_1")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DAddress3)
                    .HasColumnName("D_ADDRESS_3")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DCity)
                    .HasColumnName("D_CITY")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DName)
                    .HasColumnName("D_NAME")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DPostalCode)
                    .HasColumnName("D_POSTAL_CODE")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DStateProv)
                    .HasColumnName("D_STATE_PROV")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DeliveryEndDttm)
                    .HasColumnName("DELIVERY_END_DTTM")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DeliveryStartDttm)
                    .HasColumnName("DELIVERY_START_DTTM")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FamiliaSku)
                    .HasColumnName("FAMILIA_SKU")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaAsignacion)
                    .HasColumnName("FECHA_ASIGNACION")
                    .HasColumnType("DATE");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnName("FECHA_CREACION")
                    .HasColumnType("DATE");

                entity.Property(e => e.FechaImpresion)
                    .HasColumnName("FECHA_IMPRESION")
                    .HasColumnType("DATE");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnName("FECHA_MODIFICACION")
                    .HasColumnType("DATE");

                entity.Property(e => e.IdEstado).HasColumnName("ID_ESTADO");

                entity.Property(e => e.IdEtiquetaPadre)
                    .HasColumnName("ID_ETIQUETA_PADRE")
                    .HasColumnType("NUMBER(15)");

                entity.Property(e => e.IdLoteImpresion)
                    .HasColumnName("ID_LOTE_IMPRESION")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.IdXml)
                    .HasColumnName("ID_XML")
                    .HasColumnType("NUMBER(15)");

                entity.Property(e => e.Impresiones)
                    .HasColumnName("IMPRESIONES")
                    .HasColumnType("NUMBER");

                entity.Property(e => e.Imprimible)
                    .HasColumnName("IMPRIMIBLE")
                    .HasColumnType("CHAR(1)");

                entity.Property(e => e.InitialQty)
                    .HasColumnName("INITIAL_QTY")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Integracion)
                    .HasColumnName("INTEGRACION")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.ItemName)
                    .HasColumnName("ITEM_NAME")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LocationAisle)
                    .IsRequired()
                    .HasColumnName("LOCATION_AISLE")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LocationArea)
                    .IsRequired()
                    .HasColumnName("LOCATION_AREA")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LocationBay)
                    .IsRequired()
                    .HasColumnName("LOCATION_BAY")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LocationCedis)
                    .IsRequired()
                    .HasColumnName("LOCATION_CEDIS")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LocationNivel)
                    .IsRequired()
                    .HasColumnName("LOCATION_NIVEL")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LocationPosition)
                    .IsRequired()
                    .HasColumnName("LOCATION_POSITION")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LocationTipoUbicacion)
                    .HasColumnName("LOCATION_TIPO_UBICACION")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LocationZona)
                    .IsRequired()
                    .HasColumnName("LOCATION_ZONA")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.OAddress1)
                    .HasColumnName("O_ADDRESS_1")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PicDspLocn)
                    .HasColumnName("PIC_DSP_LOCN")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PostalCode)
                    .HasColumnName("POSTAL_CODE")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PurchaseOrderNumber)
                    .HasColumnName("PURCHASE_ORDER_NUMBER")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Ref1)
                    .HasColumnName("REF1")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Ref10)
                    .HasColumnName("REF10")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Ref2)
                    .HasColumnName("REF2")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Ref3)
                    .HasColumnName("REF3")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Ref4)
                    .HasColumnName("REF4")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Ref5)
                    .HasColumnName("REF5")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Ref6)
                    .HasColumnName("REF6")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Ref7)
                    .HasColumnName("REF7")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Ref8)
                    .HasColumnName("REF8")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Ref9)
                    .HasColumnName("REF9")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RefField2)
                    .HasColumnName("REF_FIELD_2")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RefStopSeq)
                    .HasColumnName("REF_STOP_SEQ")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Sector)
                    .HasColumnName("SECTOR")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SkuDesc)
                    .HasColumnName("SKU_DESC")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.StateProv)
                    .HasColumnName("STATE_PROV")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TamanoCarton)
                    .HasColumnName("TAMANO_CARTON")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TcLpnId)
                    .HasColumnName("TC_LPN_ID")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TcOrderId)
                    .HasColumnName("TC_ORDER_ID")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TcShipmentId)
                    .HasColumnName("TC_SHIPMENT_ID")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UbicacionTienda)
                    .HasColumnName("UBICACION_TIENDA")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UsrCreacion)
                    .HasColumnName("USR_CREACION")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UsrDbCreacion)
                    .IsRequired()
                    .HasColumnName("USR_DB_CREACION")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UsrDbModificacion)
                    .HasColumnName("USR_DB_MODIFICACION")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UsrHostCreacion)
                    .IsRequired()
                    .HasColumnName("USR_HOST_CREACION")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UsrHostModificacion)
                    .HasColumnName("USR_HOST_MODIFICACION")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UsrModificacion)
                    .HasColumnName("USR_MODIFICACION")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UsrSoCreacion)
                    .IsRequired()
                    .HasColumnName("USR_SO_CREACION")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UsrSoModificacion)
                    .HasColumnName("USR_SO_MODIFICACION")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UsuarioAsignado)
                    .HasColumnName("USUARIO_ASIGNADO")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Vas)
                    .HasColumnName("VAS")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.WaveNbr)
                    .IsRequired()
                    .HasColumnName("WAVE_NBR")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.WaveNbrNavigation)
                    .WithMany(p => p.TblEtqEtiqueta)
                    .HasForeignKey(d => d.WaveNbr)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("OLA_ETIQUETA_FK");
            });

            modelBuilder.Entity<TblEtqOla>(entity =>
            {
                entity.HasKey(e => e.WaveNbr)
                    .HasName("TBL_ETQ_OLA_PK");

                entity.ToTable("TBL_ETQ_OLA");

                entity.HasIndex(e => e.FechaCreacion)
                    .HasName("IX_ETQ_OLA_FECHA");

                entity.Property(e => e.WaveNbr)
                    .HasColumnName("WAVE_NBR")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion)
                    .HasColumnName("FECHA_CREACION")
                    .HasColumnType("DATE");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnName("FECHA_MODIFICACION")
                    .HasColumnType("DATE");

                entity.Property(e => e.FechaWaveNbr)
                    .HasColumnName("FECHA_WAVE_NBR")
                    .HasColumnType("DATE");

                entity.Property(e => e.IdEstado).HasColumnName("ID_ESTADO");

                entity.Property(e => e.TipoEtiqueta)
                    .IsRequired()
                    .HasColumnName("TIPO_ETIQUETA")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TipoOla)
                    .IsRequired()
                    .HasColumnName("TIPO_OLA")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.UsrDbCreacion)
                    .IsRequired()
                    .HasColumnName("USR_DB_CREACION")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UsrDbModificacion)
                    .HasColumnName("USR_DB_MODIFICACION")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UsrHostCreacion)
                    .IsRequired()
                    .HasColumnName("USR_HOST_CREACION")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UsrHostModificacion)
                    .HasColumnName("USR_HOST_MODIFICACION")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UsrSoCreacion)
                    .IsRequired()
                    .HasColumnName("USR_SO_CREACION")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UsrSoModificacion)
                    .HasColumnName("USR_SO_MODIFICACION")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblEtqZona>(entity =>
            {
                entity.HasKey(e => new { e.Zona, e.Area, e.Cedis })
                    .HasName("TBL_ETQ_ZONA_PK");

                entity.ToTable("TBL_ETQ_ZONA");

                entity.Property(e => e.Zona)
                    .HasColumnName("ZONA")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Area)
                    .HasColumnName("AREA")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Cedis)
                    .HasColumnName("CEDIS")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasColumnName("DESCRIPCION")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaCreacion)
                    .HasColumnName("FECHA_CREACION")
                    .HasColumnType("DATE");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnName("FECHA_MODIFICACION")
                    .HasColumnType("DATE");

                entity.Property(e => e.Tope).HasColumnName("TOPE");

                entity.Property(e => e.UsrDbCreacion)
                    .IsRequired()
                    .HasColumnName("USR_DB_CREACION")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UsrDbModificacion)
                    .HasColumnName("USR_DB_MODIFICACION")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UsrHostCreacion)
                    .IsRequired()
                    .HasColumnName("USR_HOST_CREACION")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UsrHostModificacion)
                    .HasColumnName("USR_HOST_MODIFICACION")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UsrSoCreacion)
                    .IsRequired()
                    .HasColumnName("USR_SO_CREACION")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UsrSoModificacion)
                    .HasColumnName("USR_SO_MODIFICACION")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.HasSequence("CONTROL");

            modelBuilder.HasSequence("GP_SEQ_ID_DETALLE_SOLICITUD");

            modelBuilder.HasSequence("GP_SEQ_ID_SOLICITUD");

            modelBuilder.HasSequence("NOTIFICAR_TEAMS_SEQUENCE");

            modelBuilder.HasSequence("OAU_TIPO_OLA_GRUPOS");

            modelBuilder.HasSequence("SEC_ORDEN_COMPRA_MANUAL");

            modelBuilder.HasSequence("SEQ_ABAS_CANT_DIST_TRAN");

            modelBuilder.HasSequence("SEQ_ABAS_DETALLE_PEDIDO");

            modelBuilder.HasSequence("SEQ_ABAS_PEDIDO");

            modelBuilder.HasSequence("SEQ_API_PETI_PEDIDO");

            modelBuilder.HasSequence("SEQ_BOTS_BARCODES");

            modelBuilder.HasSequence("SEQ_BOTS_BINORDERS");

            modelBuilder.HasSequence("SEQ_BOTS_ESTADO_INTEGRACION");

            modelBuilder.HasSequence("SEQ_BOTS_EXCEPCIONES");

            modelBuilder.HasSequence("SEQ_BOTS_GTPSTATUS");

            modelBuilder.HasSequence("SEQ_BOTS_INTEGRACION");

            modelBuilder.HasSequence("SEQ_BOTS_ITEM_MASTER");

            modelBuilder.HasSequence("SEQ_BOTS_NOTIFICACIONES");

            modelBuilder.HasSequence("SEQ_BOTS_PICK");

            modelBuilder.HasSequence("SEQ_BOTS_PICK_DETAIL");

            modelBuilder.HasSequence("SEQ_BOTS_PUT");

            modelBuilder.HasSequence("SEQ_BOTS_TIPO_INTEGRACION");

            modelBuilder.HasSequence("SEQ_BPA_LPN_NUMBER");

            modelBuilder.HasSequence("SEQ_CLA_CONSOLA");

            modelBuilder.HasSequence("SEQ_CLA_LOG_EJECUCION");

            modelBuilder.HasSequence("SEQ_CLA_LOG_EJECUCION_DTL");

            modelBuilder.HasSequence("SEQ_CLA_TIPO_CONSOLA");

            modelBuilder.HasSequence("SEQ_DET_ALISTAMIENTO");

            modelBuilder.HasSequence("SEQ_DM_SGL_INT_CAMPOS_WMS_PK");

            modelBuilder.HasSequence("SEQ_DM_SGL_INT_TABLA_WMS_PK");

            modelBuilder.HasSequence("SEQ_DSP_CONF_CD");

            modelBuilder.HasSequence("SEQ_DSP_DIF_INVENTARIO_AUD");

            modelBuilder.HasSequence("SEQ_ETQ_CAMPO");

            modelBuilder.HasSequence("SEQ_ETQ_ETIQUETA");

            modelBuilder.HasSequence("SEQ_ETQ_ETIQUETA_AUD");

            modelBuilder.HasSequence("SEQ_ETQ_ETIQUETA_XML");

            modelBuilder.HasSequence("SEQ_ETQ_LOTE_IMPRESION");

            modelBuilder.HasSequence("SEQ_ETQ_MENSAJES_ERROR");

            modelBuilder.HasSequence("SEQ_ETQ_TITULO");

            modelBuilder.HasSequence("SEQ_ID_AUD_STAGE");

            modelBuilder.HasSequence("SEQ_ID_CANAL_EJECUCION_BAJO");

            modelBuilder.HasSequence("SEQ_ID_CANAL_EJECUCION_MEDIO");

            modelBuilder.HasSequence("SEQ_ID_LOAD");

            modelBuilder.HasSequence("SEQ_ID_OL_INV_EXTERNO");

            modelBuilder.HasSequence("SEQ_INV_CONF_CALCULO_WMS");

            modelBuilder.HasSequence("SEQ_INV_DISTRIBUCION");

            modelBuilder.HasSequence("SEQ_INV_ECUACION_DET_WMS");

            modelBuilder.HasSequence("SEQ_INV_ECUACION_ENC_WMS");

            modelBuilder.HasSequence("SEQ_INV_INVENTARIO_WMS");

            modelBuilder.HasSequence("SEQ_INV_MAPEO_CAMPO_WMS");

            modelBuilder.HasSequence("SEQ_INV_MOV_INVENTARIO");

            modelBuilder.HasSequence("SEQ_INV_PARAMETRO");

            modelBuilder.HasSequence("SEQ_INV_RESERVA");

            modelBuilder.HasSequence("SEQ_INV_RESERVA_INTENTOS");

            modelBuilder.HasSequence("SEQ_INV_RESERVA_INTENTOS_DTL");

            modelBuilder.HasSequence("SEQ_INV_RESERVA_LOTE");

            modelBuilder.HasSequence("SEQ_INVC_BATCH_NBR");

            modelBuilder.HasSequence("SEQ_ITEM_AUDITORIA_ID_PK");

            modelBuilder.HasSequence("SEQ_LM_COMPL_AUD");

            modelBuilder.HasSequence("SEQ_LM_DATOS_ADIC_AUD");

            modelBuilder.HasSequence("SEQ_LM_ITEM_AUD");

            modelBuilder.HasSequence("SEQ_LOG_MENSAJES_ERROR");

            modelBuilder.HasSequence("SEQ_MF_COORDENADAS");

            modelBuilder.HasSequence("SEQ_MF_TBL_MF_SHIPMENT");

            modelBuilder.HasSequence("SEQ_MF_TBL_MF_TRACKING");

            modelBuilder.HasSequence("SEQ_OAU_CONFIGURA_DET");

            modelBuilder.HasSequence("SEQ_OAU_CONFIGURA_DET_AUD");

            modelBuilder.HasSequence("SEQ_OAU_CONFIGURA_OLA");

            modelBuilder.HasSequence("SEQ_OAU_CONFIGURA_OLA_AUD");

            modelBuilder.HasSequence("SEQ_OAU_EJEC_DET");

            modelBuilder.HasSequence("SEQ_OAU_EJECUCION");

            modelBuilder.HasSequence("SEQ_OAU_GRUPOS");

            modelBuilder.HasSequence("SEQ_OAU_INFORMACION_NOVEDADES");

            modelBuilder.HasSequence("SEQ_OAU_LOG");

            modelBuilder.HasSequence("SEQ_OAU_NOVEDADES_INFO");

            modelBuilder.HasSequence("SEQ_OAU_PASA_XML");

            modelBuilder.HasSequence("SEQ_OAU_TIPO_OLA");

            modelBuilder.HasSequence("SEQ_OAU_TRANSPORTADORA");

            modelBuilder.HasSequence("SEQ_OAU_VALIDA_DETALLE");

            modelBuilder.HasSequence("SEQ_OC_COMER");

            modelBuilder.HasSequence("SEQ_OC_COMER_TEMP");

            modelBuilder.HasSequence("SEQ_OLA_ETQ");

            modelBuilder.HasSequence("SEQ_OMS_CAP_AJUSTE");

            modelBuilder.HasSequence("SEQ_OMS_PETICION_DESPACHO");

            modelBuilder.HasSequence("SEQ_OMS_ZONA_DIV_GEO");

            modelBuilder.HasSequence("SEQ_ORDERS_ID_1");

            modelBuilder.HasSequence("SEQ_OUTPT_LPN_DETAIL_ID_1");

            modelBuilder.HasSequence("SEQ_OUTPT_LPN_ID_1");

            modelBuilder.HasSequence("SEQ_OUTPT_ORDER_LINE_ITEM_ID_1");

            modelBuilder.HasSequence("SEQ_PETI_CONT_CARG_FI");

            modelBuilder.HasSequence("SEQ_QBR_HIST_QUIEBRES");

            modelBuilder.HasSequence("SEQ_RCB_PIX_TRAN");

            modelBuilder.HasSequence("SEQ_RCB_SHIP_ORDERS");

            modelBuilder.HasSequence("SEQ_RCB_SHIP_TRAN");

            modelBuilder.HasSequence("SEQ_RPA_CONFIG_EXCEPCION");

            modelBuilder.HasSequence("SEQ_RPA_PICK_DTL_ATTEMP");

            modelBuilder.HasSequence("SEQ_RPA_PICK_STEP");

            modelBuilder.HasSequence("SEQ_RPA_PUT");

            modelBuilder.HasSequence("SEQ_RPA_PUT_ATTEMP");

            modelBuilder.HasSequence("SEQ_RPA_PUT_STEP");

            modelBuilder.HasSequence("SEQ_RPG_ALISTAMIENTO_ASIGNA");

            modelBuilder.HasSequence("SEQ_RPG_DEVOLUCION_ASIGNA");

            modelBuilder.HasSequence("SEQ_RPG_ESTADO_REPROGRAMA_HIST");

            modelBuilder.HasSequence("SEQ_RPG_ESTADO_REPROGRAMACION");

            modelBuilder.HasSequence("SEQ_RPG_LPN");

            modelBuilder.HasSequence("SEQ_RPG_LPN_DETALLE");

            modelBuilder.HasSequence("SEQ_RPG_NIVEL");

            modelBuilder.HasSequence("SEQ_RPG_ORIGEN_REPROGRAMACION");

            modelBuilder.HasSequence("SEQ_RPG_POSICION");

            modelBuilder.HasSequence("SEQ_RPG_RECIBO");

            modelBuilder.HasSequence("SEQ_RPG_RECIBO_DETALLE");

            modelBuilder.HasSequence("SEQ_RPG_REPROGRAMACION");

            modelBuilder.HasSequence("SEQ_RPG_TAREA_ALISTAMIENTO");

            modelBuilder.HasSequence("SEQ_RPG_TAREA_ALISTAMIENTO_DET");

            modelBuilder.HasSequence("SEQ_RPG_TAREA_DEVOLUCION");

            modelBuilder.HasSequence("SEQ_RPG_TAREA_DEVOLUCION_DET");

            modelBuilder.HasSequence("SEQ_RPG_TRANSAC_XML");

            modelBuilder.HasSequence("SEQ_RPG_TRANSACCION");

            modelBuilder.HasSequence("SEQ_RPG_UBICACION");

            modelBuilder.HasSequence("SEQ_RPG_USUARIO");

            modelBuilder.HasSequence("SEQ_SAPS_ALIST_ZONA_TDA");

            modelBuilder.HasSequence("SEQ_SGL_CANAL_EJECUCION");

            modelBuilder.HasSequence("SEQ_SGL_CM_TEMP");

            modelBuilder.HasSequence("SEQ_SGL_CONS_CM");

            modelBuilder.HasSequence("SEQ_SGL_ID_EJECUCION");

            modelBuilder.HasSequence("SEQ_SGL_PRIORIDAD_EJEC");

            modelBuilder.HasSequence("SEQ_SGL_SQL_INSPECCION");

            modelBuilder.HasSequence("SEQ_SGL_SQL_INVALIDOS");

            modelBuilder.HasSequence("SEQ_SGL_TAREA_EJECUCION");

            modelBuilder.HasSequence("SEQ_SGL_TAREA_EJECUCION_MSJ");

            modelBuilder.HasSequence("SEQ_SGL_TOKEN");

            modelBuilder.HasSequence("SEQ_SHM_DC");

            modelBuilder.HasSequence("SEQ_SHM_DT");

            modelBuilder.HasSequence("SEQ_SHM_LOTE_INTEGRACION");

            modelBuilder.HasSequence("SEQ_SHM_MF");

            modelBuilder.HasSequence("SEQ_SHM_NP");

            modelBuilder.HasSequence("SEQ_SLT_ACCIONES");

            modelBuilder.HasSequence("SEQ_SLT_FORMULARIOS");

            modelBuilder.HasSequence("SEQ_TBL_AUD_SLTGENBULT");

            modelBuilder.HasSequence("SEQ_TBL_AUD_USER_ODBMS");

            modelBuilder.HasSequence("SEQ_TBL_AZ_DEPLOY_DTL");

            modelBuilder.HasSequence("SEQ_TBL_AZ_DEPLOY_HD");

            modelBuilder.HasSequence("SEQ_TBL_DIN_TRAZA_EJECUCION");

            modelBuilder.HasSequence("SEQ_TBL_DSP_CARGUE");

            modelBuilder.HasSequence("SEQ_TBL_DSP_CARGUE_LN");

            modelBuilder.HasSequence("SEQ_TBL_ETQ_TRANSPORTADORA");

            modelBuilder.HasSequence("SEQ_TBL_INV_CALCULO_RELAC_PK");

            modelBuilder.HasSequence("SEQ_TBL_INV_NP_RESERVA_PK");

            modelBuilder.HasSequence("SEQ_TBL_INV_RESERVA_ASIGNA_PK");

            modelBuilder.HasSequence("SEQ_TBL_PD_TRAZA_EJECUCION");

            modelBuilder.HasSequence("SEQ_TBL_PRG_PURGAS");

            modelBuilder.HasSequence("SEQ_TBL_PRG_PURGAS_EJECUCION");

            modelBuilder.HasSequence("SEQ_TBL_QBR_QUIEBRES");

            modelBuilder.HasSequence("SEQ_TBL_QBR_QUIEBRES_CAUSAL");

            modelBuilder.HasSequence("SEQ_TBL_RPG_ASIGNA_PERMISOS");

            modelBuilder.HasSequence("SEQ_TBL_RPG_CARGUE");

            modelBuilder.HasSequence("SEQ_TBL_RPG_CARGUE_LN");

            modelBuilder.HasSequence("SEQ_TBL_RPG_PERMISOS_ROL");

            modelBuilder.HasSequence("SEQ_TBL_SGL_AUDITORIA_PROCESO");

            modelBuilder.HasSequence("SEQ_TBL_SGL_CONS_RESERVAS");

            modelBuilder.HasSequence("SEQ_TBL_SGL_ID_SESSION_AUDITO");

            modelBuilder.HasSequence("SEQ_TBL_SLI_MER_RECL_LIQUI");

            modelBuilder.HasSequence("SEQ_TBL_SLI_MERMA_DOCUM");

            modelBuilder.HasSequence("SEQ_TBL_SLI_MERMA_RECLAMACION");

            modelBuilder.HasSequence("SEQ_TBL_SLI_MERMA_SALVA_DETAL");

            modelBuilder.HasSequence("SEQ_TBL_SLI_MERMA_SALVAMENTO");

            modelBuilder.HasSequence("SEQ_TBL_WMS_INVENTARIO_AUD");

            modelBuilder.HasSequence("SEQ_WMR_REQUEST");

            modelBuilder.HasSequence("SQ_ID_AUD_OMS_DET_CALENDARIO");

            modelBuilder.HasSequence("SQ_ID_CANAL_EJECUCION");

            modelBuilder.HasSequence("SQ_ID_ID_STAGE");

            modelBuilder.HasSequence("SQ_ID_INT_STAGE");

            modelBuilder.HasSequence("SQ_LOG_ID_APLICACION");

            modelBuilder.HasSequence("SQ_SGL_ID_SESION_JWT");

            modelBuilder.HasSequence("SQ_SGL_ID_USUARIO_RPA");

            modelBuilder.HasSequence("SQ_SGL_SQL_ID_AUD_TMP");

            modelBuilder.HasSequence("SQ_SGL_SQL_ID_ESTADISITICA");

            modelBuilder.HasSequence("SQ_TIP_ID_TIP");

            modelBuilder.HasSequence("SQ_TIP_ID_TIP_DTL");

            modelBuilder.HasSequence("SQ_TIP_ID_TIP_STAGE");

            modelBuilder.HasSequence("TBL_OMS_CATALYST_AU");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
