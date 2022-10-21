using System.Collections.Generic;
using ShipmentCommonClasses.Dtos;

namespace ShipmentCommonClasses.CommonDtos
{
    public class DetailedDataModel
    {
        public List<ShipmentListData> detailedData { get; set; }

        public int recordsCount { get; set; }
    }
}