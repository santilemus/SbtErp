select Oid, ObjectTypeName, Name, ParametersObjectTypeName, IsInPlaceReport, 
       PredefinedReportType,
       convert(xml,(convert(varbinary(max), Content))) as Content 
  from ReportDataV2