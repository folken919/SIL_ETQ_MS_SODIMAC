using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Sodimac.Cedis.Infraestructure.Models
{
    public partial class TblEtqEtiqueta
    {
        public long IdEtiqueta { get; set; }
        public long? IdEtiquetaPadre { get; set; }
        public string WaveNbr { get; set; }
        public string TcLpnId { get; set; }
        public string OAddress1 { get; set; }
        public string City { get; set; }
        public string StateProv { get; set; }
        public string PostalCode { get; set; }
        public string DName { get; set; }
        public string DAddress1 { get; set; }
        public string UbicacionTienda { get; set; }
        public string FamiliaSku { get; set; }
        public string TamanoCarton { get; set; }
        public string Sector { get; set; }
        public string Vas { get; set; }
        public string DCity { get; set; }
        public string DAddress3 { get; set; }
        public string DStateProv { get; set; }
        public string DPostalCode { get; set; }
        public string DeliveryStartDttm { get; set; }
        public string CiudadDestino { get; set; }
        public string ItemName { get; set; }
        public string SkuDesc { get; set; }
        public string InitialQty { get; set; }
        public string PicDspLocn { get; set; }
        public string ConsDspLocn { get; set; }
        public string TcOrderId { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public string DeliveryEndDttm { get; set; }
        public string RefStopSeq { get; set; }
        public string TcShipmentId { get; set; }
        public string RefField2 { get; set; }
        public byte IdEstado { get; set; }
        public string UsuarioAsignado { get; set; }
        public DateTime? FechaAsignacion { get; set; }
        public decimal? IdLoteImpresion { get; set; }
        public string CodeImpresion { get; set; }
        public string Imprimible { get; set; }
        public DateTime? FechaImpresion { get; set; }
        public decimal? Impresiones { get; set; }
        public string Integracion { get; set; }
        public string Cargado { get; set; }
        public string LocationCedis { get; set; }
        public string LocationTipoUbicacion { get; set; }
        public string LocationArea { get; set; }
        public string LocationZona { get; set; }
        public string LocationAisle { get; set; }
        public string LocationBay { get; set; }
        public string LocationNivel { get; set; }
        public string LocationPosition { get; set; }
        public string Ref1 { get; set; }
        public string Ref2 { get; set; }
        public string Ref3 { get; set; }
        public string Ref4 { get; set; }
        public string Ref5 { get; set; }
        public long? IdXml { get; set; }
        public string UsrCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsrModificacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string UsrHostCreacion { get; set; }
        public string UsrDbCreacion { get; set; }
        public string UsrSoCreacion { get; set; }
        public string UsrHostModificacion { get; set; }
        public string UsrDbModificacion { get; set; }
        public string UsrSoModificacion { get; set; }
        public string Ref6 { get; set; }
        public string Ref7 { get; set; }
        public string Ref8 { get; set; }
        public string Ref9 { get; set; }
        public string Ref10 { get; set; }

        public virtual TblEtqOla WaveNbrNavigation { get; set; }
    }
}
