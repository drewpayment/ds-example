namespace Dominion.Utility.DataExport.Misc
{
    public enum LogType
    {
        ObjToJson,
        DsToXml,

        /// <summary>
        /// DataSet or table to xml with schema data.
        /// </summary>
        DsToXmlWS,
        
        /// <summary>
        /// Creates an html page to view the schema and data of a DS.
        /// </summary>
        DsToHtml,

        /// <summary>
        /// Creates DT schema and data into insert statements.
        /// </summary>
        DtToInsertSql,
    }

    public enum Ext
    {
        Html,
        Json,
        Txt,
        Xml,
        Sql,
    }
}
