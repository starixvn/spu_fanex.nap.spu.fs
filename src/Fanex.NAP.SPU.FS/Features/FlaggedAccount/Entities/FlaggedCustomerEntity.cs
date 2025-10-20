namespace Fanex.NAP.SPU.FS.Features.FlaggedAccount.Entities
{
    using Fanex.NAP.SPU.FS.Shares;  
    using System.Xml.Serialization;

    [XName("CustomerEvidence")]
    public class FlaggedCustomerEntity
    {
        public int CustId { get; set; }

        public string EvidenceCode { get; set; }

        [XmlIgnore]
        public string EvidenceName { get; set; }
    }
}
